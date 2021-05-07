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

        Task<string> GenerateDepositWallet(int walletId, AppUser user);
    }

    public class PaymentService : IPaymentService
    {
        public PaymentService()
        {

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
                            Currency = "usd",
                            UnitAmountDecimal = amount,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Fiat USD Deposit",
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

            return session;
        }

        public Task<string> GenerateDepositWallet(int walletId, AppUser user)
        {
            throw new NotImplementedException();
        }
    }
}
