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

    public async Task<ApiResponseViewModel<List<ProductViewModel>>> GetAllProduct(string search = "", int pageNumber = 1)
    {
        var response = await _httpClient.GetAsync($"api/product?search={search}&page={pageNumber}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ApiResponseViewModel<List<ProductViewModel>>>();
        return result!;
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
