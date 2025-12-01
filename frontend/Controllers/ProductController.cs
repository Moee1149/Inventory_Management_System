using System.Net.Http.Headers;
using System.Text.Json;
using Frontend.IService;
using Frontend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

public class ProductController(IProductService productService, IConfiguration _config) : Controller
{
    // GET: ProductController
    // [HttpGet("product")]
    [Authorize]
    public async Task<ActionResult> Index()
    {
        if (HttpContext.User.IsInRole("Admin"))
        {
            return RedirectToAction("Index", "Admin");
        }
        try
        {
            var response = await productService.GetAllProduct();
            return View(response.Data);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error getting product: {e}");
            return StatusCode(500, new { message = "Unknow error occured" });
        }
    }

    [HttpGet("product/{id}")]
    public async Task<ActionResult> GetProductBYId(int id)
    {
        var result = await productService.GetProductById(id);
        var images = new List<string>();
        string baseUrl = _config["AppSettings:BaseUrl"]!;
        foreach (var image in result.Data!.ProductImages)
        {
            images.Add($"{baseUrl}/{image.FilePath}");
        }
        ViewBag.Images = images;
        return View("Product", result.Data);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("product/create")]
    public async Task<ActionResult> ViewAddNewProductForm()
    {
        return View("ProductCreate");
    }

    [HttpPost("product/create")]
    public async Task<IActionResult> AddNewProduct([FromForm] ProductAddViewModel newProduct)
    {
        if (!ModelState.IsValid)
        {
            return View("ProductCreate", newProduct);
        }
        var result = await productService.AddNewProduct(newProduct);
        return RedirectToAction("Index", "Product");
    }

    [HttpGet("product/edit/{id}")]
    public async Task<IActionResult> GetUpdateProductView(int id)
    {
        var result = await productService.GetProductById(id);
        var images = new List<object>();
        string baseUrl = _config["AppSettings:BaseUrl"]!;
        foreach (var image in result.Data!.ProductImages)
        {
            images.Add(new
            {
                imageUrl = $"{baseUrl}/{image.FilePath}",
                id = image.Id
            });
        }
        ViewBag.Images = images;
        return View("ProductUpdate", result.Data);
    }

    [HttpPost("product/edit")]
    public async Task<IActionResult> UpdateProduct([FromForm] ProductEditViewModel product)
    {
        try
        {
            var response = await productService.UpdateProduct(product);
            if (response.StatusCode < 200 || response.StatusCode > 299)
            {
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index", "Admin");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error updating = {e.Message}");
            return RedirectToAction("Error", "Home");
        }
    }
}

