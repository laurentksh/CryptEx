using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CryptExApi.Models.Database
{
    public class AppUser : IdentityUser<Guid>
    {
        public AppUser()
        {
            
        }
    }

    public class AppRole : IdentityRole<Guid>
    {
        public AppRole()
        {

        }
    }
}
