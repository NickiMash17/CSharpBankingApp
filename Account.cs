using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;

namespace CSharpBankingApp
{
    public enum AccountType
    {
        Savings,
        Cheque,
        Business
    }

    public class Account
    {
        [JsonPropertyName("id")]
        public string Id { get; private set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("balance")]
        public decimal Balance { get; private set; }
        
        [JsonPropertyName("transactions")]
        public List<Transaction> Transactions { get; private set; }
        
        [JsonPropertyName("pin")]
        private string Pin { get; set; }
        
        [JsonPropertyName("type")]
        public AccountType Type { get; private set; }
        
        [JsonPropertyName("interestRate")]
        public decimal InterestRate { get; private set; }
        
        [JsonPropertyName("overdraftLimit")]
        public decimal OverdraftLimit { get; private set; }
        
        [JsonPropertyName("minimumBalance")]
        public decimal MinimumBalance { get; private set; }

        [JsonConstructor]
        public Account(string id, string name, decimal balance, List<Transaction> transactions, 
                       string pin, AccountType type, decimal interestRate, 
                       decimal overdraftLimit, decimal minimumBalance)
        {
            Id = id;
            Name = name;
            Balance = balance;
            Transactions = transactions ?? new List<Transaction>();
            Pin = pin;
            Type = type;
            InterestRate = interestRate;
            OverdraftLimit = overdraftLimit;
            MinimumBalance = minimumBalance;
        }

        public Account(string name, string pin, AccountType type = AccountType.Savings)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Account name cannot be empty or null", nameof(name));
            
            if (!ValidatePinFormat(pin))
                throw new ArgumentException("PIN must be exactly 4 digits", nameof(pin));

            Id = Guid.NewGuid().ToString();
            Name = name;
            Balance = 0;
            Transactions = new List<Transaction>();
            Pin = pin;
            Type = type;
            SetAccountTypeProperties(type);
        }

        public bool ValidatePin(string enteredPin) => Pin == enteredPin;
        
        public bool ChangePin(string currentPin, string newPin)
        {
            if (!ValidatePin(currentPin)) return false;
            if (!ValidatePinFormat(newPin)) return false;
            
            Pin = newPin;
            return true;
        }
        
        private static bool ValidatePinFormat(string pin) => 
            !string.IsNullOrEmpty(pin) && pin.Length == 4 && pin.All(char.IsDigit);

        public bool ConvertAccountType(AccountType newType, string pin)
        {
            if (!ValidatePin(pin)) return false;
            if (!CanConvertTo(newType)) return false;
            
            Type = newType;
            SetAccountTypeProperties(newType);
            return true;
        }
        
        private bool CanConvertTo(AccountType newType) => newType switch
        {
            AccountType.Savings => true,
            AccountType.Cheque => Balance >= 500,
            AccountType.Business => Balance >= 1000,
            _ => false
        };
        
        private void SetAccountTypeProperties(AccountType type)
        {
            switch (type)
            {
                case AccountType.Savings:
                    InterestRate = 0.025m;
                    OverdraftLimit = 0;
                    MinimumBalance = 0;
                    break;
                
                case AccountType.Cheque:
                    InterestRate = 0.005m;
                    OverdraftLimit = 200;
                    MinimumBalance = 100;
                    break;
                
                case AccountType.Business:
                    InterestRate = 0.01m;
                    OverdraftLimit = 500;
                    MinimumBalance = 500;
                    break;
            }
        }

        public bool Deposit(decimal amount, string pin)
        {
            if (!ValidatePin(pin) || amount <= 0)
                return false;

            Balance += amount;
            Transactions.Add(new Transaction(
                TransactionType.Deposit, 
                amount, 
                DateTime.Now, 
                $"Deposit of {FormatZAR(amount)}"));
            return true;
        }

        public bool Withdraw(decimal amount, string pin)
        {
            if (!ValidatePin(pin) || amount <= 0)
                return false;

            if (Balance - amount < -OverdraftLimit)
                return false;

            if (Balance - amount < MinimumBalance && Balance - amount >= 0)
                return false;

            Balance -= amount;
            Transactions.Add(new Transaction(
                TransactionType.Withdrawal, 
                amount, 
                DateTime.Now, 
                $"Withdrawal of {FormatZAR(amount)}"));
            return true;
        }

        public decimal CalculateInterest() => 
            Balance <= 0 ? 0 : Balance * (InterestRate / 12);
        
        public bool ApplyMonthlyInterest(string pin)
        {
            if (!ValidatePin(pin))
                return false;
                
            decimal interestAmount = CalculateInterest();
            if (interestAmount > 0)
            {
                Balance += interestAmount;
                Transactions.Add(new Transaction(
                    TransactionType.Interest, 
                    interestAmount, 
                    DateTime.Now, 
                    $"Interest payment of {FormatZAR(interestAmount)}"));
                return true;
            }
            return false;
        }

        public List<Transaction> GetTransactionHistory(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = Transactions.AsQueryable();
            
            if (startDate.HasValue)
                query = query.Where(t => t.Timestamp >= startDate.Value);
                
            if (endDate.HasValue)
                query = query.Where(t => t.Timestamp <= endDate.Value);
                
            return query.OrderByDescending(t => t.Timestamp).ToList();
        }
        
        public string GetAccountSummary()
        {
            return $"Account ID: {Id}\n" +
                   $"Name: {Name}\n" +
                   $"Type: {Type}\n" +
                   $"Balance: {FormatZAR(Balance)}\n" +
                   $"Interest Rate: {InterestRate:P}\n" +
                   $"Overdraft Limit: {FormatZAR(OverdraftLimit)}\n" +
                   $"Minimum Balance: {FormatZAR(MinimumBalance)}";
        }

        private static string FormatZAR(decimal amount)
        {
            var zarCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-ZA");
            return amount.ToString("C2", zarCulture);
        }
    }
}