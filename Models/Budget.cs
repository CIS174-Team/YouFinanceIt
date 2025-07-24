<<<<<<< HEAD
namespace YouFinanceIt.Models
=======
using YouFinanceIt.Data;

public class Budget
>>>>>>> a0189e97b5bcab242b3a3b46957aa61e03b34c3b
{
    public class Budget
    {
        public int BudgetID { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

<<<<<<< HEAD
        public User User { get; set; }
        public Category Category { get; set; }
    }
=======
    public User? User { get; set; }
    public Category? Category { get; set; }
>>>>>>> a0189e97b5bcab242b3a3b46957aa61e03b34c3b
}
