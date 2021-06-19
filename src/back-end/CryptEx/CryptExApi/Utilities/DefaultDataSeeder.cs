using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CryptExApi.Data;
using CryptExApi.Exceptions;
using CryptExApi.Models.Database;
using CryptExApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CryptExApi.Utilities
{
    /// <summary>
    /// Default data required to be seeded in every context of the application.
    /// </summary>
    public static class DefaultDataSeeder
    {
        public static readonly List<(string leftTicker, string rightTicker, decimal exchangeRate)> FiatExchangeRates = new()
        {
            //As of 16.06.2021 23:21
            ("USD", "USD", 1m),
            ("USD", "CHF", 0.91m),
            ("USD", "EUR", 0.83m),
            ("USD", "GBP", 0.71m),
            ("USD", "CAD", 1.23m),
            ("USD", "AUD", 1.31m),
            ("USD", "JPY", 110.66m),

            ("CHF", "CHF", 1m),
            ("CHF", "USD", 1.10m),
            ("CHF", "EUR", 0.92m),
            ("CHF", "GBP", 0.79m),
            ("CHF", "CAD", 1.35m),
            ("CHF", "AUD", 1.45m),
            ("CHF", "JPY", 121.78m),

            ("EUR", "EUR", 1m),
            ("EUR", "USD", 1.20m),
            ("EUR", "CHF", 1.09m),
            ("EUR", "GBP", 0.86m),
            ("EUR", "CAD", 1.47m),
            ("EUR", "AUD", 1.58m),
            ("EUR", "JPY", 132.75m),

            ("GBP", "GBP", 1m),
            ("GBP", "USD", 1.40m),
            ("GBP", "CHF", 1.27m),
            ("GBP", "EUR", 1.17m),
            ("GBP", "CAD", 1.72m),
            ("GBP", "AUD", 1.84m),
            ("GBP", "JPY", 154.85m),

            ("CAD", "CAD", 1m),
            ("CAD", "USD", 0.81m),
            ("CAD", "CHF", 0.74m),
            ("CAD", "EUR", 0.68m),
            ("CAD", "GBP", 0.58m),
            ("CAD", "AUD", 1.07m),
            ("CAD", "JPY", 90.14m),

            ("AUD", "AUD", 1m),
            ("AUD", "USD", 0.76m),
            ("AUD", "CHF", 0.69m),
            ("AUD", "EUR", 0.63m),
            ("AUD", "GBP", 0.54m),
            ("AUD", "CAD", 0.93m),
            ("AUD", "JPY", 84.25m),

            ("JPY", "JPY", 1m),
            ("JPY", "USD", 0.0090m),
            ("JPY", "CHF", 0.0082m),
            ("JPY", "EUR", 0.0075m),
            ("JPY", "GBP", 0.0065m),
            ("JPY", "CAD", 0.011m),
            ("JPY", "AUD", 0.012m),
        };

        public static readonly List<(string ticker, string fullName)> Fiats = new()
        {
            ("USD", "US Dollar"),
            ("CHF", "Swiss Franc"),
            ("EUR", "Euro"),
            ("GBP", "British Pound"),
            ("CAD", "Canadian Dollar"),
            ("AUD", "Australian Dollar"),
            ("JPY", "Japan Yen")
        };

        public static readonly List<(string ticker, string fullName)> Cryptos = new()
        {
            ("BTC", "Bitcoin"),
            ("ETH", "Ethereum"),
            ("DAI", "Dai"),
            ("USDC", "USD Coin"),
            ("ADA", "Cardano"),
            //("DOT",   "Polkadot"),
            //("XRP",   "Ripple"),
            //("SOL",   "Solana"),
            ("LTC", "Litecoin"),
            //("VET",   "VeChain"),
            //("USDT",  "Tether"),
            ("ENJ", "Enjin"),
            ("COMP", "Compound"),
            ("AAVE", "Aave"),
            //("BNB",   "Binance Coin"),
            ("MATIC", "Polygon"),
            ("UNI", "Uniswap"),
            //("PAXG",  "PAX Gold"),
            ("DOGE",  "Dogecoin"), // Update: DOGE is now listed on Coinbase (10.06.2021) !!!
            ("MKR", "Maker"),
            ("XTZ", "Tezos"),
            //("CHZ",   "Chiliz"),
            ("GRT", "The Graph"),
            ("LINK", "Chainlink"),
            //("XMR",   "Monero")
        };

        private static readonly List<string> Countries = new()
        {
            "AF",
            "AL",
            "DZ",
            "AS",
            "AD",
            "AO",
            "AI",
            "AQ",
            "AG",
            "AR",
            "AM",
            "AW",
            "AU",
            "AT",
            "AZ",
            "BS",
            "BH",
            "BD",
            "BB",
            "BY",
            "BE",
            "BZ",
            "BJ",
            "BM",
            "BT",
            "BO",
            "BQ",
            "BA",
            "BW",
            "BV",
            "BR",
            "IO",
            "BN",
            "BG",
            "BF",
            "BI",
            "CV",
            "KH",
            "CM",
            "CA",
            "KY",
            "CF",
            "TD",
            "CL",
            "CN",
            "CX",
            "CC",
            "CO",
            "KM",
            "CD",
            "CG",
            "CK",
            "CR",
            "HR",
            "CU",
            "CW",
            "CY",
            "CZ",
            "CI",
            "DK",
            "DJ",
            "DM",
            "DO",
            "EC",
            "EG",
            "SV",
            "GQ",
            "ER",
            "EE",
            "SZ",
            "ET",
            "FK",
            "FO",
            "FJ",
            "FI",
            "FR",
            "GF",
            "PF",
            "TF",
            "GA",
            "GM",
            "GE",
            "DE",
            "GH",
            "GI",
            "GR",
            "GL",
            "GD",
            "GP",
            "GU",
            "GT",
            "GG",
            "GN",
            "GW",
            "GY",
            "HT",
            "HM",
            "VA",
            "HN",
            "HK",
            "HU",
            "IS",
            "IN",
            "ID",
            "IR",
            "IQ",
            "IE",
            "IM",
            "IL",
            "IT",
            "JM",
            "JP",
            "JE",
            "JO",
            "KZ",
            "KE",
            "KI",
            "KP",
            "KR",
            "KW",
            "KG",
            "LA",
            "LV",
            "LB",
            "LS",
            "LR",
            "LY",
            "LI",
            "LT",
            "LU",
            "MO",
            "MG",
            "MW",
            "MY",
            "MV",
            "ML",
            "MT",
            "MH",
            "MQ",
            "MR",
            "MU",
            "YT",
            "MX",
            "FM",
            "MD",
            "MC",
            "MN",
            "ME",
            "MS",
            "MA",
            "MZ",
            "MM",
            "NA",
            "NR",
            "NP",
            "NL",
            "NC",
            "NZ",
            "NI",
            "NE",
            "NG",
            "NU",
            "NF",
            "MP",
            "NO",
            "OM",
            "PK",
            "PW",
            "PS",
            "PA",
            "PG",
            "PY",
            "PE",
            "PH",
            "PN",
            "PL",
            "PT",
            "PR",
            "QA",
            "MK",
            "RO",
            "RU",
            "RW",
            "RE",
            "BL",
            "SH",
            "KN",
            "LC",
            "MF",
            "PM",
            "VC",
            "WS",
            "SM",
            "ST",
            "SA",
            "SN",
            "RS",
            "SC",
            "SL",
            "SG",
            "SX",
            "SK",
            "SI",
            "SB",
            "SO",
            "ZA",
            "GS",
            "SS",
            "ES",
            "LK",
            "SD",
            "SR",
            "SJ",
            "SE",
            "CH",
            "SY",
            "TW",
            "TJ",
            "TZ",
            "TH",
            "TL",
            "TG",
            "TK",
            "TO",
            "TT",
            "TN",
            "TR",
            "TM",
            "TC",
            "TV",
            "UG",
            "UA",
            "AE",
            "GB",
            "UM",
            "US",
            "UY",
            "UZ",
            "VU",
            "VE",
            "VN",
            "VG",
            "VI",
            "WF",
            "EH",
            "YE",
            "ZM",
            "ZW",
            "AX"
        };

        public static async Task Seed(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetService<CryptExDbContext>();
            var userManager = serviceProvider.GetService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetService<RoleManager<AppRole>>();

            if (!await roleManager.RoleExistsAsync("user")) {
                await roleManager.CreateAsync(new AppRole("user"));
            }

            if (!await roleManager.RoleExistsAsync("admin")) {
                await roleManager.CreateAsync(new AppRole("admin"));
            }

            // Admin account
            if (await userManager.FindByEmailAsync("admin@cryptex-trade.tech") == null) {
                var user = new AppUser
                {
                    Email = "admin@cryptex-trade.tech",
                    UserName = "admin",
                    FirstName = "CryptEx",
                    LastName = "Admin",
                    BirthDay = DateTime.UnixEpoch,
                    PreferedCurrency = "CHF",
                    PreferedLanguage = "en-us",
                    CreationDate = DateTime.UnixEpoch
                };

                var result = await userManager.CreateAsync(user, "Password123$");

                if (!result.Succeeded) {
                    throw new IdentityException("Could not create admin account.");
                }

                await userManager.AddToRolesAsync(user, new List<string>() { "user", "admin" });
                await userManager.AddClaimsAsync(user, new List<Claim> { new Claim("premium", true.ToString()) });
            }

            // Fiats
            foreach (var (ticker, fullName) in Fiats) {
                if (!dbContext.Wallets.Any(x => x.Ticker == ticker)) {
                    await dbContext.Wallets.AddAsync(new Wallet
                    {
                        Ticker = ticker,
                        FullName = fullName,
                        Type = WalletType.Fiat
                    });
                }
            }

            // Cryptos
            foreach (var (ticker, fullName) in Cryptos) {
                if (!dbContext.Wallets.Any(x => x.Ticker == ticker)) {
                    await dbContext.Wallets.AddAsync(new Wallet
                    {
                        Ticker = ticker,
                        FullName = fullName,
                        Type = WalletType.Crypto
                    });
                }
            }

            // Countries
            foreach (var countryCode in Countries) {
                if (!dbContext.Countries.Any(x => x.Iso31661Alpha2Code == countryCode)) {
                    await dbContext.Countries.AddAsync(new Country
                    {
                        Iso31661Alpha2Code = countryCode
                    });
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }

    public interface IDataSeeder
    {
        Task Seed(IServiceProvider serviceProvider);
    }

    /// <summary>
    /// Default data when testing the application.
    /// </summary>
    public class DevelopmentDataSeeder : IDataSeeder
    {
        public async Task Seed(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetService<CryptExDbContext>();
            var userManager = serviceProvider.GetService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetService<RoleManager<AppRole>>();
            var authService = serviceProvider.GetService<IAuthService>();

            if (await userManager.FindByEmailAsync("testaccount@cryptex-trade.tech") == null) {
                await authService.CreateUser(new Models.DTO.CreateUserDTO
                {
                    Email = "testaccount@cryptex-trade.tech",
                    FirstName = "Test",
                    LastName = "Account",
                    Password = "Password123$"
                });

                var user = await userManager.FindByEmailAsync("testaccount@cryptex-trade.tech");
                var wallet = await dbContext.Wallets.SingleAsync(x => x.Ticker == "CHF");
                var btc = await dbContext.Wallets.SingleAsync(x => x.Ticker == "BTC");

                await dbContext.FiatDeposits.AddAsync(new FiatDeposit
                {
                    Amount = 20000000,
                    Status = Models.PaymentStatus.Success,
                    StripeSessionId = "TESTSESSIONID",
                    CreationDate = DateTime.UtcNow.AddDays(-3),
                    UserId = user.Id,
                    WalletId = wallet.Id
                });

                var result = await dbContext.AssetConversionLocks.AddAsync(new AssetConversionLock
                {
                    ExchangeRate = 0.000031m,
                    LeftId = wallet.Id,
                    RightId = btc.Id,
                    UserId = user.Id,
                    ExpirationUtc = DateTime.UtcNow.AddDays(-3),
                });

                await dbContext.SaveChangesAsync();

                await dbContext.AssetConversions.AddAsync(new AssetConversion
                {
                    Amount = 1000000,
                    PriceLockId = result.Entity.Id,
                    UserId = user.Id,
                    Status = Models.PaymentStatus.Success
                });

                await dbContext.SaveChangesAsync();
            }
        }
    }

    /// <summary>
    /// Default data when in production.
    /// </summary>
    public class ProductionDataSeeder : IDataSeeder
    {
        public async Task Seed(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetService<RoleManager<AppRole>>();

            //Add as necessary.
        }
    }
}
