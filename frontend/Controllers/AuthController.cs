using System.Text.Json;
using Frontend.IService;
using Frontend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

public class AuthController(IAuthService _authService) : Controller
{
    // GET: AuthController
    [HttpGet("auth/login")]
    public ActionResult ViewLoginUser()
    {
        if (_authService.IsLoggenIn())
        {
            return RedirectToAction("Index", "Home");
        }
        return View("Login");
    }

    [HttpPost("auth/login")]
    public async Task<IActionResult> LoginUser(UserViewModel user)
    {
        if (!ModelState.IsValid)
        {
            return View("Login", user);
        }
        var response = await _authService.HandleUserLogin(user);
        if (response.StatusCode < 200 || response.StatusCode > 299)
        {
            ViewBag.ErrorMessgae = response.Message;
            return View("Register", user);
        }
        if (!string.IsNullOrEmpty(response?.Data))
        {
            _authService.SetTokenCookie(response.Data);
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Route("auth/register")]
    public ActionResult ViewRegisterUser()
    {
        return View("Register");
    }

    [HttpPost("auth/register")]
    public async Task<ActionResult> RegisterUser(UserCreateViewModel newUser)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("Register", newUser);
            }
            var response = await _authService.HandleUserRegister(newUser);
            Console.WriteLine("Status code = " + response.StatusCode);
            if (response.StatusCode < 200 || response.StatusCode > 299)
            {
                ViewBag.ErrorMessgae = response.Message;
                return View("Register", newUser);
            }
            return RedirectToAction("ViewLoginUser", "Auth");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error creating user : {e}");
            return View("Register");
        }
    }
}
