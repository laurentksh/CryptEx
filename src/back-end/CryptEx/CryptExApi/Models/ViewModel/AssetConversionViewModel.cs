using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.ViewModel
{
    public class AssetConversionViewModel
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public WalletPairViewModel Pair { get; set; }
    }
}
