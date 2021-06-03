using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Exceptions;
using CryptExApi.Models;
using CryptExApi.Models.Database;
using CryptExApi.Repositories;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;

namespace CryptExApi.Services
{
    public interface IStripeService
    {
        Task HandleCheckoutCallback(string jsonBody, string stripeSignature);

        Task<FiatDeposit> CreateDeposit(Session session);

        Task FullfillDeposit(Session session);

        Task SetDepositAsFailed(Session session);
    }

    public class StripeService : IStripeService
    {
        private readonly IStripeRepository repository;
        private readonly IConfiguration configuration;

        public StripeService(IConfiguration configuration, IStripeRepository repository)
        {
            this.configuration = configuration;
            this.repository = repository;
        }

        public async Task HandleCheckoutCallback(string jsonBody, string stripeSignature)
        {
            var stripeEvent = EventUtility.ConstructEvent(jsonBody, stripeSignature, configuration["WHCheckoutCallbackSecret"]);
            Session session;

            switch (stripeEvent.Type) {
                case Events.CheckoutSessionCompleted:
                    session = stripeEvent.Data.Object as Session;

                    if (!await repository.DepositExists(session.Id)) // Save the deposit in DB, mark it as "processing".
                        await CreateDeposit(session);

                    if (session.PaymentStatus == "paid") { // Payment successfull (probably paid with Card as success was instant)
                        await FullfillDeposit(session);
                    } else {
                        await SetDepositAsPending(session);
                    }

                    break;
                case Events.CheckoutSessionAsyncPaymentSucceeded:
                    session = stripeEvent.Data.Object as Session;

                    await FullfillDeposit(session);
                    break;
                case Events.CheckoutSessionAsyncPaymentFailed:
                    session = stripeEvent.Data.Object as Session;

                    await SetDepositAsFailed(session); //Payment failed.
                    break;
            }
        }

        public async Task<FiatDeposit> CreateDeposit(Session session)
        {
            if (await repository.DepositExists(session.Id))
                throw new ArgumentException("Session already exists.");

            return await repository.CreateDeposit(new FiatDeposit
            {
                Status = PaymentStatus.NotProcessed,
                StripeSessionId = session.Id,
                Amount = Convert.ToDecimal(session.AmountTotal ?? 0)
            });
        }

        public async Task FullfillDeposit(Session session)
        {
            if (!await repository.DepositExists(session.Id))
                throw new NotFoundException($"Session with id {session.Id} does not exist.");

            await repository.SetDepositStatus(session.Id, PaymentStatus.Success);
        }

        public async Task SetDepositAsFailed(Session session)
        {
            if (!await repository.DepositExists(session.Id))
                throw new NotFoundException($"Session with id {session.Id} does not exist.");

            await repository.SetDepositStatus(session.Id, PaymentStatus.Failed);
        }

        public async Task SetDepositAsPending(Session session)
        {
            if (!await repository.DepositExists(session.Id))
                throw new NotFoundException($"Session with id {session.Id} does not exist.");

            await repository.SetDepositStatus(session.Id, PaymentStatus.Pending);
        }
    }
}
