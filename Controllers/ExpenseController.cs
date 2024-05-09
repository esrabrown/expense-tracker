using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using ExpenseTracker.Models;

namespace ExpenseTracker.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ExpenseDbContext _context;


        public ExpenseController(ExpenseDbContext context)
        {
            _context = context;
        }

        // GET: /expenses
        public async Task<IActionResult> Index()
        {
            var expenses = await _context.Expenses.ToListAsync();
            return View(expenses);
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
                        return NotFound(); // Return a 404 Not Found if expense with the given ID is not found
                    }
                    else
                    {
                        throw; // Handle other exceptions
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
