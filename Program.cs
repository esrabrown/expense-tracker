using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Define connection string and server version
var connectionString = "server=localhost;user=expensetracker;password=expensetracker123;database=expense_tracker";
var serverVersion = new MySqlServerVersion(new Version(8, 0, 32));

builder.Services.AddDbContext<ExpenseDbContext>(dbContextOptions =>
    dbContextOptions.UseMySql(connectionString, serverVersion));


builder.Services.AddDefaultIdentity<IdentityUser>
(options =>
{
   options.SignIn.RequireConfirmedAccount = true;
   options.Password.RequireDigit = false;
   options.Password.RequiredLength = 10;
   options.Password.RequireNonAlphanumeric = false;
   options.Password.RequireUppercase = true;
   options.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<ExpenseDbContext>();


// Add services to the container.
builder.Services.AddControllersWithViews();

// Use the defined connection string and server version in AddDbContext
builder.Services.AddDbContext<ExpenseDbContext>(dbContextOptions => dbContextOptions.UseMySql(connectionString, serverVersion));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
// builder.Services.AddRazorPages();
app.UseAuthentication();
app.MapRazorPages();
// app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
