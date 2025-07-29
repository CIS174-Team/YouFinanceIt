using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionModel = YouFinanceIt.Data.Transaction;

namespace YouFinanceIt.Services
{
    public interface ITransactionService
    {
        Task<List<TransactionModel>> GetAllAsync(string userId);
        Task<TransactionModel?> GetByIdAsync(int id, string userId);
        Task AddAsync(TransactionModel transaction);
        Task UpdateAsync(TransactionModel transaction);
        Task DeleteAsync(int id, string userId);
    }
}
