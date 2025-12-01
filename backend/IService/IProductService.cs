using Backend.Entity;
using Backend.Entity.Models;
using Backend.Shared;

namespace Backend.IService.IProductService;

public interface IProductService
{
    public Task<ServiceResult<Product>> CreateNewProduct(ProductDto request);
    public Task<ServiceResult<List<Product>>> GetAllProduct();
    public Task<ServiceResult<Product>> GetProductById(int id);
    public Task<ServiceResult<Product>> UpdateExistingProduct(ProductDto request, int id);
    public Task<ServiceResult<string>> DeleteProductById(int id);
}