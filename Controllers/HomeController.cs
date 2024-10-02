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

            // Calculate weekly, monthly, and yearly spent using correct Where clauses
            var startOfWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var startOfYear = new DateTime(DateTime.Now.Year, 1, 1);

            // weekly, monthly, and yearly spending
            var weeklySpent = _context.Expenses.Where(e => e.Date >= startOfWeek).Sum(e => e.Amount);

            var monthlySpent = _context.Expenses.Where(e => e.Date >= startOfMonth).Sum(e => e.Amount);

            var yearlySpent = _context.Expenses.Where(e => e.Date >= startOfYear).Sum(e => e.Amount);

            // Pass the results to the View using ViewBag
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
