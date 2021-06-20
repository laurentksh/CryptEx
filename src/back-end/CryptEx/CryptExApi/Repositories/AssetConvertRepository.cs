using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Data;
using CryptExApi.Exceptions;
using CryptExApi.Models.Database;
using CryptExApi.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace CryptExApi.Repositories
{
    public interface IAssetConvertRepository
    {
        Task<AssetConversionLock> LockTransaction(AppUser user, Wallet left, Wallet right, decimal exchangeRate);

        Task RemoveTransactionLock(AppUser user, Guid id);

        Task<AssetConversionLock> GetTransactionLock(Guid transactionLockId);

        Task<List<AssetConversionLock>> GetTransactionLocks(AppUser user);

        Task<Guid> Convert(AppUser user, AssetConversionLock priceLock, decimal amount);

        Task<AssetConversion> GetTransaction(AppUser user, Guid transactionId);

        Task<List<AssetConversion>> GetTransactions(AppUser user);

        Task<List<AssetConversion>> GetOngoingTransactions(Guid id);
    }

    public class AssetConvertRepository : IAssetConvertRepository
    {
        private readonly CryptExDbContext dbContext;

        public AssetConvertRepository(CryptExDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<AssetConversionLock> LockTransaction(AppUser user, Wallet left, Wallet right, decimal exchangeRate)
        {
            var result = await dbContext.AssetConversionLocks.AddAsync(new AssetConversionLock
            {
                Left = left,
                LeftId = left.Id,
                Right = right,
                RightId = right.Id,
                User = user,
                UserId = user.Id,
                ExpirationUtc = DateTime.UtcNow.AddSeconds(60),
                ExchangeRate = exchangeRate
            });

            await dbContext.SaveChangesAsync();
            await result.ReloadAsync();

            return result.Entity;
        }

        public async Task RemoveTransactionLock(AppUser user, Guid id)
        {
            var tLock = await dbContext.AssetConversionLocks.SingleOrDefaultAsync(x => x.Id == id);

            if (tLock == null)
                throw new NotFoundException("Transaction lock does not exist.");

            if (tLock.UserId != user.Id)
                throw new ForbiddenException("You do not own this transaction lock.");

            dbContext.AssetConversionLocks.Remove(tLock);

            await dbContext.SaveChangesAsync();
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
            var tLock = await dbContext.AssetConversionLocks
                .Include(x => x.Left)
                .Include(x => x.Right)
                .Include(x => x.Conversion)
                .SingleOrDefaultAsync(x => x.Id == transactionLockId);

            if (tLock == null)
                throw new NotFoundException("Transaction lock not found.");

            if (tLock.ExpirationUtc <= DateTime.UtcNow && tLock.Conversion == null) {
                dbContext.AssetConversionLocks.Remove(tLock);
                await dbContext.SaveChangesAsync();

                return null;
            }

            return tLock;
        }

        public async Task<List<AssetConversionLock>> GetTransactionLocks(AppUser user)
        {
            var tLocks = await dbContext.AssetConversionLocks
                .Include(x => x.Left)
                .Include(x => x.Right)
                .Include(x => x.Conversion)
                .Where(x => x.UserId == user.Id)
                .ToListAsync();

            var toDelete = new List<AssetConversionLock>(tLocks.Count);
            foreach (var tLock in tLocks) {
                if (tLock.ExpirationUtc <= DateTime.UtcNow && tLock.Conversion == null) {
                    toDelete.Add(tLock);
                    tLocks.Remove(tLock);
                }
            }

            dbContext.AssetConversionLocks.RemoveRange(toDelete);
            await dbContext.SaveChangesAsync();

            return tLocks;
        }
    }
}
