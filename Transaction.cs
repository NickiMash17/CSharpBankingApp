using System;
using System.Text.Json.Serialization;

namespace CSharpBankingApp
{
    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Transfer,
        Interest,
        Fee,
        AccountTypeChange
    }

    public class Transaction
    {
        public Guid Id { get; private set; }
        public TransactionType Type { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string Description { get; private set; }

        // For JSON serialization
        [JsonConstructor]
        public Transaction(Guid id, TransactionType type, decimal amount, DateTime timestamp, string description)
        {
            Id = id;
            Type = type;
            Amount = amount;
            Timestamp = timestamp;
            Description = description;
        }

        // Constructor for new transactions
        public Transaction(TransactionType type, decimal amount, DateTime timestamp, string description = null)
        {
            Id = Guid.NewGuid();
            Type = type;
            Amount = amount;
            Timestamp = timestamp;
            Description = description ?? GetDefaultDescription(type);
        }

        private string GetDefaultDescription(TransactionType type)
        {
            return type switch
            {
                TransactionType.Deposit => "Deposit",
                TransactionType.Withdrawal => "Withdrawal",
                TransactionType.Transfer => "Transfer",
                TransactionType.Interest => "Interest payment",
                TransactionType.Fee => "Service fee",
                TransactionType.AccountTypeChange => "Account type conversion",
                _ => "Transaction"
            };
        }

        public override string ToString()
        {
            return $"{Timestamp:yyyy-MM-dd HH:mm:ss} | {Type} | {Amount:C} | {Description}";
        }
    }
}