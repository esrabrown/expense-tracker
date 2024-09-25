using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;




namespace ExpenseTracker.Data;

public class ExpenseDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Expense> Expenses { get; set; }

    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // I can customize the ASP.NET Identity model and override the defaults if needed!!!
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
