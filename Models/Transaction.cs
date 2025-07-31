using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YouFinanceIt.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }

        [Required]
        public string UserID { get; set; } = string.Empty; // Foreign key to ApplicationUser.Id

        [Required]
        public int AccountID { get; set; } // Foreign key to Account

        [Required]
        [StringLength(100)]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [StringLength(50)]
        public string? Type { get; set; } // e.g., "Deposit", "Withdrawal"

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ApplicationUser? User { get; set; }
        public Account? Account { get; set; }

        // public int? CategoryID { get; set; } // Removed as per request
        // public Category? Category { get; set; }
    }
}
