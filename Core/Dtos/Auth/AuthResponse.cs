using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.Auth
{
    public class AuthResponse : AuthDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public AuthResponse() { }

        public AuthResponse(bool isSuccess, string message, string token, string username)
        {
            IsSuccess = isSuccess;
            Message = message;
            Token = token;
            Username = username;
        }
    }
}
