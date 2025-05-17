using System;
using System.Collections.Generic;
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
        // Core properties
        public string Id { get; private set; }
        public string Name { get; set; }
        public decimal Balance { get; private set; }
        public List<Transaction> Transactions { get; private set; }
        
        // New PIN authentication
        private string Pin { get; set; }
        
        // Account type properties
        public AccountType Type { get; private set; }
        public decimal InterestRate { get; private set; }
        public decimal OverdraftLimit { get; private set; }
        public decimal MinimumBalance { get; private set; }

        // For JSON serialization
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

        // Constructor for new accounts
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
            
            // Initialize type-specific properties
            Type = type;
            SetAccountTypeProperties(type);
        }

        // PIN related methods
        public bool ValidatePin(string enteredPin)
        {
            return Pin == enteredPin;
        }
        
        public bool ChangePin(string currentPin, string newPin)
        {
            if (!ValidatePin(currentPin))
                return false;
            
            if (!ValidatePinFormat(newPin))
                return false;
            
            Pin = newPin;
            return true;
        }
        
        private bool ValidatePinFormat(string pin)
        {
            return !string.IsNullOrEmpty(pin) && pin.Length == 4 && pin.All(char.IsDigit);
        }

        // Account type conversion
        public bool ConvertAccountType(AccountType newType, string pin)
        {
            if (!ValidatePin(pin))
                return false;
            
            // Check conversion rules
            if (!CanConvertTo(newType))
                return false;
            
            Type = newType;
            SetAccountTypeProperties(newType);
            return true;
        }
        
        private bool CanConvertTo(AccountType newType)
        {
            switch (newType)
            {
                case AccountType.Savings:
                    // Any account can convert to savings
                    return true;
                
                case AccountType.Cheque:
                    // Need minimum 500 for cheque account
                    return Balance >= 500;
                
                case AccountType.Business:
                    // Need minimum 1000 for business account
                    return Balance >= 1000;
                
                default:
                    return false;
            }
        }
        
        private void SetAccountTypeProperties(AccountType type)
        {
            switch (type)
            {
                case AccountType.Savings:
                    InterestRate = 0.025m; // 2.5% interest
                    OverdraftLimit = 0;
                    MinimumBalance = 0;
                    break;
                
                case AccountType.Cheque:
                    InterestRate = 0.005m; // 0.5% interest
                    OverdraftLimit = 200;
                    MinimumBalance = 100;
                    break;
                
                case AccountType.Business:
                    InterestRate = 0.01m; // 1% interest
                    OverdraftLimit = 500;
                    MinimumBalance = 500;
                    break;
            }
        }

        // Transaction methods
        public bool Deposit(decimal amount, string pin)
        {
            if (!ValidatePin(pin))
                return false;

            if (amount <= 0)
                return false;

            Balance += amount;
            Transactions.Add(new Transaction(TransactionType.Deposit, amount, DateTime.Now));
            
            return true;
        }

        public bool Withdraw(decimal amount, string pin)
        {
            if (!ValidatePin(pin))
                return false;

            if (amount <= 0)
                return false;

            // Check if withdrawal is allowed based on account type and balance
            if (Balance - amount < -OverdraftLimit)
                return false;

            // Check if withdrawal would put account below minimum balance (except for overdraft)
            if (Balance - amount < MinimumBalance && Balance - amount >= 0)
                return false;

            Balance -= amount;
            Transactions.Add(new Transaction(TransactionType.Withdrawal, amount, DateTime.Now));
            
            return true;
        }

        // Account feature methods
        public decimal CalculateInterest()
        {
            // Only calculate interest on positive balances
            if (Balance <= 0)
                return 0;
                
            decimal interest = Balance * (InterestRate / 12); // Monthly interest
            return interest;
        }
        
        public bool ApplyMonthlyInterest(string pin)
        {
            if (!ValidatePin(pin))
                return false;
                
            decimal interestAmount = CalculateInterest();
            if (interestAmount > 0)
            {
                Balance += interestAmount;
                Transactions.Add(new Transaction(TransactionType.Interest, interestAmount, DateTime.Now));
                return true;
            }
            return false;
        }

        // Method to get transaction history with optional date filtering
        public List<Transaction> GetTransactionHistory(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = Transactions.AsQueryable();
            
            if (startDate.HasValue)
                query = query.Where(t => t.Timestamp >= startDate.Value);
                
            if (endDate.HasValue)
                query = query.Where(t => t.Timestamp <= endDate.Value);
                
            return query.OrderByDescending(t => t.Timestamp).ToList();
        }
        
        // Method to get account summary
        public string GetAccountSummary()
        {
            return $"Account ID: {Id}\n" +
                   $"Name: {Name}\n" +
                   $"Type: {Type}\n" +
                   $"Balance: {Balance:C}\n" +
                   $"Interest Rate: {InterestRate:P}\n" +
                   $"Overdraft Limit: {OverdraftLimit:C}\n" +
                   $"Minimum Balance: {MinimumBalance:C}";
        }
    }
}