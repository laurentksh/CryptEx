using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.ViewModel
{
    public class AuthViewModel
    {
        public string JWToken { get; set; }

        public AuthViewModel(string jwToken)
        {
            JWToken = jwToken;
        }
    }
}
