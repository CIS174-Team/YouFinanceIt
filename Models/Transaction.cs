// Models/Transaction.cs
using System.ComponentModel.DataAnnotations;
using YouFinanceIt.Models; // Now includes Account and ApplicationUser

namespace YouFinanceIt.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        [Required]
        public string UserID { get; set; } // Foreign key to ApplicationUser.Id (string)
        [Required]
        public int AccountID { get; set; } // Foreign key to Account
        [Required]
        public int CategoryID { get; set; } // Foreign key to Category

        [Required]
        [StringLength(100)]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [StringLength(50)]
        public string? Type { get; set; } // e.g., "Deposit", "Withdrawal", "Transfer"

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ApplicationUser? User { get; set; }
        public Account? Account { get; set; } // Changed from YouFinanceIt.Account.Account
        public Category? Category { get; set; }
    }
}
