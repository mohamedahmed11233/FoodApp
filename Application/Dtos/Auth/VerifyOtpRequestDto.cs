using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Auth
{
    public class VerifyOtpRequestDto
    {
        public int UserId { get; set; }
        public string Code { get; set; }
    }

}
