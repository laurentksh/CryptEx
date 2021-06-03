using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models.Database;

namespace CryptExApi.Models.ViewModel
{
    public class WalletViewModel
    {
        public Guid Id { get; set; }

        public string Ticker { get; set; }

        public string FullName { get; set; }

        public WalletType Type { get; set; }

        /// <summary>Wallet paired with the selected currency (e.g: BTCUSD)</summary>
        public WalletPairViewModel SelectedCurrencyPair { get; set; }

        public static WalletViewModel FromWallet(Wallet wallet, WalletPairViewModel pair = null) => new()
        {
            Id = wallet.Id,
            Ticker = wallet.Ticker,
            FullName = wallet.FullName,
            Type = wallet.Type,
            SelectedCurrencyPair = pair
        };
    }
}
