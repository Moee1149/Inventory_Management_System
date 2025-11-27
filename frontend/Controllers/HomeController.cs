using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using frontend.Models;
using Frontend.IService;

namespace frontend.Controllers;

public class HomeController : Controller
{
    private readonly IAuthService _authService;

    public HomeController(IAuthService authService)
    {
        _authService = authService;
    }

    public IActionResult LogoutUser()
    {
        _authService.Logout();
        return RedirectToAction("Index", "Product");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
