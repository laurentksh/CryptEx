using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.ViewModel.Admin
{
    public class StatsViewModel
    {
        public long TotalUsers { get; set; }

        public long NewUsers24h { get; set; }

        public long NewUsers7d { get; set; }

        public long NewUsers30d { get; set; }

        public decimal TotalTradedAmount { get; set; }

        public decimal TradedAmount24h { get; set; }

        public decimal TradedAmount7d { get; set; }

        public decimal TradedAmount30d { get; set; }
    }
}
