using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.Database
{
    public class Wallet
    {
        public Guid Id { get; set; }

        public string Ticker { get; set; }

        public WalletType Type { get; set; }
    }

    public enum WalletType
    {
        Crypto,
        Fiat
    }
}
