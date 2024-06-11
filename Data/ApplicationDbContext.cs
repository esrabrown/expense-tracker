using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ExpenseTracker.Models;

namespace ExpenseTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {

        public DbSet<Expense> Expenses { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {

    //     base.OnModelCreating(modelBuilder);

    //     // modelBuilder.Entity<Expense>()
    //     // .HasOne(p => p.Description)
    //     // .IsRequired(false);

    //     modelBuilder.Entity<Expense>()
    //     .Property(p => p.Amount)
    //     .IsRequired();

    // }
    }
}
