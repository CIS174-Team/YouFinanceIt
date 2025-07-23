using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace YouFinanceIt.Models
{
    public class User : IdentityUser<int>
    {
        // do not re-declare UserID, Email, or PasswordHash — they are inherited from IdentityUser<int>

        public DateTime CreatedDate { get; set; }

        public ICollection<Account> Accounts { get; set; } = new List<Account>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
