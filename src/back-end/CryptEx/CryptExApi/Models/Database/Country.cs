using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.Database
{
    public class Country
    {
        public Guid Id { get; set; }

        /// <summary>
        /// ISO 3166-1 Alpha 2 Country identifier, e.g: CH, SE, NE
        /// </summary>
        public string ISO31661Alpha2Code { get; set; }
    }
}
