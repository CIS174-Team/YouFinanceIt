using YouFinanceIt.Models;

public class Category
{
    public int CategoryID { get; set; }
    public string UserID { get; set; } // Null for default categories
    public string? CategoryName { get; set; }

    public ApplicationUser? User { get; set; }
    public ICollection<Transaction>? Transactions { get; set; }
    public ICollection<Budget>? Budgets { get; set; }
}
