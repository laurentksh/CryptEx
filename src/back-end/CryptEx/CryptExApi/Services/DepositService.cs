using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Data;
using CryptExApi.Models.Database;
using CryptExApi.Models.SignalR;
using CryptExApi.Models.ViewModel;
using CryptExApi.Models.ViewModel.Payment;
using CryptExApi.Repositories;
using CryptExApi.Utilities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Stripe.Checkout;

namespace CryptExApi.Services
{
    public interface IDepositService
    {
        Task<FiatDepositViewModel> CreatePaymentSession(decimal amount, AppUser user);

        Task<CryptoDepositViewModel> GenerateDepositWallet(Guid walletId, AppUser user);

        Task<List<DepositViewModel>> GetDeposits(AppUser user);

        Task<FiatDeposit> CreateFiatDeposit(AppUser user, Session session);

        Task UpdateDeposits(Guid userId);
    }

    public class DepositService : IDepositService
    {
        private readonly IConfiguration configuration;
        private readonly IHubContext<DepositHub> hubContext;
        private readonly IDepositRepository repository;
        private readonly IStripeRepository stripeRepository;
        private readonly IWalletRepository walletRepository;

        public DepositService(IConfiguration configuration, IHubContext<DepositHub> hubContext, IDepositRepository repository, IStripeRepository stripeRepository, IWalletRepository walletRepository)
        {
            this.configuration = configuration;
            this.hubContext = hubContext;
            this.repository = repository;
            this.stripeRepository = stripeRepository;
            this.walletRepository = walletRepository;
        }

        public async Task<FiatDepositViewModel> CreatePaymentSession(decimal amount, AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var currency = user.PreferedCurrency.ToLower();

            var paymentMethods = new List<string> //https://stripe.com/docs/api/payment_methods/object
            {
                "card" //Base payment method, available in every circumstance
            };

            switch (currency) { //TODO: Test every currency
                case "eur":
                    paymentMethods.AddRange(new List<string>
                    {
                        "ideal",
                        //"fpx",
                        //"bacs_debit",
                        "bancontact",
                        "giropay",
                        "p24",
                        "eps",
                        "sofort",
                        "sepa_debit",
                        //"grabpay",
                        //"afterpay_clearpay",
                        //"acss_debit"
                    });
                    break;
                case "chf":
                    paymentMethods.AddRange(new List<string>
                    {
                        //"alipay" //Supports CHF but doesn't support it at the same time ??
                    });
                    break;
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = paymentMethods,
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = currency,
                            UnitAmountDecimal = amount * 100,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Fiat {user.PreferedCurrency} Deposit",
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = configuration["BaseUrl"] + "/deposit-withdraw?fiatDepositStatus=success",
                CancelUrl = configuration["BaseUrl"] + "/deposit-withdraw?fiatDepositStatus=cancelled",
                CustomerEmail = user.Email,
                AllowPromotionCodes = true,
                SubmitType = "pay",
                ClientReferenceId = user.Id.ToString()
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            var deposit = await CreateFiatDeposit(user, session);

            await UpdateDeposits(user.Id);

            return FiatDepositViewModel.FromFiatDeposit(deposit);
        }

        public async Task<FiatDeposit> CreateFiatDeposit(AppUser user, Session session)
        {
            if (await stripeRepository.DepositExists(session.Id))
                throw new ArgumentException("Session already exists.");

            var fiatWallet = await walletRepository.GetFiatFull(session.Currency.ToLower());

            return await stripeRepository.CreateDeposit(new FiatDeposit
            {
                Status = Models.PaymentStatus.NotProcessed,
                StripeSessionId = session.Id,
                CreationDate = DateTime.UtcNow,
                Amount = Convert.ToDecimal((session.AmountTotal / 100) ?? 0),
                UserId = user.Id,
                User = user,
                WalletId = fiatWallet.Id,
                Wallet = fiatWallet
            });
        }

        public async Task<CryptoDepositViewModel> GenerateDepositWallet(Guid walletId, AppUser user)
        {
            await UpdateDeposits(user.Id);

            return new CryptoDepositViewModel
            {
                Id = Guid.NewGuid(),
                Amount = -1,
                Date = DateTime.UtcNow,
                WalletAddress = StringUtilities.ComputeHash(StringUtilities.SecureRandom(32)),
                WalletId = walletId
            };
        }

        public async Task<List<DepositViewModel>> GetDeposits(AppUser user)
        {
            return await repository.GetDeposits(user);
        }

        public async Task UpdateDeposits(Guid userId)
        {
            await hubContext.Clients.User(userId.ToString()).SendAsync(DepositHub.Name, await repository.GetDeposits(userId));
        }
    }
}
