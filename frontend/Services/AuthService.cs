using System.IdentityModel.Tokens.Jwt;
using Frontend.IService;
using Frontend.ViewModels;

namespace Frontend.Services;

public class AuthService(IHttpContextAccessor _http, IApiClient _apiClient) : IAuthService
{
    public async Task<ApiResponseViewModel<string>> HandleUserLogin(UserViewModel user)
    {
        var response = await _apiClient.PostJsonAsync<UserViewModel, string>("api/auth/login", user);
        return response;
    }

    public async Task<ApiResponseViewModel<string>> HandleUserRegister(UserCreateViewModel newUser)
    {
        var response = await _apiClient.PostJsonAsync<UserCreateViewModel, string>("api/auth/register", newUser);
        return response;
    }

    public bool IsLoggenIn()
    {
        return _http.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }

    public void Logout()
    {
        _http.HttpContext?.Response.Cookies.Delete("accessToken");
    }

    public void SetTokenCookie(string token)
    {
        // Decode token to read expiration
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        // exp is in Unix timestamp
        var expUnix = jwt.Payload.Expiration ?? 0;
        var expDate = DateTimeOffset.FromUnixTimeSeconds(expUnix);

        // Add small buffer (e.g., 10 minutes)
        var cookieExpires = expDate.AddMinutes(10);
        _http.HttpContext?.Response.Cookies.Append("accessToken", token, new CookieOptions
        {
            HttpOnly = false,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = cookieExpires.UtcDateTime
        });
    }
}