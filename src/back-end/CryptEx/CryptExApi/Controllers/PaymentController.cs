using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public PaymentController(ILogger<PaymentController> logger, IExceptionHandlerService exceptionHandler)
        {
            this.logger = logger;
            this.exceptionHandler = exceptionHandler;
        }

        [Route("deposit/fiat")]
        public async Task<IActionResult> DepositFiat([FromQuery] decimal amount)
        {
            try {
                var session = await paymentService.CreatePaymentSession(amount);
                return Ok(session);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could create payment session.");
                return exceptionHandler.Handle(ex, Request);
            }
        }

        [Route("deposit/crypto")]
        public async Task<IActionResult> DepositCrypto([FromQuery] int walletId)
        {
            try {
                var address = await paymentService.GenerateDepositWallet();
                
                return Ok(address);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not generate deposit wallet address.");
                return exceptionHandler.Handle(ex, Request);
            }
        }
    }
}
