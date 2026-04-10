namespace Shared.Common;

public class Result
{
    public bool IsSuccess { get; init; }
    public string? Error { get; init; }
    public string? Message { get; init; }

    public static Result Success(string? message = null) => new()
    {
        IsSuccess = true,
        Message = message
    };

    public static Result Failure(string error, string? message = null) => new()
    {
        IsSuccess = false,
        Error = error,
        Message = message
    };
}

public class Result<T>
{
    public bool IsSuccess { get; init; }
    public T? Data { get; init; }
    public string? Error { get; init; }
    public string? Message { get; init; }

    public static Result<T> Success(T data, string? message = null) => new()
    {
        IsSuccess = true,
        Data = data,
        Message = message
    };

    public static Result<T> Failure(string error, string? message = null) => new()
    {
        IsSuccess = false,
        Error = error,
        Message = message
    };
}
