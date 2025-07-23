namespace YouFinanceIt.Models
{
    public class Account
    {
        public int AccountID { get; set; }
        public int UserID { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }

        public User User { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
