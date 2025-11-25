using Backend.Entity.Models;
using Backend.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService _authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(UserDto request)
    {
        try
        {
            var result = await _authService.RegisterNewUser(request);
            if (result.StatusCode == 400)
            {
                return BadRequest(result.Message);
            }
            if (result.StatusCode == 409)
            {
                return Conflict(result.Message);
            }
            return Ok(new { message = result.Message, data = ""});
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error Creating User: {e}");
            return StatusCode(500, new { error = e, message = "Error creating User" });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(UserDto request)
    {
        try
        {
            var result = await _authService.LoginUser(request);
            if (result.StatusCode == 400)
            {
                return BadRequest(result.Message);
            }
            if (result.StatusCode == 401)
            {
                return Unauthorized(result.Message);
            }
            if (result.StatusCode == 404)
            {
                return NotFound(result.Message);
            }
            return Ok(new { message = result.Message, data = result.Data });
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error Authneticating User: {e}");
            return StatusCode(500, new { error = e, message = "Error creating User" });
        }
    }
}
