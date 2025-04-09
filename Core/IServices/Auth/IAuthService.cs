using Domain.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IServices.Auth
{
    public interface IAuthService
    {
        Task<AuthDto> RegisterAsync(RegisterDto register);
        Task<AuthDto> GetTokenAsync(TokenRequestDto token);
        //Task<string> AddRoleAsync(AddRoleDto role);
    }
}
