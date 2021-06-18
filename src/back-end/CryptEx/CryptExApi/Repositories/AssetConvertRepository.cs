using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Data;
using CryptExApi.Models.Database;
using CryptExApi.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace CryptExApi.Repositories
{
    public interface IAssetConvertRepository
    {
        Task<AssetConversionLock> LockTransaction(Wallet left, Wallet right, decimal exchangeRate);

        Task<Guid> Convert(AppUser user, AssetConversionLock priceLock, decimal amount);

        Task<AssetConversion> GetTransaction(AppUser user, Guid transactionId);

        Task<List<AssetConversion>> GetTransactions(AppUser user);

        Task<List<AssetConversion>> GetOngoingTransactions(Guid id);

        Task<AssetConversionLock> GetTransactionLock(Guid transactionLockId);

        Task<List<AssetConversionLock>> GetTransactionLocks(AppUser user);
    }

    public class AssetConvertRepository : IAssetConvertRepository
    {
        private readonly CryptExDbContext dbContext;

        public AssetConvertRepository(CryptExDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<AssetConversionLock> LockTransaction(Wallet left, Wallet right, decimal exchangeRate)
        {
            var result = await dbContext.AssetConversionLocks.AddAsync(new AssetConversionLock
            {
                Left = left,
                LeftId = left.Id,
                Right = right,
                RightId = right.Id,
                ExchangeRate = exchangeRate
            });

            await dbContext.SaveChangesAsync();
            
            return result.Entity;
        }

        public async Task<Guid> Convert(AppUser user, AssetConversionLock priceLock, decimal amount)
        {
            var result = await dbContext.AssetConversions.AddAsync(new AssetConversion
            {
                Amount = amount,
                Status = Models.PaymentStatus.NotProcessed,
                PriceLock = priceLock,
                PriceLockId = priceLock.Id,
                User = user,
                UserId = user.Id,
            });

            await dbContext.SaveChangesAsync();

            return result.Entity.Id;
        }

        public async Task<AssetConversion> GetTransaction(AppUser user, Guid transactionId)
        {
            return await dbContext.AssetConversions
                .Include(x => x.PriceLock)
                    .ThenInclude(x => x.Left)
                .Include(x => x.PriceLock)
                    .ThenInclude(x => x.Right)
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.UserId == user.Id && x.Id == transactionId);
        }

        public async Task<List<AssetConversion>> GetTransactions(AppUser user)
        {
            return await dbContext.AssetConversions
                .Include(x => x.PriceLock)
                    .ThenInclude(x => x.Left)
                .Include(x => x.PriceLock)
                    .ThenInclude(x => x.Right)
                .Include(x => x.User)
                .Where(x => x.UserId == user.Id).ToListAsync();
        }

        public async Task<List<AssetConversion>> GetOngoingTransactions(Guid id)
        {
            return await dbContext.AssetConversions
                .Include(x => x.PriceLock)
                    .ThenInclude(x => x.Left)
                .Include(x => x.PriceLock)
                    .ThenInclude(x => x.Right)
                .Include(x => x.User)
                .Where(x =>
                    x.UserId == id &&
                    (x.Status == Models.PaymentStatus.NotProcessed || x.Status == Models.PaymentStatus.Pending))
                .ToListAsync();
        }

        public async Task<AssetConversionLock> GetTransactionLock(Guid transactionLockId)
        {
            return await dbContext.AssetConversionLocks
                .Include(x => x.Left)
                .Include(x => x.Right)
                .Include(x => x.Conversion)
                .SingleOrDefaultAsync(x => x.Id == transactionLockId);
        }

        public async Task<List<AssetConversionLock>> GetTransactionLocks(AppUser user)
        {
            return await dbContext.AssetConversionLocks
                .Include(x => x.Left)
                .Include(x => x.Right)
                .Include(x => x.Conversion)
                .ToListAsync();
        }
    }
}
