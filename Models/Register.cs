using System.ComponentModel.DataAnnotations;


namespace ExpenseTracker.Models
{
   public class Register
   {

    [Required]
    [StringLength(20, ErrorMessage = "The first name must be at least 3 and at most 20 characters long.", MinimumLength = 3)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(20, ErrorMessage = "The last name must be at least 3 and at most 20 characters long.", MinimumLength = 3)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set;}


   }

}