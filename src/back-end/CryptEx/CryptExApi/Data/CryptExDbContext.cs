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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FiatWithdrawal>()
                .HasOne(x => x.BankAccount)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FiatDeposit>()
                .Property(x => x.Amount)
                .HasPrecision(9, 2);

            builder.Entity<FiatWithdrawal>()
                .Property(x => x.Amount)
                .HasPrecision(9, 2);

            builder.Entity<CryptoDeposit>()
                .Property(x => x.Amount)
                .HasPrecision(9, 2);
        }
    }
}
