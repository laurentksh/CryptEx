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
        Task<AssetConversionLockViewModel> GetTransactionLock(AppUser user, Guid id);

        Task<AssetConversionLockViewModel> LockTransaction(AppUser user, AssetConvertionLockDto dto);

        Task RemoveTransactionLock(AppUser user, Guid id);

        Task<Guid> Convert(AppUser user, AssetConversionDto dto);

        Task<AssetConversionViewModel> GetTransaction(AppUser user, Guid transactionId);

        Task<List<AssetConversionViewModel>> GetTransactions(AppUser user);

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

        public async Task<AssetConversionLockViewModel> GetTransactionLock(AppUser user, Guid id)
        {
            var tLock = await repository.GetTransactionLock(id);

            if (tLock == null)
                throw new NotFoundException("Transaction lock not found.");

            if (tLock.UserId != user.Id)
                throw new ForbiddenException("You do not own this transaction lock.");

            return AssetConversionLockViewModel.FromConversionLock(tLock);
        }

        public async Task<AssetConversionLockViewModel> LockTransaction(AppUser user, AssetConvertionLockDto dto)
        {
            if (user.Status != AccountStatus.Active)
                throw new ForbiddenException("Your account has been disabled. Please contact our support");

            var wallets = await walletRepository.GetWallets();
            var left = wallets.SingleOrDefault(x => x.Id == dto.LeftAssetId);
            var right = wallets.SingleOrDefault(x => x.Id == dto.RightAssetId);

            if (left == null || right == null)
                throw new BadRequestException("Wallet does not exist.");
           
            var exchangeRate = 0m;
            if (left.Type == WalletType.Fiat && right.Type == WalletType.Fiat)
                exchangeRate = await walletRepository.GetFiatExchangeRate(left.Ticker, right.Ticker);
            else if (left.Type == WalletType.Crypto && right.Type == WalletType.Crypto)
                exchangeRate = await walletRepository.GetCryptoExchangeRate(left.Ticker, right.Ticker, noCache: true);
            else
                exchangeRate = await walletRepository.GetCryptoExchangeRate(left.Ticker, right.Ticker, noCache: true); //Coinbase handles the conversion

            return AssetConversionLockViewModel.FromConversionLock(await repository.LockTransaction(user, left, right, exchangeRate));
        }

        public async Task RemoveTransactionLock(AppUser user, Guid id)
        {
            await repository.RemoveTransactionLock(user, id);
        }

        public async Task<Guid> Convert(AppUser user, AssetConversionDto dto)
        {
            if (dto == null)
                throw new BadRequestException("Missing transaction lock");
            if (user.Status != AccountStatus.Active)
                throw new ForbiddenException("Your account has been disabled. Please contact our support");

            var transactionLock = await repository.GetTransactionLock(dto.TransactionLockId);

            if (transactionLock == null)
                throw new BadRequestException("Invalid transaction lock id.");

            if (transactionLock.UserId != user.Id)
                throw new ForbiddenException("You do not own this transaction lock.");

            var left = transactionLock.Left;
            var right = transactionLock.Right;

            if (left == null || right == null)
                throw new Exception("Invalid transaction lock.");

            var available = await walletRepository.GetWalletAmount(user, left.Id);

            if (dto.Amount > available)
                throw new InsufficientFundsException($"You do not have enough {left.Ticker} funds. (Available: {available}, Requested: {dto.Amount})");

            return await repository.Convert(user, transactionLock, dto.Amount);
        }

        public async Task<AssetConversionViewModel> GetTransaction(AppUser user, Guid transactionId)
        {
            var transaction = await repository.GetTransaction(user, transactionId);

            if (transaction == null)
                throw new NotFoundException("Transaction not found.");

            return AssetConversionViewModel.FromAssetConversion(transaction);
        }

        public async Task<List<AssetConversionViewModel>> GetTransactions(AppUser user)
        {
            return (await repository.GetTransactions(user)).Select(x => AssetConversionViewModel.FromAssetConversion(x)).ToList();
        }

        public async Task<List<AssetConversionViewModel>> GetOngoingTransactions(Guid id)
        {
            return (await repository.GetOngoingTransactions(id)).Select(x => AssetConversionViewModel.FromAssetConversion(x)).ToList();
        }
    }
}
