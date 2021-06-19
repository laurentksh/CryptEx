using System;

namespace CryptExApi.Models.DTO.AssetConvert
{
    public class AssetConvertionLockDto
    {
        public Guid LeftAssetId { get; set; }

        public Guid RightAssetId { get; set; }
    }
}
