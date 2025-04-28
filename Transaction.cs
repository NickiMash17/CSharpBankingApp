namespace CSharpBankingApp
{
    public class Transaction
    {
        public decimal Amount { get; }
        public DateTime Date { get; }
        public string Type { get; } // "Deposit" or "Withdrawal"
        public string Notes { get; }

        public Transaction(decimal amount, DateTime date, string type, string notes)
        {
            Amount = amount;
            Date = date;
            Type = type;
            Notes = notes;
        }
    }
}