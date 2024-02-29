




namespace ExpenseTracker.AddControllersWithViews
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
         return Views(expenses);
     }

     public IActionResult Create()
     {
         return View();
     }



    public async Task<IActionResult>  Create([Bind("Id, Description, Amount, Date")] Expense expense)
    {
      if (ModelState.isValid)
      {
        _context.Add(expense);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }

      return AddControllersWithViews(expense);
    }



//gonna add edit, delete actions




  }
}