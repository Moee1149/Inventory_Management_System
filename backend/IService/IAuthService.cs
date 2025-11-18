
using Backend.Entity;
using Backend.Entity.Models;
using Backend.Shared;

namespace Backend.IService;

public interface IAuthService
{
    public Task<ServiceResult<User>> RegisterNewUser(UserDto request);
    public Task<ServiceResult<string>> LoginUser(UserDto request);

}