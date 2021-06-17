using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Exceptions;
using CryptExApi.Models.Database;
using CryptExApi.Models.DTO;
using CryptExApi.Models.ViewModel;
using CryptExApi.Models.ViewModel.Payment;
using CryptExApi.Repositories;

namespace CryptExApi.Services
{
    public interface IAssetConvertService
    {
        Task<Guid> Convert(AppUser user, AssetConvertDto dto);

        Task<AssetConversionViewModel> GetTransaction(AppUser user, Guid transactionId);
    }

    public class AssetConvertService : IAssetConvertService
    {
        private readonly IAssetConvertRepository repository;
        private readonly IWalletRepository walletRepository;

        public AssetConvertService(IAssetConvertRepository repository, IWalletRepository walletRepository)
        {
            this.repository = repository;
            this.walletRepository = walletRepository;
        }

        public async Task<Guid> Convert(AppUser user, AssetConvertDto dto)
        {
            var wallets = await walletRepository.GetWallets();
            var left = wallets.SingleOrDefault(x => x.Id == dto.LeftAssetId);
            var right = wallets.SingleOrDefault(x => x.Id == dto.RightAssetId);

            if (left == null || right == null)
                throw new BadRequestException("Wallet does not exist.");
            var available = await walletRepository.GetWalletAmount(user, left.Id);

            if (dto.Amount > available)
                throw new InsufficientFundsException($"You do not have enough {left.Ticker} funds. (Available: {available}, Requested: {dto.Amount})");

            var exchangeRate = 0m;
            if (left.Type == WalletType.Fiat && right.Type == WalletType.Fiat)
                exchangeRate = await walletRepository.GetFiatExchangeRate(left.Ticker, right.Ticker);
            else if (left.Type == WalletType.Crypto && right.Type == WalletType.Crypto)
                exchangeRate = await walletRepository.GetCryptoExchangeRate(left.Ticker, right.Ticker);
            else
                exchangeRate = await walletRepository.GetCryptoExchangeRate(left.Ticker, right.Ticker); //Coinbase handles the conversion

            return await repository.Convert(user, left, right, dto.Amount, exchangeRate);
        }

        public async Task<AssetConversionViewModel> GetTransaction(AppUser user, Guid transactionId)
        {
            return AssetConversionViewModel.FromAssetConversion(await repository.GetTransaction(user, transactionId));
        }
    }
}
