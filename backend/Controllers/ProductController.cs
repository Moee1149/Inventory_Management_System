using Backend.Entity.Models;
using Backend.IService.IProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService _productService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> CreateNewProduct([FromForm] ProductDto request)
        {
            Console.WriteLine("We are in controller");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Console.WriteLine("ok adding product here before");
                var result = await _productService.CreateNewProduct(request);
                Console.WriteLine("ok adding product here");
                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error creating product: {e}");
                return StatusCode(500, new { message = "Unknow error occured" });
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductDto request, int id)
        {
            try
            {
                var result = await _productService.UpdateExistingProduct(request, id);
                return Ok(new { data = result.Data, message = result.Message });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error geting product: {e}");
                return StatusCode(500, new { message = "Unknow error occured" });
            }
        }

        [HttpGet("productId/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var result = await _productService.GetProductById(id);
                if (result.StatusCode == 404)
                {
                    return NotFound(new { message = result.Message });
                }
                return Ok(new { message = result.Message, Data = result.Data });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error geting product: {e}");
                return StatusCode(500, new { message = "Unknow error occured" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            try
            {
                var result = await _productService.GetAllProduct();
                return Ok(new { data = result.Data, message = result.Message });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error get all product: {e}");
                return StatusCode(500, new { message = "Unknow error occured" });
            }
        }
    }
}
