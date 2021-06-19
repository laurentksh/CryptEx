using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coinbase;
using CryptExApi.Models.Database;
using CryptExApi.Models.ViewModel;
using CryptExApi.Models.ViewModel.Wallets;
using CryptExApi.Repositories;

namespace CryptExApi.Services
{
    public interface IWalletService
    {
        string GetDefaultCurrency();

        Task<bool> CurrencyExists(string currency);

        Task<bool> CryptoExists(string ticker);

        Task<List<WalletViewModel>> GetFiatWallets();

        Task<List<WalletViewModel>> GetCryptoWallets(string currency);

        Task<List<UserWalletViewModel>> GetFiatWallets(AppUser user);

        Task<List<UserWalletViewModel>> GetCryptoWallets(AppUser user);

        Task<TotalsViewModel> GetTotals(AppUser user);

        Task<WalletViewModel> GetCryptoFull(Guid id, string currency);
    }

    public class WalletService : IWalletService
    {
        private readonly IWalletRepository walletsRepository;

        public WalletService(IWalletRepository walletsRepository)
        {
            this.walletsRepository = walletsRepository;
        }

        public string GetDefaultCurrency() => "USD";

        public async Task<bool> CurrencyExists(string currency) => await walletsRepository.WalletExists(currency, WalletType.Fiat);

        public async Task<bool> CryptoExists(string ticker) => await walletsRepository.WalletExists(ticker, WalletType.Crypto);

        public async Task<List<WalletViewModel>> GetFiatWallets()
        {
            return (await walletsRepository.GetFiatWallets())
                .Select(x => WalletViewModel.FromWallet(x))
                .ToList();
        }

        public async Task<List<WalletViewModel>> GetCryptoWallets(string currency)
        {
            currency ??= GetDefaultCurrency(); //Defaults to USD

            var wallets = await walletsRepository.GetCryptoWallets();

            var result = new List<WalletViewModel>();
            foreach (var wallet in wallets) {
                result.Add(await walletsRepository.GetCryptoFull(wallet.Id, currency));
            }

            return result;
        }

        public async Task<List<UserWalletViewModel>> GetFiatWallets(AppUser user)
        {

            return await walletsRepository.GetFiatWallets(user);
        }

        public async Task<List<UserWalletViewModel>> GetCryptoWallets(AppUser user)
        {
            return await walletsRepository.GetCryptoWallets(user);
        }

        public async Task<TotalsViewModel> GetTotals(AppUser user)
        {
            var right = WalletViewModel.FromWallet(await walletsRepository.GetFiatFull(user.PreferedCurrency));
            var fiat = await walletsRepository.GetFiatWallets(user);
            var cryptos = await walletsRepository.GetCryptoWallets(user);
            var cryptosYtd = await walletsRepository.GetCryptoWallets(user, DateTime.UtcNow.AddHours(-24));

            var total = fiat.Sum(x => x.Amount) + cryptos.Sum(x => x.Amount * x.SelectedCurrencyPair.Rate);
            var fiatTotal = fiat.Sum(x => x.Amount);
            var cryptoTotal = cryptos.Sum(x => x.Amount * x.SelectedCurrencyPair.Rate);

            var fiatTotalYtd = fiatTotal; //We do not have a fiat exchange rate provider.
            var cryptoTotalYtd = cryptosYtd.Sum(x => x.Amount * x.SelectedCurrencyPair.Rate);
            var totalYtd = fiatTotalYtd + cryptoTotalYtd;

            var performance = cryptoTotal - cryptoTotalYtd;

            return new TotalsViewModel
            {
                AccountTotal = new TotalViewModel(total, right),
                FiatTotal = new TotalViewModel(fiatTotal, right),
                CryptoTotal = new TotalViewModel(cryptoTotal, right),

                AccountTotalYtd = new TotalViewModel(totalYtd, right),
                FiatTotalYtd = new TotalViewModel(fiatTotalYtd, right),
                CryptoTotalYtd = new TotalViewModel(cryptoTotalYtd, right),
                PerformanceYtd = new TotalViewModel(performance, right)
            };
        }

        public async Task<WalletViewModel> GetCryptoFull(Guid id, string currency)
        {
            return await walletsRepository.GetCryptoFull(id, currency ?? GetDefaultCurrency());
        }
    }
}
