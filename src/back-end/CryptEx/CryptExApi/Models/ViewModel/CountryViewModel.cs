using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models.Database;

namespace CryptExApi.Models.ViewModel
{
    public class CountryViewModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// ISO 3166-1 Alpha 2 Country identifier, e.g: CH, SE, NE
        /// </summary>
        public string Iso31661Alpha2Code { get; set; }

        public static CountryViewModel FromCountry(Country country) => new()
        {
            Id = country.Id,
            Iso31661Alpha2Code = country.Iso31661Alpha2Code
        };
    }
}
