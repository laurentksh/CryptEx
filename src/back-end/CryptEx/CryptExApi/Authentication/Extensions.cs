using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CryptExApi.Models.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CryptExApi.Authentication
{
    /// <summary>
    /// Method extensions for auth.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Get the user from HttpContext.
        /// </summary>
        /// <param name="context">Request's HttpContext</param>
        /// <returns>AppUser or null</returns>
        public async static Task<AppUser> GetUser(this HttpContext context)
        {
            var userManager = context.RequestServices.GetService(typeof(UserManager<AppUser>)) as UserManager<AppUser>;

            var user = await userManager.GetUserAsync(context.User);

            return user;
        }

        /// <summary>
        /// Get the user's id from HttpContext.
        /// </summary>
        /// <param name="context">Request's HttpContext</param>
        /// <returns>User's id or <see cref="Guid.Empty"/></returns>
        public static Guid GetUserId(this HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated) {
                var id = context.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                return Guid.Parse(id);
            } else {
                return Guid.Empty;
            }
        }
    }
}
