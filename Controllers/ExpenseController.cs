using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Authorization;
using ExpenseTracker.Data;


namespace ExpenseTracker.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpenseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /expenses
        public async Task<IActionResult> Index()
        {
            var expenses = await _context.Expenses.ToListAsync();

            // Get the current date and time
            var now = DateTime.Now;

            // Calculate the start and end of this week (Monday to Sunday)
            var currentDay = now.DayOfWeek;
            var daysToSubtract = (currentDay == DayOfWeek.Sunday) ? 6 : (int)currentDay - 1;
            var startOfWeek = now.AddDays(-daysToSubtract).Date; // Monday of this week
            var endOfWeek = startOfWeek.AddDays(7).AddTicks(-1); // End of Sunday

            // Calculate total expenses for this week
            var totalThisWeek = expenses
                .Where(e => e.Date >= startOfWeek && e.Date <= endOfWeek)
                .Sum(e => e.Amount);

            // Calculate total expenses for this month
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1); // Last day of the current month
            var totalThisMonth = expenses
                .Where(e => e.Date >= startOfMonth && e.Date <= endOfMonth)
                .Sum(e => e.Amount);

            // Calculate total expenses for this year
            var startOfYear = new DateTime(now.Year, 1, 1);
            var endOfYear = new DateTime(now.Year, 12, 31, 23, 59, 59); // Last moment of the year
            var totalThisYear = expenses
                .Where(e => e.Date >= startOfYear && e.Date <= endOfYear)
                .Sum(e => e.Amount);

            // Create the ViewModel
            var viewModel = new ExpenseSummary
            {
                Expenses = expenses,
                TotalThisWeek = totalThisWeek,
                TotalThisMonth = totalThisMonth,
                TotalThisYear = totalThisYear
            };

         return View(viewModel);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Amount,Date")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
             return View(expense);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Description, Amount, Date")] Expense expense)
        {
            if (id != expense.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Expenses.Any(e => e.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(expense);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses.FirstOrDefaultAsync(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
