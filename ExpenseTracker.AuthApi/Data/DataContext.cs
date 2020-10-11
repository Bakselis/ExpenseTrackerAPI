using System;
using System.Collections.Generic;
using System.Text;
using ExpenseTracker.AuthApi.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.AuthApi.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    }
}