using CryptExApi.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.ViewModel.AssetConvert
{
    public class AssetConversionLockViewModel
    {
        public Guid Id { get; set; }

        public DateTime ExpirationUtc { get; set; }

        public WalletPairViewModel Pair { get; set; }

        public static AssetConversionLockViewModel FromConversionLock(AssetConversionLock assetConversionLock) => new()
        {
            Id = assetConversionLock.Id,
            ExpirationUtc = assetConversionLock.ExpirationUtc,
            Pair = new WalletPairViewModel(
                WalletViewModel.FromWallet(assetConversionLock.Left),
                WalletViewModel.FromWallet(assetConversionLock.Right),
                assetConversionLock.ExchangeRate
            )
        };
    }
}
