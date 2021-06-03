using CryptExApi.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.DTO
{
    public class IbanDto
    {
        [Required, IbanValidator]
        public string Iban { get; set; }
    }
}
