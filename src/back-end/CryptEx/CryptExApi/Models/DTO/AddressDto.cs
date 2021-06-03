using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.DTO
{
    public class AddressDto
    {
        public string Street { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public Guid CountryId { get; set; }
    }
}
