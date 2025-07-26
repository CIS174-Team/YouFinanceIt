using YouFinanceIt.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YouFinanceIt.Services
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetAllAsync(int userId);
        Task<Transaction?> GetByIdAsync(int id, int userId);
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(int id, int userId);
    }
}