using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models.Database;

namespace CryptExApi.Models.ViewModel
{
    public class FullUserViewModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDay { get; set; }

        public string PreferedLanguage { get; set; }

        public string PreferedCurrency { get; set; }

        public DateTime CreationDate { get; set; }

        public BankAccountViewModel BankAccount { get; set; }

        public AddressViewModel Address { get; set; }

        public static FullUserViewModel FromAppUser(AppUser user) => new()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDay = user.BirthDay,
            PreferedLanguage = user.PreferedLanguage,
            PreferedCurrency = user.PreferedCurrency,
            CreationDate = user.CreationDate,
            BankAccount = user.BankAccount == null ? null : BankAccountViewModel.FromBankAccount(user.BankAccount),
            Address = user.Address == null ? null : AddressViewModel.FromAddress(user.Address)
        };
    }
}
