using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.SignalR
{
    [Authorize]
    public class AssetConversionHub : Hub
    {
        public const string Name = "assetconversiondata";
    }
}
