using Frontend.ViewModels;

namespace Frontend.IService;

public interface IProductService
{
    public Task<ApiResponseViewModel<ProductDisplayViewModel>> AddNewProduct(ProductAddViewModel newProduct);
    public Task<ApiResponseViewModel<List<ProductDisplayViewModel>>> GetAllProduct(string search = "", int pageNumber = 1);
    public Task<ApiResponseViewModel<ProductDisplayViewModel>> GetProductById(int id);
    public Task<ApiResponseViewModel<ProductDisplayViewModel>> UpdateProduct(ProductEditViewModel product);
}