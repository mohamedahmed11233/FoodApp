using Domain.Enum.SharedEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Auth
{
    public class RegisterResponseDto
    {
        public bool IsSucess { get; set; }

        public string Username { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public IEnumerable<Roles> Roles { get; set; } = default!;
        public string Token { get; set; } = string.Empty;
    }
}
