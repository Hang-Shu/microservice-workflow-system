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
