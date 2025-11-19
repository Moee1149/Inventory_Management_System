using Backend.Entity.Models.ProductDto;
using Backend.IService.IProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService _productService) : ControllerBase
    {
        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> CreateNewProduct([FromForm] ProductDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.CreateNewProduct(request);
            return Ok(new { message = result.Message, data = result.Data });
        }
    }
}
