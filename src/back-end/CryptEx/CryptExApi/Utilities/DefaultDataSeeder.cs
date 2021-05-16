using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Data;
using CryptExApi.Models.Database;
using CryptExApi.Services;
using Microsoft.AspNetCore.Identity;

namespace CryptExApi.Utilities
{
    /// <summary>
    /// Default data required to be seeded in every context of the application.
    /// </summary>
    public static class DefaultDataSeeder
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetService(typeof(UserManager<AppUser>)) as UserManager<AppUser>;
            var roleManager = serviceProvider.GetService(typeof(RoleManager<AppRole>)) as RoleManager<AppRole>;

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
                    BirthDay = DateTime.UnixEpoch
                };

                var result = await userManager.CreateAsync(user, "Password123$");

                if (!result.Succeeded) {
                    throw new Exception("Could not create admin account.");
                }

                await userManager.AddToRolesAsync(user, new List<string>() { "user", "admin" });
            }
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
            var userManager = serviceProvider.GetService(typeof(UserManager<AppUser>)) as UserManager<AppUser>;
            var roleManager = serviceProvider.GetService(typeof(RoleManager<AppRole>)) as RoleManager<AppRole>;
            var authService = serviceProvider.GetService(typeof(IAuthService)) as IAuthService;

            if (await userManager.FindByEmailAsync("testaccount@cryptex-trade.tech") == null) {
                await authService.CreateUser(new Models.DTO.CreateUserDTO
                {
                    Email = "testaccount@cryptex-trade.tech",
                    FirstName = "Test",
                    LastName = "Account",
                    Password = "Password123$"
                });
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
            var userManager = serviceProvider.GetService(typeof(UserManager<AppUser>)) as UserManager<AppUser>;
            var roleManager = serviceProvider.GetService(typeof(RoleManager<AppRole>)) as RoleManager<AppRole>;
            
            //Add as necessary.
        }
    }
}
