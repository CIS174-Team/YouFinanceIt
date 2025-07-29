using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using YouFinanceIt.Models;

namespace YouFinanceIt.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Budget> Budgets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Always call base first for Identity

            // Decimal precision
            modelBuilder.Entity<Account>().Property(a => a.Balance).HasPrecision(18, 2);
            modelBuilder.Entity<Budget>().Property(b => b.Amount).HasPrecision(18, 2);
            modelBuilder.Entity<Transaction>().Property(t => t.Amount).HasPrecision(18, 2);

            // Relationships
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.User)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.UserID)
                .OnDelete(DeleteBehavior.Cascade);

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
        }
    }

    public class Account
    {
        public int AccountID { get; set; }
        public string UserID { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }

        public ApplicationUser User { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }

    public class Transaction
    {
        public int TransactionID { get; set; }
        public string UserID { get; set; }  // Changed from int to string
        public int AccountID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }

        public ApplicationUser User { get; set; }  // If using Identity User
        public Account Account { get; set; }
        public Category Category { get; set; }
    }


    public class Category
    {
        public int CategoryID { get; set; }
        public string? UserID { get; set; } // null if default category
        public string CategoryName { get; set; }

        public ApplicationUser User { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Budget> Budgets { get; set; }
    }

    public class Budget
    {
        public int BudgetID { get; set; }
        public string UserID { get; set; }
        public int CategoryID { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ApplicationUser User { get; set; }
        public Category Category { get; set; }
    }

    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedDate { get; set; }

        public ICollection<Account> Accounts { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Budget> Budgets { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
