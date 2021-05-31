using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CryptExApi.Models.Database;
using CryptExApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CryptExApi.Extensions
{
    /// <summary>
    /// Method extensions for auth.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Returns whether the Request is authenticated or not.
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>true: Authenticated; false: Not Authenticated</returns>
        public static bool IsAuthenticated(HttpContext context) => context.User.Identity.IsAuthenticated;

        /// <summary>
        /// Get the user from HttpContext.
        /// </summary>
        /// <remarks>
        /// WARNING: The returned AppUser object does not contains foreign properties.
        /// </remarks>
        /// <param name="context">Request's HttpContext</param>
        /// <returns>AppUser or null</returns>
        public async static Task<AppUser> GetUser(this HttpContext context)
        {
            var userManager = context.RequestServices.GetService(typeof(UserManager<AppUser>)) as UserManager<AppUser>;

            var claim = context.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (claim == null)
                return null;

            var userId = claim.Value;

            if (userId == null)
                return null;

            var user = await userManager.FindByIdAsync(userId);

            return user;
        }

        /// <summary>
        /// Get the user's id from HttpContext.
        /// </summary>
        /// <param name="context">Request's HttpContext</param>
        /// <returns>User's id or <see cref="Guid.Empty"/></returns>
        public static Guid GetUserId(this HttpContext context)
        {
            if (IsAuthenticated(context)) {
                var id = context.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

                return Guid.Parse(id);
            } else {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Get the currency specified in the request's headers, or return the default currency.
        /// </summary>
        /// <remarks>
        /// This method also validates that the currency exists.
        /// </remarks>
        /// <param name="context">Request's HttpContext</param>
        /// <returns>Specified currency or default currency</returns>
        public static async Task<string> GetCurrencyFromHeadersOrDefault(this HttpContext context)
        {
            var walletsService = context.RequestServices.GetService<IWalletService>();

            if (!context.Request.Headers.TryGetValue("X-Currency", out var values))
                return walletsService.GetDefaultCurrency();

            var currency = values.First().ToUpper();

            if (!await walletsService.CurrencyExists(currency))
                return walletsService.GetDefaultCurrency();

            return currency;
        }
    }
}
