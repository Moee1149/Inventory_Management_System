using Frontend.IService;
using Frontend.ViewModels;

namespace Frontend.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient http)
    {
        _httpClient = http;
    }

    public async Task<HttpResponseMessage> HandleUserLogin(UserViewModel user)
    {
        return await _httpClient.PostAsJsonAsync($"api/auth/login", user);
    }

    public async Task<HttpResponseMessage> HandleUserRegister(UserCreateViewModel newUser)
    {
        return await _httpClient.PostAsJsonAsync($"api/auth/register", newUser);
    }
}
