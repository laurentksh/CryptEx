using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.ViewModel.Wallets
{
    public class TotalsViewModel
    {
        public TotalViewModel AccountTotal { get; set; }

        public TotalViewModel FiatTotal { get; set; }

        public TotalViewModel CryptoTotal { get; set; }


        public TotalViewModel AccountTotalYtd { get; set; }

        public TotalViewModel FiatTotalYtd { get; set; }

        public TotalViewModel CryptoTotalYtd { get; set; }


        public TotalViewModel PerformanceYtd { get; set; }
    }
}
