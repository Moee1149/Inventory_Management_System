using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entity;

public class ProductToImage
{
    public int Id { get; set; }
    public string FilePath { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    [ForeignKey(nameof(ProductId))]
    public int ProductId { get; set; }
}