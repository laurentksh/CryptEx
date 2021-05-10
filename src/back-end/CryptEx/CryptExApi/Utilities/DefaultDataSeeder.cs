using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Data;
using CryptExApi.Models.Database;
using Microsoft.AspNetCore.Identity;

namespace CryptExApi.Utilities
{
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
        }
    }
}
