using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using ExpenseTracker.Models;

var builder = WebApplication.CreateBuilder(args);

// Define connection string and server version
var connectionString = "server=localhost;user=expensetracker;password=expensetracker123;database=expense_tracker";
var serverVersion = new MySqlServerVersion(new Version(8, 0, 32));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Use the defined connection string and server version in AddDbContext
builder.Services.AddDbContext<ExpenseDbContext>(dbContextOptions => dbContextOptions.UseMySql(connectionString, serverVersion));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
