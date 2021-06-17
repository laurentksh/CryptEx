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
        Task<Guid> Convert(AppUser user, Wallet left, Wallet right, decimal amount, decimal exchangeRate);

        Task<AssetConversion> GetTransaction(AppUser user, Guid transactionId);
    }

    public class AssetConvertRepository : IAssetConvertRepository
    {
        private readonly CryptExDbContext dbContext;

        public AssetConvertRepository(CryptExDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Guid> Convert(AppUser user, Wallet left, Wallet right, decimal amount, decimal exchangeRate)
        {
            var result = await dbContext.AssetConversions.AddAsync(new AssetConversion
            {
                Amount = amount,
                Status = Models.PaymentStatus.NotProcessed,
                Left = left,
                LeftId = left.Id,
                Right = right,
                RightId = right.Id,
                ExchangeRate = exchangeRate,
                User = user,
                UserId = user.Id,
            });

            await dbContext.SaveChangesAsync();

            return result.Entity.Id;
        }

        public async Task<AssetConversion> GetTransaction(AppUser user, Guid transactionId)
        {
            return await dbContext.AssetConversions
                .Include(x => x.Left)
                .Include(x => x.Right)
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.UserId == user.Id && x.Id == transactionId);
        }
    }
}
