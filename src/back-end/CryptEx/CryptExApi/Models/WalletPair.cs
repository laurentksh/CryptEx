using CryptExApi.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models
{
    public class WalletPair
    {
        /// <summary>
        /// Left side of the pair (e.g: BTC)
        /// </summary>
        public Wallet Left { get; set; }

        /// <summary>
        /// Right side of the pair (e.g: USD)
        /// </summary>
        public Wallet Right { get; set; }

        /// <summary>
        /// Rate for which the left is traded for the right (1:{rate})
        /// Example for BTCUSD (as of 17.05.2021): 45403.06 
        /// </summary>
        public decimal Rate { get; set; }

        public override string ToString() => Left.Ticker.ToUpper() + Right.Ticker.ToUpper();
    }
}
