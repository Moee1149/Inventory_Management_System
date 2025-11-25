namespace Frontend.ViewModels;

public class ProductDisplayViewModel : ProductViewModel
{
    public string Name { get; set; } = "";
    public string Price { get; set; } = "";
    public ICollection<ProductImageViewModel> ProductImages { get; set; } = new List<ProductImageViewModel>();
}
public class ProductImageViewModel
{
    public int Id { get; set; }
    public string FilePath { get; set; } = "";
}