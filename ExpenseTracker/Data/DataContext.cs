using ExpenseTracker.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Expense> Expenses { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<ExpenseTag> ExpenseTags { get; set; }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<ExpenseTag>().Ignore(xx => xx.Expense).HasKey(x => new {x.ExpenseId, x.TagName});
        }
    }
}
