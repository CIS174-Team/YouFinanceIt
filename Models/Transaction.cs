public class Transaction
{
    public int TransactionID { get; set; }
    public int UserID { get; set; }
    public int AccountID { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Description { get; set; }
    public int CategoryID { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedDate { get; set; }

    public User User { get; set; }
    public Account Account { get; set; }
    public Category Category { get; set; }
}