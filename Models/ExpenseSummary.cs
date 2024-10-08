using System.Collections.Generic;

namespace ExpenseTracker.Models
{
    public class ExpenseSummary
    {
        public List<Expense> Expenses { get; set; } = new List<Expense>();
        public decimal TotalThisWeek { get; set; }
        public decimal TotalThisMonth { get; set; }
        public decimal TotalThisYear { get; set; }
    }
}
