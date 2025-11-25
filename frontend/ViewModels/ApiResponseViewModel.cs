namespace Frontend.ViewModels;

public class ApiResponseViewModel<T>
{
    public T? Data { get; set; }
    public string Message { get; set; } = "";
    public int StatusCode { get; set; }
}