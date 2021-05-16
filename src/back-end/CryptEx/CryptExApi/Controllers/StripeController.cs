using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;

namespace CryptExApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase // https://stripe.com/docs/payments/checkout/fulfill-orders
    {
        private readonly ILogger<StripeController> logger;
        
        private readonly IStripeService stripeService;

        public StripeController(ILogger<StripeController> logger, IConfiguration configuration, IStripeService stripeService)
        {
            this.logger = logger;
            this.stripeService = stripeService;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckoutCallback()
        {
            try {
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

                await stripeService.HandleCheckoutCallback(json, Request.Headers["Stripe-Signature"]);
                return Ok();
            } catch (StripeException stripeEx) {
                logger.LogCritical(stripeEx, "Could not handle Stripe callback.");
                return BadRequest();
            } catch (Exception ex) {
                logger.LogCritical(ex, "Error.");
                return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "StripeEventProcessError");
            }
        }
    }
}
