using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models.Database;
using CryptExApi.Models.ViewModel;
using CryptExApi.Models.ViewModel.Payment;
using CryptExApi.Repositories;
using Microsoft.Extensions.Configuration;
using Stripe.Checkout;

namespace CryptExApi.Services
{
    public interface IPaymentService
    {
        Task<FiatDepositViewModel> DepositFiat(decimal amount, AppUser user);

        Task<CryptoDepositViewModel> DepositCrypto(Guid walletId, AppUser user);

        Task WithdrawFiat(Guid userId, decimal amount);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IDepositService depositService;
        private readonly IPaymentRepository paymentRepository;

        public PaymentService(IDepositService depositService, IPaymentRepository paymentRepository)
        {
            this.depositService = depositService;
            this.paymentRepository = paymentRepository;
        }

        public async Task<FiatDepositViewModel> DepositFiat(decimal amount, AppUser user)
        {
            return await depositService.CreatePaymentSession(amount, user);
        }

        public async Task<CryptoDepositViewModel> DepositCrypto(Guid walletId, AppUser user)
        {
            return await depositService.GenerateDepositWallet(walletId, user);
        }

        public async Task WithdrawFiat(Guid userId, decimal amount)
        {
            await paymentRepository.WithdrawFiat(userId, amount);
        }
    }
}
