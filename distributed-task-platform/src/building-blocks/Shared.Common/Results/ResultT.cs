using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Common
{
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
}
