using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Data;
using CryptExApi.Exceptions;
using CryptExApi.Models;
using CryptExApi.Models.Database;
using CryptExApi.Models.ViewModel;
using CryptExApi.Models.ViewModel.Admin;
using CryptExApi.Models.ViewModel.Payment;
using Microsoft.EntityFrameworkCore;

namespace CryptExApi.Repositories
{
    public interface IAdminRepository
    {
        Task<List<UserViewModel>> SearchUser(string query);

        Task<StatsViewModel> GetStats();

        Task<List<DepositViewModel>> GetAllDeposits(Guid? userId, PaymentStatus? status = null, WalletType type = WalletType.Fiat);

        Task<List<BankAccountViewModel>> GetPendingBankAccounts();

        Task SetBankAccountStatus(Guid id, BankAccountStatus status);
    }
    public class AdminRepository : IAdminRepository
    {
        private readonly CryptExDbContext dbContext;

        public AdminRepository(CryptExDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<UserViewModel>> SearchUser(string query)
        {
            var users = (await dbContext.Users
                .Include(x => x.Address)
                    .ThenInclude(x => x.Country)
                .ToListAsync())
                .Where(x =>
                {
                    var match = false;
                    match = x.FirstName.Contains(query);
                    match = x.LastName.Contains(query);
                    match = x.Email.Contains(query);
                    if (x.Address != null)
                        match = x.Address.ToString().Contains(query);

                    return match;
                })
                .Take(25)
                .Select(x => UserViewModel.FromAppUser(x));

            return users.ToList();
        }

        public async Task<StatsViewModel> GetStats()
        {
            //This method is quite resource intensive, in a real world application we would use tools like caching to reduce the performance impact.
            var result = new StatsViewModel();

            result.TotalUsers = await GetTotalUsers();
            result.TotalTradedAmount = await GetTotalTraded();

            result.NewUsers24h = await GetTotalUsers(x => x > DateTime.UtcNow.AddHours(-24));
            result.NewUsers7d = await GetTotalUsers(x => x > DateTime.UtcNow.AddDays(-7));
            result.NewUsers30d = await GetTotalUsers(x => x > DateTime.UtcNow.AddDays(-30));

            result.TradedAmount24h = await GetTotalTraded(x => x > DateTime.UtcNow.AddHours(-24));
            result.TradedAmount7d = await GetTotalTraded(x => x > DateTime.UtcNow.AddDays(-7));
            result.TradedAmount30d = await GetTotalTraded(x => x > DateTime.UtcNow.AddDays(-30));

            return result;
        }

        private async Task<long> GetTotalUsers(Func<DateTime, bool> time = null)
        {
            return (await dbContext.Users
                .ToListAsync())
                .Where(x => time == null || time(x.CreationDate)).LongCount();
        }

        private async Task<decimal> GetTotalTraded(Func<DateTime, bool> time = null)
        {
            decimal total = 0;

            foreach (var deposit in (await dbContext.FiatDeposits.ToListAsync()).Where(x => time == null || time(x.CreationDate)).ToList())
                total += deposit.Amount;
            foreach (var deposit in (await dbContext.CryptoDeposits.ToListAsync()).Where(x => time == null || time(x.CreationDate)).ToList())
                total += deposit.Amount;
            foreach (var withdraw in (await dbContext.FiatWithdrawals.ToListAsync()).Where(x => time == null || time(x.CreationDate)))
                total += withdraw.Amount;

            return total;
        }

        public async Task<List<DepositViewModel>> GetAllDeposits(Guid? userId, PaymentStatus? status = null, WalletType type = WalletType.Fiat)
        {
            var fiatDeposits = new List<FiatDeposit>();

            if (type == WalletType.Fiat) {
                fiatDeposits = dbContext.FiatDeposits
                    .Include(x => x.Wallet)
                    .Where(x => !userId.HasValue || x.UserId == userId.Value)
                    .Where(x => !status.HasValue || x.Status == status.Value)
                    .ToList();
            }

            var cryptoDeposits = new List<CryptoDeposit>();

            if (type == WalletType.Crypto) {
                cryptoDeposits = dbContext.CryptoDeposits
                    .Include(x => x.Wallet)
                    .Where(x => !userId.HasValue || x.UserId == userId.Value)
                    .Where(x => !status.HasValue || x.Status == status.Value)
                    .ToList();
            }

            var result = new List<DepositViewModel>(fiatDeposits.Count + cryptoDeposits.Count);
            result.AddRange(fiatDeposits.Select(x => FiatDepositViewModel.FromFiatDeposit(x)));
            result.AddRange(cryptoDeposits.Select(x => CryptoDepositViewModel.FromCryptoDeposit(x)));

            return result;
        }

        public async Task<List<BankAccountViewModel>> GetPendingBankAccounts()
        {
            var bankAccounts = dbContext.BankAccounts.Where(x => x.Status == BankAccountStatus.NotProcessed).ToList();

            return bankAccounts.Select(x => BankAccountViewModel.FromBankAccount(x)).ToList();
        }

        public async Task SetBankAccountStatus(Guid id, BankAccountStatus status)
        {
            var account = await dbContext.BankAccounts.SingleOrDefaultAsync(x => x.Id == id);

            if (account == null)
                throw new NotFoundException("Bank account not found.");

            account.Status = status;

            await dbContext.SaveChangesAsync();
        }
    }
}
