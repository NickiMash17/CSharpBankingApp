namespace CSharpBankingApp
{
    public class Account
    {
        private List<Transaction> _transactions = new List<Transaction>();
        
        public string Number { get; }
        public string Owner { get; }
        public decimal Balance 
        { 
            get
            {
                decimal balance = 0;
                foreach (var transaction in _transactions)
                {
                    balance += transaction.Amount;
                }
                return balance;
            }
        }

        public Account(string name, decimal initialBalance)
        {
            this.Owner = name;
            this.Number = new Random().Next(100000, 999999).ToString();
            MakeDeposit(initialBalance, DateTime.Now, "Initial balance");
        }

        public void MakeDeposit(decimal amount, DateTime date, string note)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Deposit amount must be positive");
            }
            var deposit = new Transaction(amount, date, "Deposit", note);
            _transactions.Add(deposit);
        }

        public void MakeWithdrawal(decimal amount, DateTime date, string note)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Withdrawal amount must be positive");
            }
            if (Balance - amount < 0)
            {
                throw new InvalidOperationException("Insufficient funds for this withdrawal");
            }
            var withdrawal = new Transaction(-amount, date, "Withdrawal", note);
            _transactions.Add(withdrawal);
        }

        public string GetAccountHistory()
        {
            var report = new System.Text.StringBuilder();
            
            report.AppendLine("Date\t\tAmount\tNote");
            foreach (var transaction in _transactions)
            {
                report.AppendLine($"{transaction.Date.ToShortDateString()}\t{transaction.Amount}\t{transaction.Notes}");
            }
            
            return report.ToString();
        }
    }
}