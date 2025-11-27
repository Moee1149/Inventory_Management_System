using Backend.Entity;
using Backend.Entity.Models;
using Backend.Shared;

namespace Backend.IService;

public interface IAuthService
{
    public Task<ServiceResult<User>> RegisterNewUser(UserDto request);
    public Task<ServiceResult<LoginReturnType>> LoginUser(UserDto request);

}

public class LoginReturnType
{
    public string Token { get; set; } = "";
    public User? User { get; set; } = null;
}