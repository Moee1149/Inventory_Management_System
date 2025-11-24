using Frontend.ViewModels;

namespace Frontend.IService;


public interface IApiClient
{
    public Task<HttpResponseMessage> HandleUserLogin(UserViewModel user);
    public Task<HttpResponseMessage> HandleUserRegister(UserCreateViewModel newUser);
}