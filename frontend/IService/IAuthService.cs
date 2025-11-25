using Frontend.ViewModels;

namespace Frontend.IService;

public interface IAuthService
{
    public Task<ApiResponseViewModel<string>> HandleUserRegister(UserCreateViewModel newUser);
    public Task<ApiResponseViewModel<string>> HandleUserLogin(UserViewModel user);
    public void SetTokenCookie(string token);
    public bool IsLoggenIn();
    public void Logout();
}