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

        public CountryViewModel Country { get; set; }

        public static AddressViewModel FromAddress(UserAddress address) => new()
        {
            Street = address.Street,
            City = address.City,
            PostalCode = address.PostalCode,
            Country = CountryViewModel.FromCountry(address.Country)
        };
    }
}
