using Domain.Dtos.Auth;
using Domain.IServices.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.service.Auth
{
    public class AuthService : IAuthService
    {
        public Task<AuthDto> GetTokenAsync(TokenRequestDto token)
        {
            throw new NotImplementedException();
        }

        public Task<AuthDto> RegisterAsync(RegisterDto register)
        {
            throw new NotImplementedException();
        }
    }
}
