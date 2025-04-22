using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Auth
{
    public class ResetPasswordRequestDto
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
        public string OtpCode { get; set; }
    }

}
