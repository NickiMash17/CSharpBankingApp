using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CSharpBankingApp
{
    class Program
    {
        private static Bank _bank = null!;
        private static Account? _currentAccount = null;
        private static bool _running = true;

        static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("Welcome to SA Banking App v2.0");
            Console.WriteLine("========================================");
            
            _bank = new Bank();
            
            while (_running)
            {
                if (_currentAccount == null)
                {
                    ShowMainMenu();
                }
                else
                {
                    ShowAccountMenu();
                }
            }
            
            Console.WriteLine("Thank you for using SA Banking Application!");
            _bank.Dispose();
        }

        #region Main Menu
        
        static void ShowMainMenu()
        {
            Console.WriteLine("\nMain Menu:");
            Console.WriteLine("1. Create New Account");
            Console.WriteLine("2. Login to Existing Account");
            Console.WriteLine("3. View All Accounts (Admin)");
            Console.WriteLine("4. Manage Backups (Admin)");
            Console.WriteLine("5. Exit");
            Console.Write("\nEnter your choice: ");
            
            string choice = Console.ReadLine() ?? "";
            
            switch (choice)
            {
                case "1":
                    CreateAccount();
                    break;
                case "2":
                    LoginToAccount();
                    break;
                case "3":
                    ViewAllAccounts();
                    break;
                case "4":
                    ManageBackups();
                    break;
                case "5":
                    _running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
        
        static void CreateAccount()
        {
            Console.WriteLine("\n=== Create New Account ===");
            
            Console.Write("Enter account name: ");
            string name = Console.ReadLine() ?? "";
            
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Account name cannot be empty.");
                return;
            }
            
            if (_bank.FindAccountByName(name) != null)
            {
                Console.WriteLine("An account with this name already exists.");
                return;
            }
            
            Console.Write("Enter 4-digit PIN: ");
            string pin = Console.ReadLine() ?? "";
            
            Console.WriteLine("\nSelect Account Type:");
            Console.WriteLine("1. Savings Account (2.5% interest, no overdraft)");
            Console.WriteLine("2. Cheque Account (0.5% interest, R200 overdraft)");
            Console.WriteLine("3. Business Account (1% interest, R500 overdraft)");
            Console.Write("Enter your choice (1-3): ");
            
            string typeChoice = Console.ReadLine() ?? "1";
            AccountType type = AccountType.Savings;
            
            switch (typeChoice)
            {
                case "2":
                    type = AccountType.Cheque;
                    break;
                case "3":
                    type = AccountType.Business;
                    break;
            }
            
            Account? newAccount = _bank.CreateAccount(name, pin, type);
            
            if (newAccount != null)
            {
                Console.WriteLine($"Account created successfully!");
                Console.WriteLine($"Your account ID is: {newAccount.Id}");
                Console.WriteLine("\nAccount details:");
                Console.WriteLine(newAccount.GetAccountSummary());
                
                Console.Write("\nWould you like to make an initial deposit? (y/n): ");
                if (Console.ReadLine()?.ToLower() == "y")
                {
                    Console.Write("Enter deposit amount: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                    {
                        if (_bank.Deposit(newAccount.Id, amount, pin))
                        {
                            Console.WriteLine($"Successfully deposited {amount:C}");
                            Console.WriteLine($"New balance: {newAccount.Balance:C}");
                        }
                        else
                        {
                            Console.WriteLine("Deposit failed. Please try again later.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid amount format.");
                    }
                }
                
                _currentAccount = newAccount;
            }
            else
            {
                Console.WriteLine("Failed to create account. Please check your inputs and try again.");
            }
        }
        
        static void LoginToAccount()
        {
            Console.WriteLine("\n=== Login to Account ===");
            
            Console.Write("Enter account name: ");
            string name = Console.ReadLine() ?? "";
            
            var account = _bank.FindAccountByName(name);
            
            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }
            
            Console.Write("Enter PIN: ");
            string pin = Console.ReadLine() ?? "";
            
            if (_bank.VerifyPin(account.Id, pin))
            {
                Console.WriteLine($"Welcome back, {account.Name}!");
                _currentAccount = account;
            }
            else
            {
                Console.WriteLine("Incorrect PIN. Access denied.");
            }
        }
        
        static void ViewAllAccounts()
        {
            Console.WriteLine("\n=== All Accounts (Admin Mode) ===");
            Console.Write("Enter admin password: ");
            
            if (Console.ReadLine() != "admin123")
            {
                Console.WriteLine("Incorrect admin password.");
                return;
            }
            
            var accounts = _bank.GetAllAccounts();
            
            if (accounts.Count == 0)
            {
                Console.WriteLine("No accounts found.");
                return;
            }
            
            Console.WriteLine($"\nTotal accounts: {accounts.Count}");
            Console.WriteLine("ID | Name | Type | Balance");
            Console.WriteLine("----------------------------");
            
            foreach (var account in accounts)
            {
                Console.WriteLine($"{account.Id} | {account.Name} | {account.Type} | {account.Balance:C}");
            }
        }
        
        static void ManageBackups()
        {
            Console.WriteLine("\n=== Backup Management (Admin) ===");
            Console.Write("Enter admin password: ");
            
            if (Console.ReadLine() != "admin123")
            {
                Console.WriteLine("Incorrect admin password.");
                return;
            }
            
            bool manageBackups = true;
            
            while (manageBackups)
            {
                Console.WriteLine("\nBackup Management Menu:");
                Console.WriteLine("1. Create New Backup");
                Console.WriteLine("2. View Available Backups");
                Console.WriteLine("3. Restore from Backup");
                Console.WriteLine("4. Return to Main Menu");
                Console.Write("\nEnter your choice: ");
                
                string choice = Console.ReadLine() ?? "";
                
                switch (choice)
                {
                    case "1":
                        _bank.CreateBackup();
                        Console.WriteLine("Backup created successfully.");
                        break;
                    case "2":
                        var backups = _bank.GetAvailableBackups();
                        if (backups.Count == 0)
                        {
                            Console.WriteLine("No backups available.");
                        }
                        else
                        {
                            Console.WriteLine("\nAvailable Backups:");
                            for (int i = 0; i < backups.Count; i++)
                            {
                                Console.WriteLine($"{i+1}. {backups[i]}");
                            }
                        }
                        break;
                    case "3":
                        var restoreBackups = _bank.GetAvailableBackups();
                        if (restoreBackups.Count == 0)
                        {
                            Console.WriteLine("No backups available for restore.");
                        }
                        else
                        {
                            Console.WriteLine("\nSelect Backup to Restore:");
                            for (int i = 0; i < restoreBackups.Count; i++)
                            {
                                Console.WriteLine($"{i+1}. {restoreBackups[i]}");
                            }
                            Console.Write("Enter number or 0 to cancel: ");
                            
                            if (int.TryParse(Console.ReadLine(), out int backupIndex) && 
                                backupIndex > 0 && backupIndex <= restoreBackups.Count)
                            {
                                Console.Write("Are you sure? This will overwrite current data. (y/n): ");
                                if (Console.ReadLine()?.ToLower() == "y")
                                {
                                    if (_bank.RestoreFromBackup(restoreBackups[backupIndex - 1]))
                                    {
                                        Console.WriteLine("Backup restored successfully.");
                                        _currentAccount = null;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failed to restore backup.");
                                    }
                                }
                            }
                        }
                        break;
                    case "4":
                        manageBackups = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        
        #endregion
        
        #region Account Menu
        
        static void ShowAccountMenu()
        {
            if (_currentAccount == null) return;
            
            Console.WriteLine($"\nAccount: {_currentAccount.Name} | Balance: {_currentAccount.Balance:C} | Type: {_currentAccount.Type}");
            Console.WriteLine("\nAccount Menu:");
            Console.WriteLine("1. View Account Details");
            Console.WriteLine("2. Deposit");
            Console.WriteLine("3. Withdraw");
            Console.WriteLine("4. Transfer");
            Console.WriteLine("5. View Transaction History");
            Console.WriteLine("6. Change PIN");
            Console.WriteLine("7. Change Account Type");
            Console.WriteLine("8. Calculate Interest");
            Console.WriteLine("9. Logout");
            Console.Write("\nEnter your choice: ");
            
            string choice = Console.ReadLine() ?? "";
            
            switch (choice)
            {
                case "1":
                    DisplayAccountDetails();
                    break;
                case "2":
                    MakeDeposit();
                    break;
                case "3":
                    MakeWithdrawal();
                    break;
                case "4":
                    MakeTransfer();
                    break;
                case "5":
                    ViewTransactionHistory();
                    break;
                case "6":
                    ChangePin();
                    break;
                case "7":
                    ChangeAccountType();
                    break;
                case "8":
                    CalculateInterest();
                    break;
                case "9":
                    _currentAccount = null;
                    Console.WriteLine("Logged out successfully.");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
        
        static void DisplayAccountDetails()
        {
            if (_currentAccount == null) return;
            
            Console.WriteLine("\n=== Account Details ===");
            Console.WriteLine(_currentAccount.GetAccountSummary());
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }
        
        static void MakeDeposit()
        {
            if (_currentAccount == null) return;
            
            Console.WriteLine("\n=== Make Deposit ===");
            
            Console.Write("Enter amount to deposit: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Console.WriteLine("Invalid amount format.");
                return;
            }
            
            Console.Write("Enter PIN to confirm: ");
            string pin = Console.ReadLine() ?? "";
            
            if (_bank.Deposit(_currentAccount.Id, amount, pin))
            {
                Console.WriteLine($"Successfully deposited {amount:C}");
                Console.WriteLine($"New balance: {_currentAccount.Balance:C}");
            }
            else
            {
                Console.WriteLine("Deposit failed. Please check your PIN and try again.");
            }
        }
        
        static void MakeWithdrawal()
        {
            if (_currentAccount == null) return;
            
            Console.WriteLine("\n=== Make Withdrawal ===");
            
            Console.Write("Enter amount to withdraw: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Console.WriteLine("Invalid amount format.");
                return;
            }
            
            Console.Write("Enter PIN to confirm: ");
            string pin = Console.ReadLine() ?? "";
            
            if (_bank.Withdraw(_currentAccount.Id, amount, pin))
            {
                Console.WriteLine($"Successfully withdrew {amount:C}");
                Console.WriteLine($"New balance: {_currentAccount.Balance:C}");
            }
            else
            {
                Console.WriteLine("Withdrawal failed. Please check your PIN, balance, and account limits.");
            }
        }
        
        static void MakeTransfer()
        {
            if (_currentAccount == null) return;
            
            Console.WriteLine("\n=== Make Transfer ===");
            
            Console.Write("Enter recipient account name: ");
            string recipientName = Console.ReadLine() ?? "";
            
            var recipientAccount = _bank.FindAccountByName(recipientName);
            
            if (recipientAccount == null)
            {
                Console.WriteLine("Recipient account not found.");
                return;
            }
            
            if (recipientAccount.Id == _currentAccount.Id)
            {
                Console.WriteLine("Cannot transfer to the same account.");
                return;
            }
            
            Console.Write("Enter amount to transfer: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Console.WriteLine("Invalid amount format.");
                return;
            }
            
            Console.Write("Enter PIN to confirm: ");
            string pin = Console.ReadLine() ?? "";
            
            if (_bank.Transfer(_currentAccount.Id, recipientAccount.Id, amount, pin))
            {
                Console.WriteLine($"Successfully transferred {amount:C} to {recipientAccount.Name}");
                Console.WriteLine($"New balance: {_currentAccount.Balance:C}");
            }
            else
            {
                Console.WriteLine("Transfer failed. Please check your PIN, balance, and account limits.");
            }
        }
        
        static void ViewTransactionHistory()
        {
            if (_currentAccount == null) return;
            
            Console.WriteLine("\n=== Transaction History ===");
            
            Console.WriteLine("Filter options:");
            Console.WriteLine("1. All transactions");
            Console.WriteLine("2. Filter by date range");
            Console.Write("Enter your choice: ");
            
            string choice = Console.ReadLine() ?? "";
            
            List<Transaction> transactions;
            
            switch (choice)
            {
                case "2":
                    Console.Write("Enter start date (YYYY-MM-DD): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                    {
                        Console.WriteLine("Invalid date format.");
                        return;
                    }
                    
                    Console.Write("Enter end date (YYYY-MM-DD): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
                    {
                        Console.WriteLine("Invalid date format.");
                        return;
                    }
                    
                    transactions = _currentAccount.GetTransactionHistory(startDate, endDate);
                    break;
                default:
                    transactions = _currentAccount.GetTransactionHistory();
                    break;
            }
            
            if (transactions.Count == 0)
            {
                Console.WriteLine("No transactions found for the specified criteria.");
                return;
            }
            
            Console.WriteLine("\nTransaction History:");
            Console.WriteLine("Date | Type | Amount | Description");
            Console.WriteLine("-------------------------------");
            
            foreach (var transaction in transactions)
            {
                Console.WriteLine(transaction.ToString());
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }
        
        static void ChangePin()
        {
            if (_currentAccount == null) return;
            
            Console.WriteLine("\n=== Change PIN ===");
            
            Console.Write("Enter current PIN: ");
            string currentPin = Console.ReadLine() ?? "";
            
            Console.Write("Enter new 4-digit PIN: ");
            string newPin = Console.ReadLine() ?? "";
            
            if (_bank.ChangePin(_currentAccount.Id, currentPin, newPin))
            {
                Console.WriteLine("PIN changed successfully.");
            }
            else
            {
                Console.WriteLine("Failed to change PIN. Please check your current PIN and ensure new PIN is valid (4 digits).");
            }
        }
        
        static void ChangeAccountType()
        {
            if (_currentAccount == null) return;
            
            Console.WriteLine("\n=== Change Account Type ===");
            Console.WriteLine("Current account type: " + _currentAccount.Type);
            Console.WriteLine("\nAvailable account types:");
            Console.WriteLine("1. Savings Account (2.5% interest, no overdraft)");
            Console.WriteLine("2. Cheque Account (0.5% interest, R200 overdraft, min balance R100)");
            Console.WriteLine("3. Business Account (1% interest, R500 overdraft, min balance R500)");
            Console.Write("\nSelect new account type (1-3): ");
            
            string choice = Console.ReadLine() ?? "";
            AccountType newType;
            
            switch (choice)
            {
                case "1":
                    newType = AccountType.Savings;
                    break;
                case "2":
                    newType = AccountType.Cheque;
                    break;
                case "3":
                    newType = AccountType.Business;
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    return;
            }
            
            if (newType == _currentAccount.Type)
            {
                Console.WriteLine("This is already your current account type.");
                return;
            }
            
            Console.Write("\nEnter PIN to confirm conversion: ");
            string pin = Console.ReadLine() ?? "";
            
            if (_bank.ConvertAccountType(_currentAccount.Id, newType, pin))
            {
                Console.WriteLine($"Account successfully converted to {newType} account.");
                Console.WriteLine(_currentAccount.GetAccountSummary());
            }
            else
            {
                Console.WriteLine("Conversion failed. Please check your PIN and ensure you meet the minimum requirements.");
            }
        }
        
        static void CalculateInterest()
        {
            if (_currentAccount == null) return;
            
            Console.WriteLine("\n=== Interest Calculation ===");
            
            decimal interestAmount = _currentAccount.CalculateInterest();
            
            Console.WriteLine($"Current balance: {_currentAccount.Balance:C}");
            Console.WriteLine($"Interest rate: {_currentAccount.InterestRate:P}");
            Console.WriteLine($"Monthly interest amount: {interestAmount:C}");
            
            Console.Write("\nWould you like to apply this interest now? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                Console.Write("Enter PIN to confirm: ");
                string pin = Console.ReadLine() ?? "";
                
                if (_currentAccount.ApplyMonthlyInterest(pin))
                {
                    Console.WriteLine($"Interest of {interestAmount:C} applied successfully.");
                    Console.WriteLine($"New balance: {_currentAccount.Balance:C}");
                }
                else
                {
                    Console.WriteLine("Failed to apply interest. Please check your PIN.");
                }
            }
        }
        
        #endregion
    }
}