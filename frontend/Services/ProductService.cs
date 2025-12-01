using System.Net.Http.Headers;
using Frontend.IService;
using Frontend.ViewModels;

namespace Frontend.Services;

public class ProductService(IApiClient _apiClient) : IProductService
{
    public async Task<ApiResponseViewModel<ProductDisplayViewModel>> AddNewProduct(ProductAddViewModel newProduct)
    {
        var formData = ConvertViewModelToFormData(newProduct);
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

    public async Task<ApiResponseViewModel<ProductDisplayViewModel>> UpdateProduct(ProductEditViewModel product)
    {
        var formData = ConvertViewModelToFormData(product);
        if (!string.IsNullOrEmpty(product.DeletedImageIds))
        {
            var ids = product.DeletedImageIds.Split(',')
                .Where(s => int.TryParse(s, out _))
                .Select(int.Parse)
                .ToList();

            foreach (var id in ids)
            {
                formData.Add(new StringContent(id.ToString()), "DeletedIds");
            }
        }
        product.DeletedImageIds = "";
        var response = await _apiClient.PutAsync<ProductDisplayViewModel>($"api/product/update/{product.Id}", formData);
        return response;
    }

    private MultipartFormDataContent ConvertViewModelToFormData(ProductAddViewModel product)
    {

        var formData = new MultipartFormDataContent();

        formData.Add(new StringContent(product.ProductName ?? ""), "ProductName");
        formData.Add(new StringContent(product.ProductPrice ?? ""), "ProductPrice");
        formData.Add(new StringContent(product.Description ?? ""), "Description");
        formData.Add(new StringContent(product.Category ?? ""), "Category");
        formData.Add(new StringContent(product.Stock.ToString()), "Stock");

        if (product.ProductImages != null)
        {
            foreach (var image in product.ProductImages)
            {
                if (image.Length > 0)
                {
                    var streamContent = new StreamContent(image.OpenReadStream());
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
                    formData.Add(streamContent, "ProductImages", image.FileName);
                }
            }
        }
        return formData;
    }

}