using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models.Database;

namespace CryptExApi.Models.ViewModel
{
    public class AddressViewModel
    {
        public string Street { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public Guid CountryId { get; set; }

        public string ISO31661Alpha2Code { get; set; }

        public static AddressViewModel FromAddress(UserAddress address) => new()
        {
            Street = address.Street,
            City = address.City,
            PostalCode = address.PostalCode,
            CountryId = address.CountryId,
            ISO31661Alpha2Code = address.Country.ISO31661Alpha2Code
        };
    }
}
