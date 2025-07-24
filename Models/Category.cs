<<<<<<< HEAD
namespace YouFinanceIt.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public int? UserID { get; set; }
        public string CategoryName { get; set; }

        public User User { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Budget> Budgets { get; set; }
    }
=======
using YouFinanceIt.Data;

public class Category
{
    public int CategoryID { get; set; }
    public int UserID { get; set; } // Null for default categories
    public string? CategoryName { get; set; }

    public User? User { get; set; }
    public ICollection<Transaction>? Transactions { get; set; }
    public ICollection<Budget>? Budgets { get; set; }
>>>>>>> a0189e97b5bcab242b3a3b46957aa61e03b34c3b
}
