using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Extensions;
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
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> logger;
        private readonly IPaymentService paymentService;
        private readonly IDepositService depositService;
        private readonly IExceptionHandlerService exceptionHandler;


        public PaymentController(ILogger<PaymentController> logger, IExceptionHandlerService exceptionHandler, IPaymentService paymentService, IDepositService depositService)
        {
            this.logger = logger;
            this.exceptionHandler = exceptionHandler;
            this.paymentService = paymentService;
            this.depositService = depositService;
        }

        [HttpPost("deposit/fiat")]
        public async Task<IActionResult> DepositFiat([FromQuery] decimal amount)
        {
            var user = await HttpContext.GetUser();

            try {
                var session = await paymentService.DepositFiat(amount, user);

                return Ok(session);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not create payment session.");
                return exceptionHandler.Handle(ex, Request);
            }
        }

        [HttpPost("deposit/crypto")]
        public async Task<IActionResult> DepositCrypto([FromQuery] Guid walletId)
        {
            var user = await HttpContext.GetUser();

            try {
                var address = await paymentService.DepositCrypto(walletId, user);
                
                return Ok(address);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not generate deposit wallet address.");
                return exceptionHandler.Handle(ex, Request);
            }
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> WithdrawFiat([FromQuery] decimal amount)
        {
            var user = await HttpContext.GetUser();

            try {
                await paymentService.WithdrawFiat(user, amount);

                return Ok();
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not generate deposit wallet address.");
                return exceptionHandler.Handle(ex, Request);
            }
        }

        [HttpGet("deposits")]
        public async Task<IActionResult> GetDeposits([FromQuery] bool signalR = false)
        {
            try {
                if (signalR) {
                    await depositService.UpdateDeposits(HttpContext.GetUserId());
                    return Ok();
                } else {
                    return Ok(await depositService.GetDeposits(await HttpContext.GetUser()));
                }
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not return deposits.");
                return exceptionHandler.Handle(ex, Request);
            }
        }
    }
}
