using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models;
using CryptExApi.Models.Database;
using CryptExApi.Models.ViewModel;
using CryptExApi.Models.ViewModel.Payment;
using CryptExApi.Repositories;

namespace CryptExApi.Services
{
    public interface IAdminService
    {
        Task<List<DepositViewModel>> GetAllDeposits(Guid? userId, PaymentStatus? status = null, WalletType type = WalletType.Fiat);

        Task SetPaymentStatus(string sessionId, PaymentStatus status);

        Task<List<BankAccountViewModel>> GetPendingBankAccounts();

        Task SetBankAccountStatus(Guid id, BankAccountStatus status);
    }

    public class AdminService : IAdminService
    {
        private readonly IStripeRepository stripeRepo;
        private readonly IAdminRepository adminRepo;

        public AdminService(IStripeRepository stripeRepo)
        {
            this.stripeRepo = stripeRepo;
        }

        public async Task<List<DepositViewModel>> GetAllDeposits(Guid? userId, PaymentStatus? status = null, WalletType type = WalletType.Fiat)
        {
            return await adminRepo.GetAllDeposits(userId, status, type);
        }

        public async Task SetPaymentStatus(string sessionId, PaymentStatus status)
        {
            await stripeRepo.SetDepositStatus(sessionId, status);
        }

        public async Task<List<BankAccountViewModel>> GetPendingBankAccounts()
        {
            return await adminRepo.GetPendingBankAccounts();
        }

        public async Task SetBankAccountStatus(Guid id, BankAccountStatus status)
        {
            await adminRepo.SetBankAccountStatus(id, status);
        }
    }
}
