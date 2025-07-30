// Data/YFIDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YouFinanceIt.Models; // Now includes ApplicationUser, Account, Transaction, Category, Budget

namespace YouFinanceIt.Data
{
    // Inherit from IdentityDbContext<ApplicationUser> to include ASP.NET Core Identity tables
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for your custom financial models.
        // These models are now defined in separate files under YouFinanceIt.Models
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Budget> Budgets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // IMPORTANT: Call the base method for IdentityDbContext to configure Identity tables
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision
            modelBuilder.Entity<Account>().Property(a => a.Balance).HasPrecision(18, 2);
            modelBuilder.Entity<Budget>().Property(b => b.Amount).HasPrecision(18, 2);
            modelBuilder.Entity<Transaction>().Property(t => t.Amount).HasPrecision(18, 2);

            // Configure relationships with ApplicationUser (Identity User)
            // One ApplicationUser can have many Accounts
            modelBuilder.Entity<Account>()
                .HasOne(a => a.User) // Navigation property in Account
                .WithMany(u => u.Accounts) // Navigation property in ApplicationUser
                .HasForeignKey(a => a.UserID) // Foreign key in Account
                .OnDelete(DeleteBehavior.Cascade); // Delete all accounts when a user is deleted

            // One ApplicationUser can have many Transactions
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User) // Navigation property in Transaction
                .WithMany(u => u.Transactions) // Navigation property in ApplicationUser
                .HasForeignKey(t => t.UserID) // Foreign key in Transaction
                .OnDelete(DeleteBehavior.Restrict); // No cascade on Transaction.User (handled by Account cascade)

            // One ApplicationUser can have many Budgets
            modelBuilder.Entity<Budget>()
                .HasOne(b => b.User) // Navigation property in Budget
                .WithMany(u => u.Budgets) // Navigation property in ApplicationUser
                .HasForeignKey(b => b.UserID) // Foreign key in Budget
                .OnDelete(DeleteBehavior.Cascade); // Delete all budgets when a user is deleted

            // One ApplicationUser can have many Categories (custom)
            modelBuilder.Entity<Category>()
                .HasOne(c => c.User) // Navigation property in Category
                .WithMany(u => u.Categories) // Navigation property in ApplicationUser
                .HasForeignKey(c => c.UserID) // Foreign key in Category
                .OnDelete(DeleteBehavior.Restrict); // No cascade

            // Existing relationships (between your custom models)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountID)
                .OnDelete(DeleteBehavior.Restrict);

            // CORRECTED: 'c' was out of scope here. It should be 't.CategoryID'.
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryID) // Corrected from c.CategoryID
                .OnDelete(DeleteBehavior.Restrict);

            // CORRECTED: 'c' was out of scope here. It should be 'b.CategoryID'.
            modelBuilder.Entity<Budget>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Budgets)
                .HasForeignKey(b => b.CategoryID) // Corrected from c.CategoryID
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
