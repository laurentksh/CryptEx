using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models.Database;

namespace CryptExApi.Models.ViewModel
{
    public class AssetConversionViewModel
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }

        public WalletPairViewModel Pair { get; set; }

        public static AssetConversionViewModel FromAssetConversion(AssetConversion t) => new()
        {
            Id = t.Id,
            Amount = t.Amount,
            Status = t.Status,
            Pair = WalletPairViewModel.FromWalletPair(new WalletPair(t.Left, t.Right, t.ExchangeRate))
        };
    }
}
