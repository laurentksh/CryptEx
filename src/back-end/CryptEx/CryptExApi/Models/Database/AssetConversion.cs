using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.Database
{
    /// <summary>
    /// Represents a crypto currency transaction between two assets.
    /// </summary>
    /// <example>
    /// Usage example:
    /// 
    /// User wants to convert 100 CHSB to USDC.
    /// Amount: 100
    /// Left: CHSB
    /// Right: USDC
    /// ExchangeRate: 0.667~
    /// 
    /// If we want to get the amount in USDC, we do Amount * ExchangeRate
    /// </example>
    public class AssetConversion
    {
        public Guid Id { get; set; }
        
        /// <summary>Amount of the left side to convert (i.e, in a BTCUSDT pair, 100 would mean 100 Bitcoin to convert to USDT)</summary>
        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }

        public Guid PriceLockId { get; set; }

        public AssetConversionLock PriceLock { get; set; }
        
        public AppUser User { get; set; }
        
        public Guid UserId { get; set; }
    }
}
