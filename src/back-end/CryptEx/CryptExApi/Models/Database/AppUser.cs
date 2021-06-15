using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CryptExApi.Models.Database
{
    public class AppUser : IdentityUser<Guid>
    {
        public AppUser() : base()
        {
            
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDay { get; set; }

        public string PreferedLanguage { get; set; } = "en-us";

        public string PreferedCurrency { get; set; } = "usd";

        public DateTime CreationDate { get; set; }

        public BankAccount BankAccount { get; set; }

        public UserAddress Address { get; set; }
    }

    public class AppRole : IdentityRole<Guid>
    {
        public AppRole() : base()
        {

        }

        public AppRole(string roleName) : base(roleName)
        {

        }
    }
}
