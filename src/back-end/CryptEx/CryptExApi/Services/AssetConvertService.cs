using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models.Database;
using CryptExApi.Models.DTO;

namespace CryptExApi.Services
{
    public interface IAssetConvertService
    {
        Task Convert(AppUser user, AssetConvertDto dto);
    }

    public class AssetConvertService : IAssetConvertService
    {
        public Task Convert(AppUser user, AssetConvertDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
