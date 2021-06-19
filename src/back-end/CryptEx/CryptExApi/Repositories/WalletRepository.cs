using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Coinbase;
using CryptExApi.Data;
using CryptExApi.Exceptions;
using CryptExApi.Models;
using CryptExApi.Models.Database;
using CryptExApi.Models.ViewModel;
using CryptExApi.Models.ViewModel.Payment;
using CryptExApi.Models.ViewModel.Wallets;
using CryptExApi.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

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

        Task<List<FiatDepositViewModel>> GetFiatDeposits(AppUser user, Guid walletId);

        Task<List<CryptoDepositViewModel>> GetCryptoDeposits(AppUser user, Guid walletId);

        /// <summary>
        /// Get the value of a wallet, with the user's preferred currency.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="walletId">Wallet</param>
        /// <returns>Wallet total value</returns>
        Task<decimal> GetWalletAmount(AppUser user, Guid walletId);

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
        Task<List<UserWalletViewModel>> GetCryptoWallets(AppUser user, DateTime? at = null);

        /// <summary>
        /// Get the total amount (of cryptos, fiats or both)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<TotalViewModel> GetTotal(AppUser user, WalletType type, DateTime? at = null);

        /// <summary>
        /// Get a fiat wallet.
        /// </summary>
        /// <param name="id">Unique id</param>
        /// <returns></returns>
        Task<Wallet> GetFiatFull(Guid id);

        /// <summary>
        /// Get a fiat wallet.
        /// </summary>
        /// <param name="ticker">Ticker</param>
        /// <returns></returns>
        Task<Wallet> GetFiatFull(string ticker);

        /// <summary>
        /// Get a cryptocurrency and it's exchange price (source: Coinbase)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        Task<WalletViewModel> GetCryptoFull(Guid id, string currency);

        /// <summary>
        /// Returns wheter an asset exists or not.
        /// </summary>
        /// <param name="ticker">Asset ticker (e.g: 'USD', 'BTC')</param>
        /// <param name="match">If the asset has to be fiat, crypto or both.</param>
        /// <returns></returns>
        Task<bool> WalletExists(string ticker, WalletType match);

        Task<decimal> GetFiatExchangeRate(string left, string right);

        /// <summary>
        /// Get the exchange price for an asset pair
        /// </summary>
        /// <param name="left">Left asset (e.g: USD)</param>
        /// <param name="right">Left asset (e.g: BTC)</param>
        /// <param name="at">(Optional) Specific date to get the price at</param>
        /// <param name="noCache">(Optional) Ignore cached values</param>
        /// <returns></returns>
        Task<decimal> GetCryptoExchangeRate(string left, string right, DateTime? at = null, bool noCache = false);
    }

    public class WalletRepository : IWalletRepository
    {
        private readonly CryptExDbContext dbContext;
        private readonly ICoinbaseClient coinbaseClient;
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<WalletRepository> logger;

        public WalletRepository(CryptExDbContext dbContext, ICoinbaseClient coinbaseClient, IMemoryCache memoryCache, ILogger<WalletRepository> logger)
        {
            this.dbContext = dbContext;
            this.coinbaseClient = coinbaseClient;
            this.memoryCache = memoryCache;
            this.logger = logger;
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
            var assetConversions = dbContext.AssetConversions
                .Include(x => x.User)
                .Include(x => x.PriceLock)
                .Where(x => x.UserId == user.Id);
            var fiatWithdrawals = dbContext.FiatWithdrawals
                .Include(x => x.User)
                .Include(x => x.Wallet)
                .Where(x => x.UserId == user.Id);

            //Add wallets to the results
            var wallets = await GetFiatWallets();
            var right = await GetFiatFull(user.PreferedCurrency);

            var tasks = wallets.Select(async x =>
            {
                return UserWalletViewModel.FromWallet(x, WalletPairViewModel.FromWalletPair(new WalletPair(x, right, await GetFiatExchangeRate(x.Ticker, user.PreferedCurrency))));
            });

            foreach (var task in tasks)
                result.Add(await task);
            
            foreach (var deposit in fiatDeposits) {
                if (deposit.Status != PaymentStatus.Success)
                    continue;

                var wallet = result.Single(x => x.Id == deposit.WalletId);
                wallet.Amount += deposit.Amount;
            }

            foreach (var conversion in assetConversions) {
                var decreaseWallet = result.SingleOrDefault(x => x.Id == conversion.PriceLock.LeftId);
                var increaseWallet = result.SingleOrDefault(x => x.Id == conversion.PriceLock.RightId);

                if (conversion.Status == PaymentStatus.Failed)
                    continue;

                if (decreaseWallet != null)
                    decreaseWallet.Amount -= conversion.Amount;
                if (increaseWallet != null)
                    increaseWallet.Amount += conversion.Amount * conversion.PriceLock.ExchangeRate;
            }

            foreach (var withdraw in fiatWithdrawals) {
                if (withdraw.Status == PaymentStatus.Failed)
                    continue;

                var wallet = result.Single(x => x.Id == withdraw.WalletId);
                wallet.Amount -= withdraw.Amount;
            }

            return result;
        }

        public async Task<List<UserWalletViewModel>> GetCryptoWallets(AppUser user, DateTime? at = null)
        {
            var result = new List<UserWalletViewModel>();
            var cryptoDeposits = dbContext.CryptoDeposits
                .Include(x => x.User)
                .Include(x => x.Wallet)
                .Where(x => x.UserId == user.Id);
            var assetConversions = dbContext.AssetConversions
                .Include(x => x.User)
                .Include(x => x.PriceLock)
                .Where(x => x.UserId == user.Id);

            //Add wallets to the results
            var wallets = await GetCryptoWallets();
            var right = await GetFiatFull(user.PreferedCurrency);

            var tasks = wallets.Select(async x =>
            {
                return UserWalletViewModel.FromWallet(x, WalletPairViewModel.FromWalletPair(new WalletPair(x, right, await GetCryptoExchangeRate(x.Ticker, right.Ticker, at))));
            });

            foreach (var task in tasks)
                result.Add(await task);

            foreach (var deposit in cryptoDeposits) {
                if (deposit.Status != PaymentStatus.Success)
                    continue;

                var wallet = result.Single(x => x.Id == deposit.WalletId);
                wallet.Amount += deposit.Amount;
            }

            foreach (var conversion in assetConversions) {
                var decreaseWallet = result.SingleOrDefault(x => x.Id == conversion.PriceLock.LeftId);
                var increaseWallet = result.SingleOrDefault(x => x.Id == conversion.PriceLock.RightId);

                if (conversion.Status == PaymentStatus.Failed)
                    continue;

                if (decreaseWallet != null)
                    decreaseWallet.Amount -= conversion.Amount;
                if (increaseWallet != null)
                    increaseWallet.Amount += conversion.Amount * conversion.PriceLock.ExchangeRate;
            }

            return result;
        }

        public async Task<List<FiatDepositViewModel>> GetFiatDeposits(AppUser user, Guid walletId)
        {
            var result = await dbContext.FiatDeposits
                .Include(x => x.User)
                .Include(x => x.Wallet)
                .Where(x => x.UserId == user.Id && x.WalletId == walletId)
                .Select(x => FiatDepositViewModel.FromFiatDeposit(x))
                .ToListAsync();

            return result;
        }

        public async Task<List<CryptoDepositViewModel>> GetCryptoDeposits(AppUser user, Guid walletId)
        {
            var result = await dbContext.CryptoDeposits
                .Include(x => x.User)
                .Include(x => x.Wallet)
                .Where(x => x.UserId == user.Id && x.WalletId == walletId)
                .Select(x => CryptoDepositViewModel.FromCryptoDeposit(x))
                .ToListAsync();

            return result;
        }

        public async Task<decimal> GetWalletAmount(AppUser user, Guid walletId)
        {
            var result = 0m;
            var fiatDeposits = await dbContext.FiatDeposits.Where(x => x.UserId == user.Id && x.WalletId == walletId).ToListAsync();
            var cryptoDeposits = await dbContext.CryptoDeposits.Where(x => x.UserId == user.Id && x.WalletId == walletId).ToListAsync();

            foreach (var deposit in fiatDeposits)
                result += deposit.Amount;

            foreach (var deposit in cryptoDeposits)
                result += deposit.Amount;

            return result;
        }

        public async Task<TotalViewModel> GetTotal(AppUser user, WalletType type, DateTime? at = null)
        {
            var selectedWallet = await GetFiatFull(user.PreferedCurrency);
            var wallets = new List<UserWalletViewModel>();
            decimal total = 0m;

            if (type == WalletType.Fiat || type == WalletType.Both)
                wallets.AddRange(await GetFiatWallets(user));
            if (type == WalletType.Crypto || type == WalletType.Both)
                wallets.AddRange(await GetCryptoWallets(user, at));

            foreach (var wallet in wallets) {
                total += wallet.Amount * wallet.SelectedCurrencyPair.Rate;
            }

            return new TotalViewModel
            {
                Amount = total,
                Reference = WalletViewModel.FromWallet(selectedWallet)
            };
        }

        public async Task<Wallet> GetFiatFull(Guid id) => dbContext.Wallets.Single(x => x.Id == id);

        public async Task<Wallet> GetFiatFull(string ticker) => dbContext.Wallets.Single(x => x.Ticker == ticker);

        public async Task<WalletViewModel> GetCryptoFull(Guid id, string currency)
        {
            var wallet = dbContext.Wallets.Single(x => x.Id == id);
            var fiat = await GetFiatFull(currency);
            var pair = new WalletPair(wallet, fiat, -1);

            // Get the exchange price
            pair.Rate = await GetCryptoExchangeRate(wallet.Ticker, fiat.Ticker);

            return WalletViewModel.FromWallet(wallet, WalletPairViewModel.FromWalletPair(pair));
        }

        public async Task<bool> WalletExists(string ticker, WalletType match)
        {
            return dbContext.Wallets.Any(x => x.Ticker == ticker && (match == WalletType.Both || x.Type == match));
        }

        public Task<decimal> GetFiatExchangeRate(string left, string right)
        {
            return Task.FromResult(DefaultDataSeeder.FiatExchangeRates.SingleOrDefault(x => x.leftTicker == left && x.rightTicker == right).exchangeRate);
        }

        public async Task<decimal> GetCryptoExchangeRate(string left, string right, DateTime? at = null, bool noCache = false)
        {
            var cacheKey = $"ExchangeRate.{left}-{right}@{at.GetValueOrDefault(DateTime.UtcNow).ToString("yyyy-MM-dd.HH", CultureInfo.InvariantCulture)}";

            bool cacheHit = memoryCache.TryGetValue(cacheKey, out decimal rate);
            if (cacheHit && !noCache)
                return rate;

            if (!cacheHit)
                logger.LogTrace($"Cache miss, querying exchange rate for {left}-{right}. Cache id: {cacheKey}");

            var pair = string.Empty;
            bool inverseRate = false;

            if (DefaultDataSeeder.Fiats.Any(x => x.ticker == left)) { //Coinbase endpoint currency pair format is {CRYPTO}-{FIAT}
                pair = right + "-" + left;
                inverseRate = true;
            } else {
                pair = left + "-" + right;
            }

            var resp = await coinbaseClient.Data.GetSpotPriceAsync(pair, at);
            
            if (resp.HasError())
                throw new CryptoApiException(resp.Errors);

            if (inverseRate)
                rate = Math.Round(1m / resp.Data.Amount, 8);
            else
                rate = resp.Data.Amount;

            memoryCache.Set(cacheKey, rate, TimeSpan.FromSeconds(60));
            
            return resp.Data.Amount;
        }
    }
}
