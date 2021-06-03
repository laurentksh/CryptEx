using CryptExApi.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.ViewModel
{
    public class IbanViewModel
    {
        public string Iban { get; set; }

        public BankAccountStatus Status { get; set; }

        public static IbanViewModel FromBankAccount(BankAccount bankAccount) => new()
        {
            Iban = bankAccount.Iban,
            Status = bankAccount.Status
        };
    }
}
