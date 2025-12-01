using Frontend.ViewModels;

namespace Frontend.IService;

public interface IApiClient
{
    public Task<ApiResponseViewModel<R>> PostJsonAsync<T, R>(string endpoint, T data);
    public Task<ApiResponseViewModel<T>> PostAsync<T>(string endpoint, MultipartFormDataContent data);
    public Task<ApiResponseViewModel<T>> GetJsonAsync<T>(string endpoint);
    // public Task<HttpResponseMessage> GetByIdJsonAsync<T>(string endpoint, T id);
    public Task<ApiResponseViewModel<T>> PutAsync<T>(string endpoint, MultipartFormDataContent data);
}