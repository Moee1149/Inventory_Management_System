using Backend.Data;
using Backend.Entity;
using Backend.Entity.Models;
using Backend.IService.IFileService;
using Backend.IService.IProductService;
using Backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace Backend.Service.ProductService;

public class ProductService(AppDbContext _context, IFileService fileService) : IProductService
{
    public async Task<ServiceResult<List<Product>>> GetAllProduct()
    {
        var result = await _context.Products.Include(p => p.ProductImages).ToListAsync();
        return ServiceResult<List<Product>>.Ok(result, 200, "Get all Products Succesfull");
    }

    public async Task<ServiceResult<Product>> GetProductById(int id)
    {
        var result = await _context.Products.Where(p => p.Id == id).Include(p => p.ProductImages).FirstOrDefaultAsync();
        if (result == null)
        {
            return ServiceResult<Product>.Fail("Product not found", 404);
        }
        return ServiceResult<Product>.Ok(result, 200, "Get Product Succesfully");
    }
    public async Task<ServiceResult<Product>> CreateNewProduct(ProductDto request)
    {
        Product product = new Product();
        await createOrUpdateProduct(product, request);
        return ServiceResult<Product>.Ok(product, 200, "Product created Successfully");
    }
    public async Task<ServiceResult<Product>> UpdateExistingProduct(ProductDto request, int id)
    {
        var existingProduct = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);

        if (existingProduct == null)
        {
            return ServiceResult<Product>.Fail("Product not found", 404);
        }
        await createOrUpdateProduct(existingProduct, request);
        return ServiceResult<Product>.Ok(existingProduct, 200, "Product created Successfully");
    }

    private async Task createOrUpdateProduct(Product product, ProductDto request)
    {
        product.Name = request.ProductName;
        product.Price = request.ProductPrice;
        product.Stock = request.Stock;
        product.Category = request.Category;
        product.Description = request.Description ?? "";
        if (product.Id != 0)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        else
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        if (request.ProductImages != null && request.ProductImages.Any())
        {
            ProductImageDto productImageDto = new ProductImageDto
            {
                files = request.ProductImages,
                ProductId = product.Id
            };
            await addImagesToProduct(productImageDto);
        }
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

    public async Task<ServiceResult<string>> DeleteProductById(int id)
    {
        var employee = await _context.Products.FindAsync(id);
        if (employee == null)
        {
            return ServiceResult<string>.Fail("Product Not Found", 400);
        }
        _context.Products.Remove(employee);
        await _context.SaveChangesAsync();
        return ServiceResult<string>.Ok("Product deleted successfully", 200);
    }
}