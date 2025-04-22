using Domain.Enum.SharedEnums;
using System.Text.Json.Serialization;

namespace Presentation.Helpers
{
    public class ResponseViewModel<T>(bool success, ErrorCode errorCode = ErrorCode.None, string? message = default, T? data = default)
    {
        public bool Success { get; set; } = success;
        public string? Message { get; set; } = message;
        public T? Data { get; set; } = data;
        public ErrorCode ErrorCode { get; set; } = errorCode;

        public static ResponseViewModel<T> SuccessResult(T data, string? message = "")
        {
            return new ResponseViewModel<T>(true, ErrorCode.None, message, data); // No error code for success
        }

        public static ResponseViewModel<T> ErrorResult(string? message, ErrorCode errorCode = ErrorCode.None)
        {
            return new ResponseViewModel<T>(false, errorCode, message); // Set the error code as needed
        }
    }
}
