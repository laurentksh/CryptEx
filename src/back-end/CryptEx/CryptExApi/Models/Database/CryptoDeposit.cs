using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.Database
{
    public class CryptoDeposit
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.NotProcessed;

        public string TransactionId { get; set; }

        public Guid WalletId { get; set; }

        public Wallet Wallet { get; set; }

        public Guid UserId { get; set; }

        public AppUser User { get; set; }
    }
}
