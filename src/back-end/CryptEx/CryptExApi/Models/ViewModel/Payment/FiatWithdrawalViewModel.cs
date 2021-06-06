using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.ViewModel.Payment
{
    public class FiatWithdrawalViewModel
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.NotProcessed;

        public DateTime Date { get; set; }

        public BankAccountViewModel BankAccount { get; set; }

        public WalletViewModel Wallet { get; set; }
    }
}
