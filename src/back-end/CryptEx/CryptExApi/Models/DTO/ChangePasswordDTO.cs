using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.DTO
{
    public class ChangePasswordDTO
    {
        public string Token { get; set; }

        public string NewPassword { get; set; }
    }
}
