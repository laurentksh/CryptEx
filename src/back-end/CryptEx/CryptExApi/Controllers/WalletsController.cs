using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Extensions;
using CryptExApi.Models.ViewModel;
using CryptExApi.Models.ViewModel.Wallets;
using CryptExApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CryptExApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly ILogger<WalletsController> logger;
        private readonly IWalletService walletsService;
        private readonly IExceptionHandlerService exceptionHandler;

        public WalletsController(ILogger<WalletsController> logger, IWalletService walletsService, IExceptionHandlerService exceptionHandler)
        {
            this.logger = logger;
            this.walletsService = walletsService;
            this.exceptionHandler = exceptionHandler;
        }

        [HttpGet("list")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<WalletViewModel>))]
        public async Task<IActionResult> GetAllWallets()
        {
            try {
                var currency = await HttpContext.GetCurrencyFromHeadersOrDefault();
                var user = await HttpContext.GetUser();
                var wallets = new List<WalletViewModel>();

                if (user != null)
                    currency = user.PreferedCurrency;

                wallets.AddRange(await walletsService.GetFiatWallets());
                wallets.AddRange(await walletsService.GetCryptoWallets(currency));

                return Ok(wallets);
            } catch (Exception ex) {
                logger.LogCritical(ex, "Could not get wallets. Application is severely compromised.");
                return exceptionHandler.Handle(ex, Request);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserWalletViewModel>))]
        public async Task<IActionResult> GetUserWallets()
        {
            try {
                var user = await HttpContext.GetUser();

                var wallets = new List<UserWalletViewModel>();
                wallets.AddRange(await walletsService.GetFiatWallets(user));
                wallets.AddRange(await walletsService.GetCryptoWallets(user));
                
                return Ok(wallets);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not get user wallets.");
                return exceptionHandler.Handle(ex, Request);
            }
        }

        [HttpGet("totals")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TotalsViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTotal()
        {
            try {
                var user = await HttpContext.GetUser();
                var totals = await walletsService.GetTotals(user);

                return Ok(totals);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not get total");
                return exceptionHandler.Handle(ex, Request);
            }
        }
    }
}
