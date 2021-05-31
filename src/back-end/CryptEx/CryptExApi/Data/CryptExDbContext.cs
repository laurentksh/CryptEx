using System;
using CryptExApi.Models.Database;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CryptExApi.Data
{
    public class CryptExDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public DbSet<FiatDeposit> FiatDeposits { get; set; }

        public DbSet<CryptoDeposit> CryptoDeposits { get; set; }

        public DbSet<FiatWithdrawal> FiatWithdrawals { get; set; }

        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<UserAddress> UserAddresses { get; set; }

        public DbSet<Country> Countries { get; set; }

        public CryptExDbContext(DbContextOptions<CryptExDbContext> options) : base(options)
        {
            
        }
    }
}
