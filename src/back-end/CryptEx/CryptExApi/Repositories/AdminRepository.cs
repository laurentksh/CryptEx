using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Data;
using CryptExApi.Models;
using CryptExApi.Models.Database;
using CryptExApi.Models.ViewModel.Payment;
using Microsoft.EntityFrameworkCore;

namespace CryptExApi.Repositories
{
    public interface IAdminRepository
    {
        Task<List<DepositViewModel>> GetAllDeposits(Guid? userId, PaymentStatus? status = null, WalletType type = WalletType.Fiat);
    }
    public class AdminRepository : IAdminRepository
    {
        private readonly CryptExDbContext dbContext;

        public AdminRepository(CryptExDbContext dbContext)
        {
            this.dbContext = dbContext;
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
    }
}
