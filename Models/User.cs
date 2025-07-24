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