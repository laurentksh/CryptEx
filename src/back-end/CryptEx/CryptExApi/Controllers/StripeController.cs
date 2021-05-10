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
        private readonly IConfiguration configuration;
        private readonly IStripeService stripeService;

        public StripeController(ILogger<StripeController> logger, IConfiguration configuration, IStripeService stripeService)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.stripeService = stripeService;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckoutCallback()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], configuration["WHCheckoutCallbackSecret"]);
                Session session;

                switch (stripeEvent.Type) {
                    case Events.CheckoutSessionCompleted:
                        session = stripeEvent.Data.Object as Session;

                        if (session.PaymentStatus == "paid") { // Payment successfull (probably paid with Card as success was instant)
                            await stripeService.FullfillDeposit(session);
                        } else {
                            // Save the deposit in DB, mark it as "processing".
                            await stripeService.CreateDeposit(session);
                        }

                        break;
                    case Events.CheckoutSessionAsyncPaymentSucceeded:
                        session = stripeEvent.Data.Object as Session;

                        await stripeService.FullfillDeposit(session);
                        break;
                    case Events.CheckoutSessionAsyncPaymentFailed:
                        session = stripeEvent.Data.Object as Session;

                        await stripeService.SetDepositAsFailed(session); //Payment failed.
                        break;
                }

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
