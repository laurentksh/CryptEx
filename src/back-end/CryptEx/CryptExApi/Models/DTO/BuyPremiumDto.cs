using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.DTO
{
    public class BuyPremiumDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [CreditCard]
        public string CreditCardNumber { get; set; }

        [Required]
        public string Expiracy { get; set; }

        [Required]
        public string Cvc { get; set; }
    }
}
