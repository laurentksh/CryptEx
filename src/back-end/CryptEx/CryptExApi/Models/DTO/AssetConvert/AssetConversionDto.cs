using System;

namespace CryptExApi.Models.DTO
{
    public class AssetConversionDto
    {
        public Guid TransactionLockId { get; set; }

        public decimal Amount { get; set; }
    }
}
