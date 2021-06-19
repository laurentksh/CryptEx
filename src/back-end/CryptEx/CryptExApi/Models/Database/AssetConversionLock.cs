using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.Database
{
    public class AssetConversionLock
    {
        public Guid Id { get; set; }

        public DateTime ExpirationUtc { get; set; }

        /// <summary>From/Left side of the conversion</summary>
        public Wallet Left { get; set; }

        public Guid LeftId { get; set; }

        /// <summary>To/Right side of the conversion</summary>
        public Wallet Right { get; set; }

        public Guid RightId { get; set; }

        public AppUser User { get; set; }

        public Guid UserId { get; set; }

        /// <summary>
        /// Exchange rate at the time of the transaction
        /// </summary>
        public decimal ExchangeRate { get; set; }
        
        [InverseProperty(nameof(AssetConversion.PriceLock))]
        public AssetConversion Conversion { get; set; }
    }
}
