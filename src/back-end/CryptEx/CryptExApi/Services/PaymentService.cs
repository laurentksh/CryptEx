using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models.Database;
using Stripe.Checkout;

namespace CryptExApi.Services
{
    public interface IPaymentService
    {
        Task<Session> CreatePaymentSession(decimal amount, AppUser user);

        Task<string> GenerateDepositWallet(Guid walletId, AppUser user);

        Task WithdrawFiat(AppUser user, decimal amount);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IStripeService stripeService;

        public PaymentService(IStripeService stripeService)
        {
            this.stripeService = stripeService;
        }

        public async Task<Session> CreatePaymentSession(decimal amount, AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> //https://stripe.com/docs/api/payment_methods/object
                {
                    "card",
                    "sofort"
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = user.PreferedCurrency.ToLower(),
                            UnitAmountDecimal = amount,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Fiat {user.PreferedCurrency} Deposit",
                            }
                        },
                        Quantity = 1,
                    }
                },
                Mode = "payment",
                SuccessUrl = "/checkout/success",
                CancelUrl = "/checkout/cancel",
                CustomerEmail = user.Email
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            await stripeService.CreateDeposit(session);

            return session;
        }

        public Task<string> GenerateDepositWallet(Guid walletId, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task WithdrawFiat(AppUser user, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
