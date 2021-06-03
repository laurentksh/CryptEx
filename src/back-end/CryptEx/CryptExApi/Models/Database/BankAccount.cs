﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.Database
{
    public class BankAccount
    {
        public Guid Id { get; set; }

        public string Iban { get; set; }

        public BankAccountStatus Status { get; set; }

        public Guid UserId { get; set; }

        public AppUser User { get; set; }
    }

    public enum BankAccountStatus
    {
        NotProcessed = -1,
        Approved = 1,
        Refused = 0
    }
}
