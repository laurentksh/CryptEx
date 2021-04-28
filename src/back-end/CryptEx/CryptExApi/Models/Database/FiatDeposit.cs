using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.Database
{
    public class FiatDeposit
    {
        public Guid Id { get; set; }

        public bool Fullfilled { get; set; }

        public string StripeSessionId { get; set; }
    }
}
