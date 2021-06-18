using CryptExApi.Services;
using CryptExApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.SignalR
{
    [Authorize]
    public class AssetConversionHub : Hub
    {
        private readonly ILogger<AssetConversionHub> logger;
        private readonly IAssetConvertService convertService;

        public AssetConversionHub(ILogger<AssetConversionHub> logger, IAssetConvertService convertService)
        {
            this.logger = logger;
            this.convertService = convertService;
        }

        public const string Name = "assetconversiondata";

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            logger.LogWarning($"SignalR connected: " + Context.UserIdentifier);

            _ = new TimerManager(async () =>
            {
                await Clients.Caller.SendAsync(Name, await convertService.GetOngoingTransactions(Guid.Parse(Context.UserIdentifier)));
            }, Context.ConnectionAborted);
        }
    }
}
