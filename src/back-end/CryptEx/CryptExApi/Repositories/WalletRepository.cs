using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coinbase;
using CryptExApi.Data;
using CryptExApi.Exceptions;
using CryptExApi.Models;
using CryptExApi.Models.Database;
using CryptExApi.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace CryptExApi.Repositories
{
    public interface IWalletRepository
    {
        /// <summary>
        /// Get all public wallets (Fiat and Crypto)
        /// </summary>
        /// <returns></returns>
        Task<List<Wallet>> GetWallets();

        /// <summary>
        /// Get all public Fiat wallets.
        /// </summary>
        /// <returns></returns>
        Task<List<Wallet>> GetFiatWallets();

        /// <summary>
        /// Get all public Crypto wallets.
        /// </summary>
        /// <returns></returns>
        Task<List<Wallet>> GetCryptoWallets();

        /// <summary>
        /// Get Fiat wallets and the user's data (amount, performance, etc).
        /// </summary>
        /// <param name="user">AppUser</param>
        /// <returns></returns>
        Task<List<UserWalletViewModel>> GetFiatWallets(AppUser user);

        /// <summary>
        /// Get Crypto wallets and the user's data (amount, performance, etc).
        /// </summary>
        /// <param name="user">AppUser</param>
        /// <returns></returns>
        Task<List<UserWalletViewModel>> GetCryptoWallets(AppUser user);

        Task<Wallet> GetFiatFull(Guid id);

        Task<Wallet> GetFiatFull(string ticker);

        Task<WalletViewModel> GetCryptoFull(Guid id, string currency);

        Task<bool> WalletExists(string ticker, WalletType match);
    }

    public class WalletRepository : IWalletRepository
    {
        private readonly CryptExDbContext dbContext;
        private readonly ICoinbaseClient coinbaseClient;

        public WalletRepository(CryptExDbContext dbContext, ICoinbaseClient coinbaseClient)
        {
            this.dbContext = dbContext;
            this.coinbaseClient = coinbaseClient;
        }

        public async Task<List<Wallet>> GetWallets()
        {
            return dbContext.Wallets
                .OrderBy(x => x.Type == WalletType.Fiat)
                .ToList();
        }

        public async Task<List<Wallet>> GetFiatWallets()
        {
            return (await GetWallets())
                .Where(x => x.Type == WalletType.Fiat)
                .ToList();
        }

        public async Task<List<Wallet>> GetCryptoWallets()
        {
            return (await GetWallets())
                .Where(x => x.Type == WalletType.Crypto)
                .ToList();
        }

        public async Task<List<UserWalletViewModel>> GetFiatWallets(AppUser user)
        {
            var result = new List<UserWalletViewModel>();
            var fiatDeposits = dbContext.FiatDeposits
                .Include(x => x.User)
                .Include(x => x.Wallet)
                .Where(x => x.UserId == user.Id);

            foreach (var deposit in fiatDeposits) {
                var wallet = result.Find(x => x.Id == deposit.WalletId);

                if (deposit.Status != PaymentStatus.Success)
                    continue;

                if (wallet != null) {
                    wallet.Amount += deposit.Amount;
                } else {
                    var dbWallet = dbContext.Wallets.Single(x => x.Id == deposit.WalletId);
                    wallet = WalletViewModel.FromWallet(dbWallet) as UserWalletViewModel;
                    wallet.Amount += deposit.Amount;

                    result.Add(wallet);
                }
            }

            //TODO: Substract withdrawals

            return result;
        }

        public async Task<List<UserWalletViewModel>> GetCryptoWallets(AppUser user)
        {
            var result = new List<UserWalletViewModel>();
            var cryptoDeposits = dbContext.CryptoDeposits
                .Include(x => x.User)
                .Include(x => x.Wallet)
                .Where(x => x.UserId == user.Id);

            foreach (var deposit in cryptoDeposits) {
                var wallet = result.Find(x => x.Id == deposit.WalletId);

                if (deposit.Status != PaymentStatus.Success)
                    continue;

                if (wallet != null) {
                    wallet.Amount += deposit.Amount;
                } else {
                    var dbWallet = dbContext.Wallets.Single(x => x.Id == deposit.WalletId);
                    wallet = WalletViewModel.FromWallet(dbWallet) as UserWalletViewModel;
                    wallet.Amount += deposit.Amount;

                    result.Add(wallet);
                }
            }

            //TODO: Substract withdrawals/transfers

            return result;
        }

        public async Task<Wallet> GetFiatFull(Guid id) => dbContext.Wallets.Single(x => x.Id == id);

        public async Task<Wallet> GetFiatFull(string ticker) => dbContext.Wallets.Single(x => x.Ticker == ticker);

        public async Task<WalletViewModel> GetCryptoFull(Guid id, string currency)
        {
            var wallet = dbContext.Wallets.Single(x => x.Id == id);
            var fiat = await GetFiatFull(currency);
            var pair = new WalletPair(wallet, fiat, -1);

            // Get the exchange price
            var resp = await coinbaseClient.Data.GetBuyPriceAsync(pair.ToCoinbaseString());

            if (resp.HasError())
                throw new CryptoApiException(resp.Errors);

            pair.Rate = resp.Data.Amount;

            return WalletViewModel.FromWallet(wallet, WalletPairViewModel.FromWalletPair(pair));
        }

        public async Task<bool> WalletExists(string ticker, WalletType match)
        {
            return dbContext.Wallets.Any(x => x.Ticker == ticker && (match == WalletType.Both || x.Type == match));
        }
    }
}
