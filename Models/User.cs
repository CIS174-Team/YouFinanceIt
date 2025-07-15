public class User
{
    public int UserID { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedDate { get; set; }

    public ICollection<Account> Accounts { get; set; }
    public ICollection<Transaction> Transactions { get; set; }
    public ICollection<Budget> Budgets { get; set; }
}