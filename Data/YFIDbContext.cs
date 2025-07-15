using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace YouFinanceIt.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Budget> Budgets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                    // Decimal precision
            modelBuilder.Entity<Account>().Property(a => a.Balance).HasPrecision(18, 2);
            modelBuilder.Entity<Budget>().Property(b => b.Amount).HasPrecision(18, 2);
            modelBuilder.Entity<Transaction>().Property(t => t.Amount).HasPrecision(18, 2);

            // Fix all cascade delete paths to avoid multiple cascade path error

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserID)
                .OnDelete(DeleteBehavior.Restrict); // No cascade

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountID)
                .OnDelete(DeleteBehavior.Restrict); //No cascade

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryID)
                .OnDelete(DeleteBehavior.Restrict); //No cascade

            modelBuilder.Entity<Account>()
                .HasOne(a => a.User)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.UserID)
                .OnDelete(DeleteBehavior.Cascade); //Delete all accounts when a user is deleted

            modelBuilder.Entity<Budget>()
                .HasOne(b => b.User)
                .WithMany(u => u.Budgets)
                .HasForeignKey(b => b.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Budget>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Budgets)
                .HasForeignKey(b => b.CategoryID)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Category>()
                .HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }

    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<Account> Accounts { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Budget> Budgets { get; set; }
        public ICollection<Category> Categories { get; set; } // for custom categories
    }

    public class Account
    {
        public int AccountID { get; set; }
        public int UserID { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }

        public User User { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }

    public class Transaction
    {
        public int TransactionID { get; set; }
        public int UserID { get; set; }
        public int AccountID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }

        public User User { get; set; }
        public Account Account { get; set; }
        public Category Category { get; set; }
    }

    public class Category
    {
        public int CategoryID { get; set; }
        public int? UserID { get; set; } // null if default category
        public string CategoryName { get; set; }

        public User User { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Budget> Budgets { get; set; }
    }

    public class Budget
    {
        public int BudgetID { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public User User { get; set; }
        public Category Category { get; set; }
    }
}