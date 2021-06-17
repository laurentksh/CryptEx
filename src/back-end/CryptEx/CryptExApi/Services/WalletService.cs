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

        Task<TotalViewModel> GetTotal(AppUser user, WalletType type);

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

        public async Task<TotalViewModel> GetTotal(AppUser user, WalletType type)
        {
            return await walletsRepository.GetTotal(user, type);
        }

        public async Task<WalletViewModel> GetCryptoFull(Guid id, string currency)
        {
            return await walletsRepository.GetCryptoFull(id, currency ?? GetDefaultCurrency());
        }
    }
}
