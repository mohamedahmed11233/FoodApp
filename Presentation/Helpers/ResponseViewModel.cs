using Domain.Enum.SharedEnums;
using System.Text.Json.Serialization;

namespace Presentation.Helpers
{
        public class ResponseViewModel<T>
        {
            public bool Success { get; set; }
            public string? Message { get; set; }
        public T Data { get; set; }
        public ErrorCode errorCode { get; set; }



        public ResponseViewModel(bool success, string? message, T data)
            {
                Success = success;
                Message = message;
                Data = data;
            }

            public static ResponseViewModel<T> SuccessResult(T data, string message = "")
            {
                return new ResponseViewModel<T>(true, message, data); // No error code for success
            }

            public static ResponseViewModel<T> ErrorResult(string message , T data)
            {
                return new ResponseViewModel<T>(false , message  , data); // Includes error code
            }
        }

    

}
