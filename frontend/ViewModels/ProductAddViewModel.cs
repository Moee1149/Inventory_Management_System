namespace Frontend.ViewModels;

public class ProductAddViewModel : ProductViewModel
{
    public string ProductName { get; set; } = "";
    public string ProductPrice { get; set; } = "";
    public List<IFormFile>? ProductImages { get; set; }
}
