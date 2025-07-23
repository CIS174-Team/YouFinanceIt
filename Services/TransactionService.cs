using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouFinanceIt.Data;
using YouFinanceIt.Models;

namespace YouFinanceIt.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly YFIDbContext _context;

        public TransactionService(YFIDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetUserTransactionsAsync(string username)
        {
            var user = await _context.Users
                .Include(u => u.Categories)
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null) return new List<Transaction>();

            return await _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.Category)
                .Where(t => t.UserID == user.Id)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task AddTransactionAsync(Transaction transaction, string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null) throw new Exception("User not found.");

            transaction.UserID = user.Id;
            transaction.CreatedDate = DateTime.UtcNow;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int transactionId, string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null) return null;

            return await _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.TransactionID == transactionId && t.UserID == user.Id);
        }

        public async Task UpdateTransactionAsync(Transaction updatedTransaction, string username)
        {
            var existingTransaction = await GetTransactionByIdAsync(updatedTransaction.TransactionID, username);
            if (existingTransaction == null) throw new Exception("Transaction not found.");

            existingTransaction.TransactionDate = updatedTransaction.TransactionDate;
            existingTransaction.Description = updatedTransaction.Description;
            existingTransaction.Amount = updatedTransaction.Amount;
            existingTransaction.AccountID = updatedTransaction.AccountID;
            existingTransaction.CategoryID = updatedTransaction.CategoryID;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransactionAsync(int transactionId, string username)
        {
            var transaction = await GetTransactionByIdAsync(transactionId, username);
            if (transaction == null) throw new Exception("Transaction not found.");

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
