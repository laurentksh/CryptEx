using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.ViewModel
{
    public class WalletPairViewModel
    {
        /// <summary>
        /// Left side of the pair (e.g: BTC)
        /// </summary>
        public WalletViewModel Left { get; set; }

        /// <summary>
        /// Right side of the pair (e.g: USD)
        /// </summary>
        public WalletViewModel Right { get; set; }

        /// <summary>
        /// Rate for which the left is traded for the right (1:{rate})
        /// Example for BTCUSD (as of 17.05.2021): 45403.06 
        /// </summary>
        public decimal Rate { get; set; }

        public WalletPairViewModel(WalletViewModel left, WalletViewModel right, decimal rate)
        {
            Left = left;
            Right = right;
            Rate = rate;
        }

        public override string ToString() => Left.Ticker.ToUpper() + Right.Ticker.ToUpper();

        public string ToCoinbaseString() => Left.Ticker.ToUpper() + "-" + Right.Ticker.ToUpper();

        public static WalletPairViewModel FromWalletPair(WalletPair pair) => new(WalletViewModel.FromWallet(pair.Left), WalletViewModel.FromWallet(pair.Right), pair.Rate);
    }
}
