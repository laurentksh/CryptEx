using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models.Database;

namespace CryptExApi.Models.ViewModel.Payment
{
    public class DepositViewModel
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }

        public WalletType WalletType { get; set; }

        public DateTime Date { get; set; }

        public Guid WalletId { get; set; }

        public WalletViewModel Wallet { get; set; }
    }

    public class FiatDepositViewModel : DepositViewModel
    {
        public string SessionId { get; set; }

        public static FiatDepositViewModel FromFiatDeposit(FiatDeposit fiatDeposit) => new FiatDepositViewModel
        {
            Id = fiatDeposit.Id,
            Amount = fiatDeposit.Amount,
            Date = fiatDeposit.CreationDate,
            Status = fiatDeposit.Status,
            SessionId = fiatDeposit.StripeSessionId,
            WalletType = WalletType.Fiat,
            WalletId = fiatDeposit.WalletId,
            Wallet = WalletViewModel.FromWallet(fiatDeposit.Wallet)
        };
    }

    public class CryptoDepositViewModel : DepositViewModel
    {
        public string WalletAddress { get; set; }

        public static CryptoDepositViewModel FromCryptoDeposit(CryptoDeposit cryptoDeposit) => new CryptoDepositViewModel
        {
            Id = cryptoDeposit.Id,
            Amount = cryptoDeposit.Amount,
            Date = cryptoDeposit.CreationDate,
            Status = cryptoDeposit.Status,
            //WalletAddress = null, //Wallet address must be set manually by caller.
            WalletType = WalletType.Crypto,
            WalletId = cryptoDeposit.WalletId,
            Wallet = WalletViewModel.FromWallet(cryptoDeposit.Wallet)
        };
    }
}
