using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CryptExApi.Extensions
{
    public static class ClaimsExtensions
    {
        public static bool IsPremium(this IList<Claim> claims) => claims.Any(x => x.Type == "premium" && x.Value == bool.TrueString);
    }
}
