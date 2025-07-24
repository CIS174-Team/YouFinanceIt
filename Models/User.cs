<<<<<<< HEAD
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
=======
// Models/User.cs
// This class extends IdentityUser for full authentication features.
using Microsoft.AspNetCore.Identity; // Required for IdentityUser
using System.ComponentModel.DataAnnotations;

namespace YouFinanceIt.Models
{
    // By inheriting from IdentityUser, you get properties like UserName, Email,
    // PhoneNumber, EmailConfirmed, PasswordHash (managed by Identity), etc.
    public class ApplicationUser : IdentityUser
    {
        // Add any custom properties specific to your application's user here.
        // For example, if you want to store a user's display name or registration date.
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Example: public string DisplayName { get; set; }
    }
}
>>>>>>> a0189e97b5bcab242b3a3b46957aa61e03b34c3b
