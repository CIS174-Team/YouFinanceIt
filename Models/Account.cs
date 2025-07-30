// Models/Account.cs
using YouFinanceIt.Models; // For ApplicationUser and Transaction (if Transaction is in Models namespace)
using System.ComponentModel.DataAnnotations;

namespace YouFinanceIt.Models // Changed namespace
{
    public class Account
    {
        public int AccountID { get; set; }
        
        public string? UserID { get; set; } // Foreign key to ApplicationUser.Id (string)
        [Required]
        public string? AccountName { get; set; }
        [Required]
        public string? AccountType { get; set; }
        [Required]
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation property to ApplicationUser
        public ApplicationUser? User { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
    }
}
