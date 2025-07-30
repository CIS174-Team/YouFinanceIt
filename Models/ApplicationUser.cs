// Models/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;
using YouFinanceIt.Models; // Now includes Account, Category, Budget, Transaction models

namespace YouFinanceIt.Models
{
    // Extend IdentityUser to add custom properties for your application's users
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties for the financial data associated with this user
        public ICollection<Account>? Accounts { get; set; } // Changed from YouFinanceIt.Account.Account
        public ICollection<Transaction>? Transactions { get; set; }
        public ICollection<Budget>? Budgets { get; set; }
        public ICollection<Category>? Categories { get; set; } // For custom categories
    }
}
