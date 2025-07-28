using System.Collections.Generic;
using System.Threading.Tasks;
using YouFinanceIt.Data;

namespace YouFinanceIt.Services
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetAllAsync(string userId);
        Task<Transaction?> GetByIdAsync(int id, string userId);
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(int id, string userId);
    }
}
