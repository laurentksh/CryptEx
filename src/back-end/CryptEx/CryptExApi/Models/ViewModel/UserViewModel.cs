using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models.Database;

namespace CryptExApi.Models.ViewModel
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDay { get; set; }

        public string PreferedLanguage { get; set; }

        public string PreferedCurrency { get; set; }

        public static UserViewModel FromAppUser(AppUser user) => new()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDay = user.BirthDay,
            PreferedLanguage = user.PreferedLanguage,
            PreferedCurrency = user.PreferedCurrency
        };
    }
}
