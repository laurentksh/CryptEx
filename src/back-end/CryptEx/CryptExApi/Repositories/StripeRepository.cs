using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Data;
using CryptExApi.Exceptions;
using CryptExApi.Models;
using CryptExApi.Models.Database;

namespace CryptExApi.Repositories
{
    public interface IStripeRepository
    {
        Task<FiatDeposit> CreateDeposit(FiatDeposit fiatDeposit);

        Task<bool> DepositExists(string sessionId);

        Task<FiatDeposit> SetDepositStatus(string sessionId, PaymentStatus status);
    }

    public class StripeRepository : IStripeRepository
    {
        private readonly CryptExDbContext DbContext;

        public StripeRepository(CryptExDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<FiatDeposit> CreateDeposit(FiatDeposit fiatDeposit)
        {
            await DbContext.FiatDeposits.AddAsync(fiatDeposit);
            await DbContext.SaveChangesAsync();

            return fiatDeposit;
        }

        public async Task<bool> DepositExists(string sessionId)
        {
            return DbContext.FiatDeposits.Any(x => x.StripeSessionId == sessionId);
        }

        public async Task<FiatDeposit> SetDepositStatus(string sessionId, PaymentStatus status)
        {
            var deposit = DbContext.FiatDeposits.SingleOrDefault(x => x.StripeSessionId == sessionId);
            deposit.Status = status;

            await DbContext.SaveChangesAsync();

            return deposit;
        }
    }
}
