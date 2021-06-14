using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.DTO
{
    public class RequestPasswordChangeDTO
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
