using Backend.Entity.Models.ProductDto;
using Backend.Shared;

namespace Backend.IService.IProductService;

public interface IProductService
{
    public Task<ServiceResult<string>> CreateNewProduct(ProductDto request);
}