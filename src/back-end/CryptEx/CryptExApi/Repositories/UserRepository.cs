using CryptExApi.Data;
using CryptExApi.Exceptions;
using CryptExApi.Models.Database;
using CryptExApi.Models.DTO;
using CryptExApi.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Repositories
{
    public interface IUserRepository
    {
        Task<AppUser> GetUser(Guid id);

        Task<AppUser> GetFullUser(Guid id);

        Task ChangeLanguage(AppUser user, string language);

        Task ChangeCurrency(AppUser user, string currency);

        Task<AddressViewModel> GetAddress(AppUser user);

        Task SetAddress(AppUser user, AddressDto dto);

        Task<BankAccountViewModel> GetIban(AppUser user);

        Task SetIban(AppUser user, IbanDto dto);
    }

    public class UserRepository : IUserRepository
    {
        public readonly CryptExDbContext DbContext;

        public UserRepository(CryptExDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<AppUser> GetUser(Guid id)
        {
            return await DbContext.Users.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AppUser> GetFullUser(Guid id)
        {
            var user = await DbContext.Users
                .Include(x => x.BankAccount)
                .Include(x => x.Address)
                .SingleAsync(x => x.Id == id);

            return user;
        }

        public async Task ChangeLanguage(AppUser user, string language)
        {
            user.PreferedLanguage = language;
            await DbContext.SaveChangesAsync();
        }

        public async Task ChangeCurrency(AppUser user, string currency)
        {
            user.PreferedCurrency = currency;
            await DbContext.SaveChangesAsync();
        }

        public async Task<AddressViewModel> GetAddress(AppUser user)
        {
            var address = await DbContext.UserAddresses
                .Include(x => x.Country)
                .SingleOrDefaultAsync(x => x.UserId == user.Id);

            return address != null ? AddressViewModel.FromAddress(address) : null;
        }

        public async Task SetAddress(AppUser user, AddressDto dto)
        {
            if (!await DbContext.Countries.AnyAsync(x => x.Id == dto.CountryId))
                throw new NotFoundException($"Country {dto.CountryId} does not exist.");
            
            var address = await DbContext.UserAddresses.SingleOrDefaultAsync(x => x.UserId == user.Id);

            if (address == null) {
                var newAddress = new UserAddress
                {
                    Street = dto.Street,
                    City = dto.City,
                    PostalCode = dto.PostalCode,
                    CreationDate = DateTime.UtcNow,
                    LastEditDate = DateTime.UtcNow,
                    CountryId = dto.CountryId,
                    UserId = user.Id,
                };

                await DbContext.UserAddresses.AddAsync(newAddress);
            } else {
                address.Street = dto.Street;
                address.City = dto.City;
                address.PostalCode = dto.PostalCode;
                address.LastEditDate = DateTime.UtcNow;
                address.CountryId = dto.CountryId;
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task<BankAccountViewModel> GetIban(AppUser user)
        {
            var bankAccount = await DbContext.BankAccounts.SingleOrDefaultAsync(x => x.UserId == user.Id);

            return bankAccount != null ? BankAccountViewModel.FromBankAccount(bankAccount) : null;
        }

        public async Task SetIban(AppUser user, IbanDto dto)
        {
            var bankAccount = await DbContext.BankAccounts.SingleOrDefaultAsync(x => x.UserId == user.Id);

            if (bankAccount == null) {
                var newBankAccount = new BankAccount
                {
                    Iban = dto.Iban.Replace(" ", ""),
                    Status = BankAccountStatus.NotProcessed,
                    UserId = user.Id,
                };

                await DbContext.BankAccounts.AddAsync(newBankAccount);
            } else {
                bankAccount.Iban = dto.Iban.Replace(" ", "");
                bankAccount.Status = BankAccountStatus.NotProcessed;
            }

            await DbContext.SaveChangesAsync();
        }
    }
}
