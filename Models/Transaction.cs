using System;
using System.Text.Json.Serialization;

namespace CSharpBankingApp.Models
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
        public string Description { get; private set; } // Non-nullable

        [JsonConstructor]
        public Transaction(Guid id, TransactionType type, decimal amount, DateTime timestamp, string description)
        {
            Id = id;
            Type = type;
            Amount = amount;
            Timestamp = timestamp;
            Description = description ?? GetDefaultDescription(type); // Ensure never null
        }

        public Transaction(TransactionType type, decimal amount, DateTime timestamp, string? description = null)
            : this(Guid.NewGuid(), type, amount, timestamp, description ?? GetDefaultDescription(type))
        {
        }

        private static string GetDefaultDescription(TransactionType type) => type switch
        {
            TransactionType.Deposit => "Deposit",
            TransactionType.Withdrawal => "Withdrawal",
            TransactionType.Transfer => "Transfer",
            TransactionType.Interest => "Interest payment",
            TransactionType.Fee => "Service fee",
            TransactionType.AccountTypeChange => "Account type conversion",
            _ => "Transaction"
        };

        public override string ToString()
        {
            var zarCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-ZA");
            return $"{Timestamp:yyyy-MM-dd HH:mm:ss} | {Type} | {Amount.ToString("C2", zarCulture)} | {Description}";
        }
    }
}