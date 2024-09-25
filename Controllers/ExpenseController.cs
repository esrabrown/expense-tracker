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
        // public async Task<IActionResult> Index()
        // {
        //     var expenses = await _context.Expenses.ToListAsync();
        //     return View(expenses);
        // }

        public async Task<IActionResult> Index()
        {
            var expenses = await _context.Expenses.ToListAsync();

            // Calculate total expenses for this week
            var startOfWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            var totalThisWeek = expenses.Where(e => e.Date >= startOfWeek).Sum(e => e.Amount);

            // Calculate total expenses for this month
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var totalThisMonth = expenses.Where(e => e.Date >= startOfMonth).Sum(e => e.Amount);

            // Calculate total expenses for this year
            var startOfYear = new DateTime(DateTime.Now.Year, 1, 1);
            var totalThisYear = expenses.Where(e => e.Date >= startOfYear).Sum(e => e.Amount);

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
        public async Task<IActionResult> Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                expense.Date = DateTime.Now;
                _context.Expenses.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Expense");
            }

            return View("Views/Expense/Index.cshtml");
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
