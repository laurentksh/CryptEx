using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Extensions;
using CryptExApi.Models.DTO;
using CryptExApi.Models.DTO.AssetConvert;
using CryptExApi.Models.SignalR;
using CryptExApi.Models.ViewModel.AssetConvert;
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
        public async Task<IActionResult> Convert(AssetConversionDto dto)
        {
            try {
                var user = await HttpContext.GetUser();
                var transactionId = await convertService.Convert(user, dto);

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
        public async Task<IActionResult> GetTransactions([FromQuery] Guid id)
        {
            try {
                var user = await HttpContext.GetUser();
                var transactions = await convertService.GetTransaction(user, id);

                return Ok(transactions);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not get transactions.");
                return exceptionHandler.Handle(ex, Request);
            }
        }

        [HttpGet("transactions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AssetConversionViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactions()
        {
            try {
                var user = await HttpContext.GetUser();
                var transactions = await convertService.GetTransactions(user);

                return Ok(transactions);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not get transactions.");
                return exceptionHandler.Handle(ex, Request);
            }
        }

        [HttpGet("lock")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AssetConversionLockViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactionLock([FromQuery] Guid id)
        {
            try {
                var user = await HttpContext.GetUser();
                var transactionLock = await convertService.GetTransactionLock(user, id);

                return Ok(transactionLock);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not get transaction lock.");
                return exceptionHandler.Handle(ex, Request);
            }
        }

        [HttpPost("lock")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AssetConversionLockViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RequestTransactionLock([FromBody] AssetConvertionLockDto dto)
        {
            try {
                var user = await HttpContext.GetUser();
                var transactionLock = await convertService.LockTransaction(user, dto);

                return Ok(transactionLock);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not request transaction lock.");
                return exceptionHandler.Handle(ex, Request);
            }
        }

        [HttpDelete("lock")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AssetConversionLockViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveTransactionLock([FromQuery] Guid id)
        {
            try {
                var user = await HttpContext.GetUser();
                await convertService.RemoveTransactionLock(user, id);

                return Ok();
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not get transaction lock.");
                return exceptionHandler.Handle(ex, Request);
            }
        }
    }
}
