using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Extensions;
using CryptExApi.Models.DTO;
using CryptExApi.Models.SignalR;
using CryptExApi.Models.ViewModel;
using CryptExApi.Services;
using CryptExApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace CryptExApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AssetConvertController : ControllerBase
    {
        private readonly ILogger<AssetConvertController> logger;
        private readonly IAssetConvertService convertService;
        private readonly IExceptionHandlerService exceptionHandler;
        private readonly IHubContext<AssetConversionHub> hub;

        public AssetConvertController(ILogger<AssetConvertController> logger, IAssetConvertService convertService, IExceptionHandlerService exceptionHandler, IHubContext<AssetConversionHub> hub)
        {
            this.logger = logger;
            this.convertService = convertService;
            this.exceptionHandler = exceptionHandler;
            this.hub = hub;
        }

        [HttpPost("convert")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Convert(AssetConvertDto dto)
        {
            try {
                var user = await HttpContext.GetUser();
                var transactionId = await convertService.Convert(user, dto);

                /*_ = new TimerManager(async () =>
                  {
                      await hub.Clients.User(user.Id.ToString()).SendAsync(AssetConversionHub.Name, await convertService.GetTransaction(user, transactionId));
                  });*/

                return Ok(transactionId);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not convert assets.");
                return exceptionHandler.Handle(ex, Request);
            }
        }

        [HttpGet("transaction")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AssetConversionViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransaction([FromQuery] Guid id)
        {
            try {
                var user = await HttpContext.GetUser();
                var transaction = await convertService.GetTransaction(user, id);

                return Ok(transaction);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not convert assets.");
                return exceptionHandler.Handle(ex, Request);
            }
        }
    }
}
