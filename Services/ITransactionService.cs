using System.Collections.Generic;
using System.Threading.Tasks;
using YouFinanceIt.Models;

namespace YouFinanceIt.Services
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetUserTransactionsAsync(string username);
        Task AddTransactionAsync(Transaction transaction, string username);
        Task<Transaction> GetTransactionByIdAsync(int transactionId, string username);
        Task UpdateTransactionAsync(Transaction transaction, string username);
        Task DeleteTransactionAsync(int transactionId, string username);
    }
}
