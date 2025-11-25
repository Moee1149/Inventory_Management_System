using System.ComponentModel.DataAnnotations;

namespace Backend.Entity.Models;

public class ProductDto
{
    [Required(ErrorMessage = "Product name is required")]
    public string ProductName { get; set; } = "";
    [Required(ErrorMessage = "Product price is required")]
    public string ProductPrice { get; set; } = "";
    public string? Description { get; set; } = "";
    [Required]
    public string Category { get; set; } = "";
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
    public List<IFormFile>? ProductImages { get; set; }
}
