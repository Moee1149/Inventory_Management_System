using Backend.Data;
using Backend.Entity;
using Backend.Entity.Models;
using Backend.Entity.Models.ProductDto;
using Backend.IService.IFileService;
using Backend.IService.IProductService;
using Backend.Shared;

namespace Backend.Service.ProductService;

public class ProductService(AppDbContext _context, IFileService fileService) : IProductService
{
    public async Task<ServiceResult<string>> CreateNewProduct(ProductDto request)
    {
        Product product = new Product
        {
            Name = request.ProductName,
            Price = request.ProductPrice,
            Stock = request.Stock,
            Category = request.Category,
            Description = request.Description ?? ""
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        ProductImageDto productImageDto = new ProductImageDto
        {
            files = request.ProductImages ?? new List<IFormFile>(),
            ProductId = product.Id
        };
        await addImagesToProduct(productImageDto);
        return ServiceResult<string>.Ok("Product created Successfully", 200);
    }

    private async Task addImagesToProduct(ProductImageDto productImageDto)
    {
        foreach (IFormFile file in productImageDto.files)
        {
            var filePath = await fileService.SaveFileToDisk(file);
            var productToImage = new ProductToImage
            {
                FilePath = filePath,
                ProductId = productImageDto.ProductId
            };
            await _context.ProductToImage.AddAsync(productToImage);
        }
        await _context.SaveChangesAsync();
    }
}