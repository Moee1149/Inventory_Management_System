namespace Backend.Entity.Models;

public class ProductImageDto
{
    public List<IFormFile> files = new();
    public int ProductId { get; set; }
}