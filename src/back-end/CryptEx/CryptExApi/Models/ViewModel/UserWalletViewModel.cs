using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models.Database;

namespace CryptExApi.Models.ViewModel
{
    public class UserWalletViewModel : WalletViewModel
    {
        public decimal Amount { get; set; }

        public static UserWalletViewModel FromWallet(Wallet wallet, WalletPairViewModel pair = null) => new()
        {
            Id = wallet.Id,
            Ticker = wallet.Ticker,
            FullName = wallet.FullName,
            Type = wallet.Type,
            SelectedCurrencyPair = pair,
            Amount = 0m
        };
    }
}
