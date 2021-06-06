using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models.Database;
using CryptExApi.Models.ViewModel;
using CryptExApi.Models.ViewModel.Payment;
using Microsoft.Extensions.Configuration;
using Stripe.Checkout;

namespace CryptExApi.Services
{
    public interface IPaymentService
    {
        Task<FiatDepositViewModel> DepositFiat(decimal amount, AppUser user);

        Task<CryptoDepositViewModel> DepositCrypto(Guid walletId, AppUser user);

        Task WithdrawFiat(AppUser user, decimal amount);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IDepositService depositService;

        public PaymentService(IDepositService depositService)
        {
            this.depositService = depositService;
        }

        public async Task<FiatDepositViewModel> DepositFiat(decimal amount, AppUser user)
        {
            return await depositService.CreatePaymentSession(amount, user);
        }

        public async Task<CryptoDepositViewModel> DepositCrypto(Guid walletId, AppUser user)
        {
            return await depositService.GenerateDepositWallet(walletId, user);
        }

        public Task WithdrawFiat(AppUser user, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
