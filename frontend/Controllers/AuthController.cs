using System.Text.Json;
using Frontend.IService;
using Frontend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

public class AuthController(IApiClient _apiClient) : Controller
{
    // GET: AuthController
    [HttpGet("auth/login")]
    public ActionResult ViewLoginUser()
    {
        return View("Login");
    }

    [HttpPost("auth/login")]
    public async Task<IActionResult> LoginUser(UserViewModel user)
    {
        if (!ModelState.IsValid)
        {
            return View("Login", user);
        }
        var response = await _apiClient.HandleUserLogin(user);
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = JsonSerializer.Deserialize<ApiResponseViewModel<object>>(content);
            ViewBag.ErrorMessage = errorResponse?.Message;
            return View();
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
        if (!ModelState.IsValid)
        {
            return View("Register", newUser);
        }
        var response = await _apiClient.HandleUserRegister(newUser);
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = JsonSerializer.Deserialize<ApiResponseViewModel<object>>(content);
            ViewBag.ErrorMessage = errorResponse?.Message;
            return View();
        }
        return RedirectToAction("ViewLoginUser", "Home");
    }
}
