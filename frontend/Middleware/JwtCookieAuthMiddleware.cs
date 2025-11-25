using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Frontend.Middleware;

public class JwtCookieAuthMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Cookies["accessToken"];

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                // Check expiration using jwt.Payload.Expiration directly
                var expUnix = jwt.Payload.Expiration ?? 0;
                var expDate = DateTimeOffset.FromUnixTimeSeconds(expUnix);

                if (expDate > DateTimeOffset.UtcNow)
                {
                    // Token is valid — set identity
                    var claims = jwt.Claims.ToList();
                    var identity = new ClaimsIdentity(claims, "CustomJwtAuth");
                    context.User = new ClaimsPrincipal(identity);

                    context.Request.Headers.Authorization = "Bearer " + token;
                }
                else
                {
                    context.Response.Cookies.Delete("accessToken");
                }
            }
            catch
            {
                context.Response.Cookies.Delete("accessToken");
            }
        }
        await _next(context);
    }
    private Dictionary<string, object>? DecodeJwtPayload(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(token);

            var json = JsonSerializer.Serialize(jwt.Payload);
            return JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        }
        catch
        {
            return null;
        }
    }

    private ClaimsPrincipal CreatePrincipalFromPayload(Dictionary<string, object> payload)
    {
        var claims = new List<Claim>();

        foreach (var item in payload)
        {
            if (item.Value != null)
            {
                claims.Add(new Claim(item.Key, item.Value.ToString()!));
            }
        }

        var identity = new ClaimsIdentity(claims, "CustomJwtAuth");
        return new ClaimsPrincipal(identity);
    }
}