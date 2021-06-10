using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models.Database;

namespace CryptExApi.Models.ViewModel
{
    public class BankAccountViewModel
    {
        public Guid Id { get; set; }

        public string Iban { get; set; }

        public BankAccountStatus Status { get; set; }
    }
}
