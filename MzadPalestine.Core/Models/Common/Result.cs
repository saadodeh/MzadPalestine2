namespace MzadPalestine.Core.Models.Common;

public class Result<T>
{
    public bool Success { get; set; }
    public bool Succeeded => Success;
    public string? Message { get; set; }
    public T? Data { get; set; }

    public static Result<T> CreateSuccess(T data, string? message = null)
    {
        return new Result<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static Result<T> Failure(string message)
    {
        return new Result<T>
        {
            Success = false,
            Message = message,
            Data = default
        };
    }

    public static Result<T> CreateFailure(string message)
    {
        return Failure(message);
    }
}