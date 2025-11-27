
namespace Backend.Shared;

public class ServiceResult<T>
{
    public T? Data { get; init; }
    public string? Message { get; init; }
    public int StatusCode { get; set; }

    public static ServiceResult<T> Ok(T data, int statusCode, string? message = null) =>
        new() { Data = data, Message = message, StatusCode = statusCode };

    public static ServiceResult<T> Fail(string message, int statusCode, T? data = default) =>
        new() { Message = message, StatusCode = statusCode, Data = data };
}