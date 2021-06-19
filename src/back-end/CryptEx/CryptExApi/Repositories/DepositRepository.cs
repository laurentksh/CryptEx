using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Data;
using CryptExApi.Exceptions;
using CryptExApi.Models.Database;
using CryptExApi.Models.ViewModel.Payment;
using CryptExApi.Utilities;
using Microsoft.EntityFrameworkCore;

namespace CryptExApi.Repositories
{
    public interface IDepositRepository
    {
        Task<List<DepositViewModel>> GetDeposits(AppUser user);

        Task<List<DepositViewModel>> GetDeposits(Guid userId);

        Task<CryptoDepositViewModel> DepositCrypto(AppUser user, Guid walletId);
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

        public async Task<CryptoDepositViewModel> DepositCrypto(AppUser user, Guid walletId)
        {
            var wallet = await dbContext.Wallets.SingleOrDefaultAsync(x => x.Id == walletId);

            if (wallet == null || wallet.Type != WalletType.Crypto)
                throw new BadRequestException("Invalid wallet Id provided.");

            var deposit = await dbContext.CryptoDeposits.AddAsync(new CryptoDeposit
            {
                Amount = 0,
                Status = Models.PaymentStatus.NotProcessed,
                CreationDate = DateTime.UtcNow,
                TransactionId = StringUtilities.SecureRandom(32, StringUtilities.AllowedChars.AlphabetNumbers),
                WalletId = walletId,
                Wallet = wallet,
                UserId = user.Id,
                User = user
            });

            await dbContext.SaveChangesAsync();

            var result = CryptoDepositViewModel.FromCryptoDeposit(deposit.Entity);
            result.WalletAddress = StringUtilities.SecureRandom(32, StringUtilities.AllowedChars.AlphabetNumbers);

            return result;
        }
    }
}
