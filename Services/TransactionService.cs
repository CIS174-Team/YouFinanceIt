using Microsoft.EntityFrameworkCore;
using YouFinanceIt.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using YouFinanceIt.Models;

namespace YouFinanceIt.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetAllAsync(string userId)
        {
            return await _context.Transactions
                .Where(t => t.UserID == userId)
                .ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(int id, string userId)
        {
            return await _context.Transactions
                .FirstOrDefaultAsync(t => t.TransactionID == id && t.UserID == userId);
        }

        public async Task AddAsync(TransactionModel transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TransactionModel transaction)
        {
            var existing = await _context.Transactions.FindAsync(transaction.TransactionID);
            if (existing is not null && existing.UserID == transaction.UserID)
            {
                _context.Entry(existing).CurrentValues.SetValues(transaction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.TransactionID == id && t.UserID == userId);
            if (transaction is not null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }
    }
}
