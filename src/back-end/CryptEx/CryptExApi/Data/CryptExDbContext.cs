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

        public DbSet<AssetConversion> AssetConversions { get; set; }

        public DbSet<AssetConversionLock> AssetConversionLocks { get; set; }

        public DbSet<FiatWithdrawal> FiatWithdrawals { get; set; }

        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<UserAddress> UserAddresses { get; set; }

        public DbSet<BankAccount> BankAccounts { get; set; }

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
                .HasPrecision(14, 2);

            builder.Entity<FiatWithdrawal>()
                .Property(x => x.Amount)
                .HasPrecision(14, 2);

            builder.Entity<CryptoDeposit>()
                .Property(x => x.Amount)
                .HasPrecision(14, 2);

            builder.Entity<AssetConversion>()
                .Property(x => x.Amount)
                .HasPrecision(14, 2);

            builder.Entity<AssetConversionLock>()
                .Property(x => x.ExchangeRate)
                .HasPrecision(20, 8);

            builder.Entity<AssetConversionLock>()
                .HasOne(x => x.Left)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AssetConversionLock>()
                .HasOne(x => x.User)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AssetConversionLock>()
                .HasOne(x => x.Right)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AssetConversion>()
                .HasOne(x => x.PriceLock)
                .WithOne(x => x.Conversion)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
