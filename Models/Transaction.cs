using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace YouFinanceIt.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }

        [Required]
        [BindNever] // Prevent binding UserID from form data
        public string UserID { get; set; }

        [Required]
        public int AccountID { get; set; }

        [Required]
        [StringLength(100)]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [StringLength(50)]
        public string? Type { get; set; } // e.g., "Income", "Expense"

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ApplicationUser? User { get; set; }
        public Account? Account { get; set; }
    }
}
