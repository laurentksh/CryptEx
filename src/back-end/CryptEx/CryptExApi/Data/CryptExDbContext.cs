using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CryptExApi.Data
{
    public class CryptExDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public DbSet<FiatDeposit> Deposits { get; set; }

        public CryptExDbContext(DbContextOptions<CryptExDbContext> options) : base(options)
        {
            
        }
    }
}
