namespace Backend.Entity;

public class Product : BaseEntity
{
    public string Description { get; set; } = "";
    public string Category { get; set; } = "";
    public string Price { get; set; } = "";
    public int Stock { get; set; }
    public ICollection<ProductToImage> ProductImages { get; set; } = new List<ProductToImage>();
}
