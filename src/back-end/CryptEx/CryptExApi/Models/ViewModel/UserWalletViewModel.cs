using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models.Database;

namespace CryptExApi.Models.ViewModel
{
    public class UserWalletViewModel : WalletViewModel
    {
        public decimal Amount { get; set; }
    }
}
