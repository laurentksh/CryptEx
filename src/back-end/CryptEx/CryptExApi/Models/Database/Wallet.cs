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

        public string FullName { get; set; }

        public WalletType Type { get; set; }
    }

    public enum WalletType
    {
        Fiat = 1,
        Crypto = 2,
        Both = Crypto | Fiat
    }
}
