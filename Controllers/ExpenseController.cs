// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using System;
// using System.Linq;
// using System.Threading.Tasks;
// using ExpenseTracker.Models;

// namespace ExpenseTracker.AddControllersWithViews
// {
//   public class ExpenseController : Controller
//   {
//      private readonly ExpenseDbContext _context;

//      public ExpenseController(ExpenseDbContext context)
//      {
//         _context = context;
//      }

//      public async Task<IActionResult> Index()
//      {
//          var expenses = await _context.Expenses.ToListAsync();
//          return View(expenses);
//      }


//     [HttpGet]
//      public IActionResult Create()
//      {
//          return View();
//      }


//     [HttpPost]
//     public async Task<IActionResult>  Create([Bind("Id, Description, Amount, Date")] Expense expense)
//     {
//       if (ModelState.IsValid)
//       {
//         _context.Add(expense);
//         await _context.SaveChangesAsync();
//         return RedirectToAction(nameof(Index));
//       }

//       return View(expense);
//     }


//     [HttpGet]
//     public async Task<IActionResult> Edit(int? id)
//     {
//        if (id == null)
//         {
//           return NotFound();
//         }

//          var expense = await _context.Expenses.FindAsync(id);
//          if (expense == null)
//          {
//             return NotFound();
//          }

//          return View(expense);
//         }

//    [HttpPost]
//    public async Task<IActionResult> Edit(int id, [Bind("Id, Description, Amount, Date")] Expense expense)
//    {
//       if (id != expense.Id)
//       {
//         return BadRequest(); //Return a 400 Bad Request if Ids don't match
//       }

//       if (ModelState.IsValid)
//       {
//         try
//         {
//           _context.Update(expense);
//           await _context.SaveChangesAsync();
//         }
//         catch (DbUpdateConcurrencyExpenction)
//         {
//           if (!_context.Expense.Any(expense => expense.Id == id))
//           {
//             return NotFound();  // Return a 404 Not Found if expense with the given ID is not found
//           }
//           else
//           {
//             throw; // Handle other exceptions
//           }
//         }
//         return RedirectToAction(nameof(Index));
//       }
//       return View(expense);
//    }


//     [HttpGet]
//      public async Task<IActionResult> Delete(int? id)
//      {
//          if (id == null)
//          {
//                 return NotFound();
//          }

//          var expense = await _context.Expenses.FirstOrDefaultAsync(m => m.Id == id);
//           if (expense == null)
//           {
//             return NotFound();
//           }

//           return View(expense);
//       }


//    [HttpPost, ActionName("Delete")]
//    public async Task<IActionResult> Delete(int id)
//    {
//     var expense = await _context.Expense.FindAsync(id);
//     if(expense == null)
//     {
//       return NotFound();   // Return a 404 Not Found if expense with the given ID is not found
//     }

//     _context.Expense.Remove(expense);
//     await _context.SaveChangesAsync();
//     return RedirectToAction(nameof(Index));
//     }
//    }



//   }

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
        public async Task<IActionResult> Create([Bind("Id, Description, Amount, Date")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                _context.Expenses.Add(expense);
                await _context.SaveChangesAsync();
                // _context.SaveChanges();
                return RedirectToAction("Index", "Home");
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
                return BadRequest(); // Return a 400 Bad Request if Ids don't match
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
                return NotFound(); // Return a 404 Not Found if expense with the given ID is not found
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
