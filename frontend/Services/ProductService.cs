using System.Net.Http.Headers;
using Frontend.IService;
using Frontend.ViewModels;

namespace Frontend.Services;

public class ProductService(IApiClient _apiClient) : IProductService
{
    public async Task<ApiResponseViewModel<ProductDisplayViewModel>> AddNewProduct(ProductAddViewModel newProduct)
    {
        using var formData = new MultipartFormDataContent();

        formData.Add(new StringContent(newProduct.ProductName ?? ""), "ProductName");
        formData.Add(new StringContent(newProduct.ProductPrice ?? ""), "ProductPrice");
        formData.Add(new StringContent(newProduct.Description ?? ""), "Description");
        formData.Add(new StringContent(newProduct.Category ?? ""), "Category");
        formData.Add(new StringContent(newProduct.Stock.ToString()), "Stock");

        if (newProduct.ProductImages != null)
        {
            foreach (var image in newProduct.ProductImages)
            {
                if (image.Length > 0)
                {
                    var streamContent = new StreamContent(image.OpenReadStream());
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
                    formData.Add(streamContent, "ProductImages", image.FileName);
                }
            }
        }

        var response = await _apiClient.PostAsync<ProductDisplayViewModel>("api/product/register", formData);
        return response;
    }

    public async Task<ApiResponseViewModel<List<ProductDisplayViewModel>>> GetAllProduct(string search = "", int pageNumber = 1)
    {
        var response = await _apiClient.GetJsonAsync<List<ProductDisplayViewModel>>($"api/product?search={search}&page={pageNumber}");
        return response;
    }

    public async Task<ApiResponseViewModel<ProductDisplayViewModel>> GetProductById(int id)
    {
        var response = await _apiClient.GetJsonAsync<ProductDisplayViewModel>($"api/product/productId/{id}");
        return response;
    }
}