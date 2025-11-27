namespace Frontend.ViewModels;

public class LoginViewModel
{
    public string Token { get; set; } = "";
    public UserType User { get; set; } = new UserType();
}

public class UserType
{
    public string Role { get; set; } = "";
}

