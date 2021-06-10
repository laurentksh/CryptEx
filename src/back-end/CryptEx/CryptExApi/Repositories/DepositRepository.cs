using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Data;
using CryptExApi.Models.Database;
using CryptExApi.Models.ViewModel.Payment;
using Microsoft.EntityFrameworkCore;

namespace CryptExApi.Repositories
{
    public interface IDepositRepository
    {
        Task<List<DepositViewModel>> GetDeposits(AppUser user);

        Task<List<DepositViewModel>> GetDeposits(Guid userId);
    }

    public class DepositRepository : IDepositRepository
    {
        private readonly CryptExDbContext dbContext;

        public DepositRepository(CryptExDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<DepositViewModel>> GetDeposits(AppUser user)
        {
            return await GetDeposits(user.Id);
        }

        public async Task<List<DepositViewModel>> GetDeposits(Guid userId)
        {
            var fiatDeposits = dbContext.FiatDeposits
                .Include(x => x.Wallet)
                .Where(x => x.UserId == userId)
                .ToList();

            var cryptoDeposits = dbContext.CryptoDeposits
                .Include(x => x.Wallet)
                .Where(x => x.UserId == userId)
                .ToList();

            var result = new List<DepositViewModel>(fiatDeposits.Count + cryptoDeposits.Count);
            result.AddRange(fiatDeposits.Select(x => FiatDepositViewModel.FromFiatDeposit(x)));
            result.AddRange(cryptoDeposits.Select(x => CryptoDepositViewModel.FromCryptoDeposit(x)));

            return result;
        }
    }
}
