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
}