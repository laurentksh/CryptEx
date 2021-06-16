using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.DTO
{
    public class AssetConvertDto
    {
        public Guid LeftAssetId { get; set; }
        
        public Guid RightAssetId { get; set; }

        public decimal Amount { get; set; }
    }
}
