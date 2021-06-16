using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.Database
{
    public class AssetConversion
    {
        public Guid Id { get; set; }
        
        /// <summary>Amount of the left side to convert (i.e, in a BTCUSDT pair, 100 would mean 100 Bitcoin to convert to USDT)</summary>
        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }
        
        /// <summary>From/Left side of the conversion</summary>
        public Wallet Left { get; set; }
        
        public Guid LeftId { get; set; }
        
        /// <summary>To/Right side of the conversion</summary>
        public Wallet Right { get; set; }
        
        public Guid RightId { get; set; }
        
        public AppUser User { get; set; }
        
        public Guid UserId { get; set; }
    }
}
