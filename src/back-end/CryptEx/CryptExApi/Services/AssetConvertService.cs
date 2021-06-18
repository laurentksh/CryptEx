using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CryptExApi.Exceptions;
using CryptExApi.Models.Database;
using CryptExApi.Models.DTO;
using CryptExApi.Models.DTO.AssetConvert;
using CryptExApi.Models.ViewModel;
using CryptExApi.Models.ViewModel.AssetConvert;
using CryptExApi.Models.ViewModel.Payment;
using CryptExApi.Repositories;

namespace CryptExApi.Services
{
    public interface IAssetConvertService
    {
        Task<AssetConversionLockViewModel> LockTransaction(AssetConvertionLockDto dto);

        Task<Guid> Convert(AppUser user, AssetConversionDto dto);

        Task<AssetConversionViewModel> GetTransaction(AppUser user, Guid transactionId);
        Task<List<AssetConversionViewModel>> GetOngoingTransactions(Guid guid);
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

        public async Task<AssetConversionLockViewModel> LockTransaction(AssetConvertionLockDto dto)
        {
            var wallets = await walletRepository.GetWallets();
            var left = wallets.SingleOrDefault(x => x.Id == dto.LeftAssetId);
            var right = wallets.SingleOrDefault(x => x.Id == dto.RightAssetId);

            if (left == null || right == null)
                throw new BadRequestException("Wallet does not exist.");
           
            var exchangeRate = 0m;
            if (left.Type == WalletType.Fiat && right.Type == WalletType.Fiat)
                exchangeRate = await walletRepository.GetFiatExchangeRate(left.Ticker, right.Ticker);
            else if (left.Type == WalletType.Crypto && right.Type == WalletType.Crypto)
                exchangeRate = await walletRepository.GetCryptoExchangeRate(left.Ticker, right.Ticker);
            else
                exchangeRate = await walletRepository.GetCryptoExchangeRate(left.Ticker, right.Ticker); //Coinbase handles the conversion

            return AssetConversionLockViewModel.FromConversionLock(await repository.LockTransaction(left, right, exchangeRate));
        }

        public async Task<Guid> Convert(AppUser user, AssetConversionDto dto)
        {
            if (dto == null)
                throw new BadRequestException("Missing transaction lock");

            var transactionLock = await repository.GetTransactionLock(dto.TransactionLockId);

            var left = transactionLock.Left;
            var right = transactionLock.Right;

            if (left == null || right == null)
                throw new BadRequestException("Invalid transaction lock id.");

            var available = await walletRepository.GetWalletAmount(user, left.Id);

            if (dto.Amount > available)
                throw new InsufficientFundsException($"You do not have enough {left.Ticker} funds. (Available: {available}, Requested: {dto.Amount})");

            return await repository.Convert(user, transactionLock, dto.Amount);
        }

        public async Task<AssetConversionViewModel> GetTransaction(AppUser user, Guid transactionId)
        {
            return AssetConversionViewModel.FromAssetConversion(await repository.GetTransaction(user, transactionId));
        }

        public async Task<List<AssetConversionViewModel>> GetOngoingTransactions(Guid id)
        {
            return (await repository.GetOngoingTransactions(id)).Select(x => AssetConversionViewModel.FromAssetConversion(x)).ToList();
        }
    }
}
