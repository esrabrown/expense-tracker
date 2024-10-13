using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;
using ExpenseTracker.Data;


namespace ExpenseTracker.Controllers;

public class HomeController : Controller
{

    private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
            _logger = logger;
            _context = context;
    }

   public IActionResult Index()
   {
        DateTime now = DateTime.Now;

        // Calculate the start of the week (Monday)
        var currentDay = now.DayOfWeek;
        var daysToSubtract = (currentDay == DayOfWeek.Sunday) ? 6 : (int)currentDay - 1;
        var startOfWeek = now.AddDays(-daysToSubtract).Date; // Monday of this week
        var endOfWeek = startOfWeek.AddDays(7).AddTicks(-1); // End of Sunday

        // Calculate the start of the month
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1); // Last moment of the month

        // Calculate the start of the year
        var startOfYear = new DateTime(now.Year, 1, 1);
        var endOfYear = new DateTime(now.Year, 12, 31, 23, 59, 59); // Last moment of the year

        // Calculate total spent in the week, month, and year
        var weeklySpent = _context.Expenses
            .Where(e => e.Date >= startOfWeek && e.Date <= endOfWeek) // Ensure correct date range
            .Sum(e => e.Amount);

        var monthlySpent = _context.Expenses
            .Where(e => e.Date >= startOfMonth && e.Date <= endOfMonth) // Ensure correct date range
            .Sum(e => e.Amount);

        var yearlySpent = _context.Expenses
            .Where(e => e.Date >= startOfYear && e.Date <= endOfYear) // Ensure correct date range
            .Sum(e => e.Amount);

        // Passing the results to the View using ViewBag
        ViewBag.WeeklySpent = weeklySpent;
        ViewBag.MonthlySpent = monthlySpent;
        ViewBag.YearlySpent = yearlySpent;

     return View();
    }


    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Create()
    {
    return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
