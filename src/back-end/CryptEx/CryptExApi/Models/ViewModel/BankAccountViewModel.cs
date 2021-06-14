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

        public DateTime CreationDate { get; set; }

        /// <summary>When the bank account was approved/refused.</summary>
        public DateTime DecisionDate { get; set; }

        public BankAccountStatus Status { get; set; }

        public static BankAccountViewModel FromBankAccount(BankAccount bankAccount) => new()
        {
            Id = bankAccount.Id,
            Iban = bankAccount.Iban,
            Status = bankAccount.Status,
            CreationDate = bankAccount.CreationDate,
            DecisionDate = bankAccount.DecisionDate
        };
    }
}
