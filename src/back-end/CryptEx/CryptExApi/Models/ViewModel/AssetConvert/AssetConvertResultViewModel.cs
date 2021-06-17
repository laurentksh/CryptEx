using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.ViewModel.AssetConvert
{
    public class AssetConvertViewModel
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }

        public WalletPairViewModel Pair { get; set; }
    }
}
