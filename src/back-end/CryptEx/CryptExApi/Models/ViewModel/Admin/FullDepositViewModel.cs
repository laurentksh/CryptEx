using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models.Database;

namespace CryptExApi.Models.ViewModel.Admin
{
    public class FullDepositViewModel
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }

        public WalletType WalletType { get; set; }

        public DateTime Date { get; set; }

        public Guid WalletId { get; set; }

        public WalletViewModel Wallet { get; set; }

        public UserViewModel User { get; set; }

        public Guid UserId { get; set; }

        public static FullDepositViewModel FromDeposit(FiatDeposit deposit) => new()
        {
            Id = deposit.Id,
            Amount = deposit.Amount,
            Status = deposit.Status,
            WalletType = WalletType.Fiat,
            Date = deposit.CreationDate,
            User = UserViewModel.FromAppUser(deposit.User),
            UserId = deposit.UserId,
            Wallet = WalletViewModel.FromWallet(deposit.Wallet, null),
            WalletId = deposit.WalletId
        };

        public static FullDepositViewModel FromDeposit(CryptoDeposit deposit) => new()
        {
            Id = deposit.Id,
            Amount = deposit.Amount,
            Status = deposit.Status,
            WalletType = WalletType.Fiat,
            Date = deposit.CreationDate,
            User = UserViewModel.FromAppUser(deposit.User),
            UserId = deposit.UserId,
            Wallet = WalletViewModel.FromWallet(deposit.Wallet, null),
            WalletId = deposit.WalletId
        };
    }
}
