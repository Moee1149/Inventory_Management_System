using System.Threading.Tasks;
using Frontend.IService;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

public class ProductController(IApiClient _apiClient) : Controller
{
    // GET: ProductController
    public async Task<ActionResult> Index()
    {
        var result = await _apiClient.GetAllProduct();
        return View(result.Data);
    }

}
