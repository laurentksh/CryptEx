using CryptExApi.Data;
using CryptExApi.Models.Database;
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

        Task ChangeLanguage(AppUser user, string language);

        Task ChangeCurrency(AppUser user, string currency);
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

        private async Task<AppUser> GetFullUser(Guid id)
        {
            var user = await DbContext.Users
                .Include(x => x.BankAccounts)
                //.Include(x => x...)
                .SingleAsync(x => x.Id == id);

            return user;
        }
    }
}
