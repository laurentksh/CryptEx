using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.Database
{
    public class AssetConversion
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        /// <summary>
        /// From/Left side of the conversion
        /// </summary>
        public Wallet Left { get; set; }

        /// <summary>
        /// To/Right side of the conversion
        /// </summary>
        public Wallet Right { get; set; }
    }
}
