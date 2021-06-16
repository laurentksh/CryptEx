using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Data;
using CryptExApi.Exceptions;
using CryptExApi.Models.Database;

namespace CryptExApi.Repositories
{
    public interface IPaymentRepository
    {
        Task WithdrawFiat(Guid userId, decimal amount);
    }

    public class PaymentRepository : IPaymentRepository
    {
        private readonly CryptExDbContext dbContext;
        private readonly IUserRepository userRepository;
        private readonly IWalletRepository walletRepository;

        public PaymentRepository(CryptExDbContext dbContext, IUserRepository userRepository, IWalletRepository walletRepository)
        {
            this.dbContext = dbContext;
            this.userRepository = userRepository;
            this.walletRepository = walletRepository;
        }

        public async Task WithdrawFiat(Guid userId, decimal amount)
        {
            var user = await userRepository.GetFullUser(userId);
            var wallet = await walletRepository.GetFiatFull(user.PreferedCurrency);
            
            if (user == null)
                throw new NotFoundException($"User with id {userId} does not exist.");
            if (wallet == null)
                throw new NullReferenceException("Wallet does not exist.");

            if (user.Status != AccountStatus.Active)
                throw new ForbiddenException("User account inactive. Please contact our support.");

            if (user.BankAccount == null || user.BankAccount.Status != BankAccountStatus.Approved)
                throw new ForbiddenException("Your bank account isn't approved.");

            var wallets = await walletRepository.GetFiatWallets(user);

            var availableFiat = wallets.Single(x => x.Id == wallet.Id).Amount;

            if (amount > availableFiat)
                throw new InsufficientFundsException($"You do not have enough {wallet.Ticker} funds. (Available: {availableFiat}, Requested: {amount})");

            await dbContext.FiatWithdrawals.AddAsync(new FiatWithdrawal
            {
                Amount = amount,
                BankAccount = user.BankAccount,
                BankAccountId = user.BankAccount.Id,
                Status = Models.PaymentStatus.NotProcessed,
                CreationDate = DateTime.UtcNow,
                UserId = userId,
                User = user,
                Wallet = wallet,
                WalletId = wallet.Id
            });
        }
    }
}
