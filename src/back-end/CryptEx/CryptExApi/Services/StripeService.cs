using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Exceptions;
using CryptExApi.Models;
using CryptExApi.Models.Database;
using CryptExApi.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;

namespace CryptExApi.Services
{
    public interface IStripeService
    {
        Task HandleCheckoutCallback(string jsonBody, string stripeSignature);

        Task FullfillDeposit(Session session);

        Task SetDepositAsFailed(Session session);
    }

    public class StripeService : IStripeService
    {
        private readonly IStripeRepository repository;
        private readonly IConfiguration configuration;
        private readonly IDepositService depositService;

        public StripeService(IConfiguration configuration, IStripeRepository repository, IDepositService depositService)
        {
            this.configuration = configuration;
            this.repository = repository;
            this.depositService = depositService;
        }

        public async Task HandleCheckoutCallback(string jsonBody, string stripeSignature)
        {
            var stripeEvent = EventUtility.ConstructEvent(jsonBody, stripeSignature, configuration["WHCheckoutCallbackSecret"]);
            Session session;
            
            switch (stripeEvent.Type) {
                case Events.CheckoutSessionCompleted:
                    session = stripeEvent.Data.Object as Session;

                    if (!await repository.DepositExists(session.Id))
                        throw new NotFoundException("Session does not exist.");

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

        public async Task FullfillDeposit(Session session)
        {
            if (!await repository.DepositExists(session.Id))
                throw new NotFoundException($"Session with id {session.Id} does not exist.");
            
            var deposit = await repository.SetDepositStatus(session.Id, PaymentStatus.Success);

            await depositService.UpdateDeposits(deposit.UserId);
        }

        public async Task SetDepositAsFailed(Session session)
        {
            if (!await repository.DepositExists(session.Id))
                throw new NotFoundException($"Session with id {session.Id} does not exist.");

            var deposit = await repository.SetDepositStatus(session.Id, PaymentStatus.Failed);
            await depositService.UpdateDeposits(deposit.UserId);
        }

        public async Task SetDepositAsPending(Session session)
        {
            if (!await repository.DepositExists(session.Id))
                throw new NotFoundException($"Session with id {session.Id} does not exist.");

            var deposit = await repository.SetDepositStatus(session.Id, PaymentStatus.Pending);
            await depositService.UpdateDeposits(deposit.UserId);
        }
    }
}
