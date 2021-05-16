using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Authentication;
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
        private readonly IExceptionHandlerService exceptionHandler;

        public PaymentController(ILogger<PaymentController> logger, IExceptionHandlerService exceptionHandler, IPaymentService paymentService)
        {
            this.logger = logger;
            this.exceptionHandler = exceptionHandler;
            this.paymentService = paymentService;
        }

        [HttpPost("deposit/fiat")]
        public async Task<IActionResult> DepositFiat([FromQuery] decimal amount)
        {
            var user = await HttpContext.GetUser();

            try {
                var session = await paymentService.CreatePaymentSession(amount, user);

                return Ok(session);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not create payment session.");
                return exceptionHandler.Handle(ex, Request);
            }
        }

        [HttpPost("deposit/crypto")]
        public async Task<IActionResult> DepositCrypto([FromQuery] int walletId)
        {
            var user = await HttpContext.GetUser();

            try {
                var address = await paymentService.GenerateDepositWallet(walletId, user);
                
                return Ok(address);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not generate deposit wallet address.");
                return exceptionHandler.Handle(ex, Request);
            }
        }
    }
}
