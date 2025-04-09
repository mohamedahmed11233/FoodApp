using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.Auth
{
    public class TokenRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
