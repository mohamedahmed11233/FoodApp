using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Auth
{
    public class GenerateSecretKeyDTO
    {
        public string SecretKey { get; set; }
        public string AuthenticatorUri { get; set; }
    }
}
