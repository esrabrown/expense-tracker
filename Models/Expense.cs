using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class Expense
    {
        public int Id { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        public decimal Amount { get; set; }


        // [Required(ErrorMessage = "Category is required.")]
        // public string Category { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }

}
