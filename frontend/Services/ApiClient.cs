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

    public async Task<ApiResponseViewModel<T>> GetJsonAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetFromJsonAsync<ApiResponseViewModel<T>>(endpoint);
        return new ApiResponseViewModel<T>
        {
            Data = response!.Data,
            Message = response.Message,
            StatusCode = response.StatusCode
        };
    }

    public async Task<ApiResponseViewModel<T>> PostAsync<T>(string endpoint, MultipartFormDataContent data)
    {
        var response = await _httpClient.PostAsync(endpoint, data);
        var result = await response.Content.ReadFromJsonAsync<ApiResponseViewModel<T>>();
        return new ApiResponseViewModel<T>
        {
            Data = result!.Data,
            Message = result.Message,
            StatusCode = (int)response.StatusCode
        };
    }

    public async Task<ApiResponseViewModel<T>> PutAsync<T>(string endpoint, MultipartFormDataContent data)
    {
        var response = await _httpClient.PutAsync(endpoint, data);
        var result = await response.Content.ReadFromJsonAsync<ApiResponseViewModel<T>>();
        return new ApiResponseViewModel<T>
        {
            Data = result!.Data,
            Message = result.Message,
            StatusCode = (int)response.StatusCode
        };
    }

    public async Task<ApiResponseViewModel<R>> PostJsonAsync<T, R>(string endpoint, T data)
    {
        var response = await _httpClient.PostAsJsonAsync(endpoint, data);
        var result = await response.Content.ReadFromJsonAsync<ApiResponseViewModel<R>>();
        return new ApiResponseViewModel<R>
        {
            Data = result!.Data,
            Message = result.Message,
            StatusCode = (int)response.StatusCode
        };
    }
}
