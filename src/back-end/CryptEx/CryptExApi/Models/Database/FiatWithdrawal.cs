using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.Database
{
    public class FiatWithdrawal
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.NotProcessed;

        public DateTime CreationDate { get; set; }

        public Guid BankAccountId { get; set; }

        public BankAccount BankAccount { get; set; }

        public Guid WalletId { get; set; }

        public Wallet Wallet { get; set; }

        public Guid UserId { get; set; }

        public AppUser User { get; set; }
    }
}
