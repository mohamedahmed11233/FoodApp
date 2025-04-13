using Domain.Enum.SharedEnums;
using System.Text.Json.Serialization;

namespace Presentation.Helpers
{
    public class ResponseViewModel<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public ErrorCode ErrorCode { get; set; }

        public ResponseViewModel(bool success, string? message, T? data, ErrorCode errorCode = ErrorCode.None)
        {
            Success = success;
            Message = message;
            Data = data;
            ErrorCode = errorCode;
        }

        public static ResponseViewModel<T> SuccessResult(T data, string message = "")
        {
            return new ResponseViewModel<T>(true, message, data, ErrorCode.None); // No error code for success
        }

        public static ResponseViewModel<T> ErrorResult(string message, T? data = default, ErrorCode errorCode = ErrorCode.None)
        {
            return new ResponseViewModel<T>(false, message, data, errorCode); // Set the error code as needed
        }
    }
}
