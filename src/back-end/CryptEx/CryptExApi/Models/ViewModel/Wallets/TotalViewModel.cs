using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.ViewModel.Wallets
{
    public class TotalViewModel
    {
        public decimal Amount { get; set; }

        public WalletViewModel Reference { get; set; }
    }
}
