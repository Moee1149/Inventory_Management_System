using Microsoft.AspNetCore.Mvc;
using Frontend.IService;
using Microsoft.AspNetCore.Authorization;

namespace frontend.Controllers;

public class AdminController(IProductService _productService) : Controller
{
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var response = await _productService.GetAllProduct();
        return View(response.Data);
    }
}
