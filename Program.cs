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
            
            switch (Console.ReadLine())
            {
                case "1": CreateAccount(); break;
                case "2": LoginToAccount(); break;
                case "3": ViewAllAccounts(); break;
                case "4": ManageBackups(); break;
                case "5": _running = false; break;
                default: Console.WriteLine("Invalid choice."); break;
            }
        }
        
        static void CreateAccount()
        {
            Console.WriteLine("\n=== Create New Account ===");
            
            Console.Write("Enter account name: ");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Account name cannot be empty.");
                return;
            }
            
            if (_bank.FindAccountByName(name) != null)
            {
                Console.WriteLine("Account with this name already exists.");
                return;
            }
            
            Console.Write("Enter 4-digit PIN: ");
            string? pin = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(pin) || pin.Length != 4 || !pin.All(char.IsDigit))
            {
                Console.WriteLine("Invalid PIN format.");
                return;
            }

            Console.WriteLine("\nSelect Account Type:");
            Console.WriteLine("1. Savings Account (2.5% interest, no overdraft)");
            Console.WriteLine("2. Cheque Account (0.5% interest, R200 overdraft)");
            Console.WriteLine("3. Business Account (1% interest, R500 overdraft)");
            Console.Write("Enter choice (1-3): ");
            
            AccountType type = Console.ReadLine() switch
            {
                "2" => AccountType.Cheque,
                "3" => AccountType.Business,
                _ => AccountType.Savings
            };

            var account = _bank.CreateAccount(name, pin, type);
            if (account == null)
            {
                Console.WriteLine("Failed to create account.");
                return;
            }

            Console.WriteLine($"Account created! ID: {account.Id}");
            Console.WriteLine(account.GetAccountSummary());
            
            Console.Write("\nMake initial deposit? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                Console.Write("Enter amount: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                {
                    if (_bank.Deposit(account.Id, amount, pin))
                    {
                        Console.WriteLine($"Deposited {amount:C}. New balance: {account.Balance:C}");
                    }
                }
            }
            
            _currentAccount = account;
        }
        
        static void LoginToAccount()
        {
            Console.WriteLine("\n=== Login ===");
            Console.Write("Enter account name: ");
            string? name = Console.ReadLine();
            
            var account = _bank.FindAccountByName(name ?? "");
            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }
            
            Console.Write("Enter PIN: ");
            string? pin = Console.ReadLine();
            
            if (_bank.VerifyPin(account.Id, pin ?? ""))
            {
                Console.WriteLine($"Welcome, {account.Name}!");
                _currentAccount = account;
            }
            else
            {
                Console.WriteLine("Incorrect PIN.");
            }
        }
        
        static void ViewAllAccounts()
        {
            Console.WriteLine("\n=== All Accounts ===");
            Console.Write("Enter admin password: ");
            if (Console.ReadLine() != "admin123")
            {
                Console.WriteLine("Access denied.");
                return;
            }
            
            var accounts = _bank.GetAllAccounts();
            Console.WriteLine($"\nTotal accounts: {accounts.Count}");
            foreach (var acc in accounts)
            {
                Console.WriteLine($"{acc.Id} | {acc.Name} | {acc.Type} | {acc.Balance:C}");
            }
        }
        
        static void ManageBackups()
        {
            Console.WriteLine("\n=== Backup Management ===");
            Console.Write("Enter admin password: ");
            if (Console.ReadLine() != "admin123")
            {
                Console.WriteLine("Access denied.");
                return;
            }

            while (true)
            {
                Console.WriteLine("\n1. Create Backup\n2. View Backups\n3. Restore\n4. Back");
                Console.Write("Choice: ");
                switch (Console.ReadLine())
                {
                    case "1":
                        _bank.CreateBackup();
                        Console.WriteLine("Backup created.");
                        break;
                    case "2":
                        var backups = _bank.GetAvailableBackups();
                        if (!backups.Any())
                        {
                            Console.WriteLine("No backups found.");
                            break;
                        }
                        Console.WriteLine("\nAvailable Backups:");
                        for (int i = 0; i < backups.Count; i++)
                        {
                            Console.WriteLine($"{i+1}. {backups[i]}");
                        }
                        break;
                    case "3":
                        var restoreBackups = _bank.GetAvailableBackups();
                        if (!restoreBackups.Any())
                        {
                            Console.WriteLine("No backups available.");
                            break;
                        }
                        Console.Write("Enter backup number: ");
                        if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= restoreBackups.Count)
                        {
                            Console.Write("Confirm restore? (y/n): ");
                            if (Console.ReadLine()?.ToLower() == "y")
                            {
                                if (_bank.RestoreFromBackup(restoreBackups[idx-1]))
                                {
                                    Console.WriteLine("Restore successful.");
                                    _currentAccount = null;
                                    return;
                                }
                            }
                        }
                        break;
                    case "4":
                        default:
                        return;
                }
            }
        }
        
        #endregion
        
        #region Account Menu
        
        static void ShowAccountMenu()
        {
            if (_currentAccount == null) return;
            
            Console.WriteLine($"\nAccount: {_currentAccount.Name} | Balance: {_currentAccount.Balance:C}");
            Console.WriteLine("\n1. Details\n2. Deposit\n3. Withdraw\n4. Transfer\n5. Transactions\n6. Change PIN\n7. Change Type\n8. Interest\n9. Logout");
            Console.Write("Choice: ");
            
            switch (Console.ReadLine())
            {
                case "1": DisplayAccountDetails(); break;
                case "2": MakeDeposit(); break;
                case "3": MakeWithdrawal(); break;
                case "4": MakeTransfer(); break;
                case "5": ViewTransactionHistory(); break;
                case "6": ChangePin(); break;
                case "7": ChangeAccountType(); break;
                case "8": CalculateInterest(); break;
                case "9": _currentAccount = null; break;
                default: Console.WriteLine("Invalid choice."); break;
            }
        }
        
        static void DisplayAccountDetails()
        {
            if (_currentAccount == null) return;
            Console.WriteLine("\n=== Account Details ===");
            Console.WriteLine(_currentAccount.GetAccountSummary());
            Console.ReadKey();
        }
        
        static void MakeDeposit()
        {
            if (_currentAccount == null) return;
            
            Console.Write("\nAmount: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Console.WriteLine("Invalid amount.");
                return;
            }
            
            Console.Write("PIN: ");
            string? pin = Console.ReadLine();
            
            if (_bank.Deposit(_currentAccount.Id, amount, pin ?? ""))
            {
                Console.WriteLine($"Deposited {amount:C}. New balance: {_currentAccount.Balance:C}");
            }
            else
            {
                Console.WriteLine("Deposit failed.");
            }
        }
        
        static void MakeWithdrawal()
        {
            if (_currentAccount == null) return;
            
            Console.Write("\nAmount: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Console.WriteLine("Invalid amount.");
                return;
            }
            
            Console.Write("PIN: ");
            string? pin = Console.ReadLine();
            
            if (_bank.Withdraw(_currentAccount.Id, amount, pin ?? ""))
            {
                Console.WriteLine($"Withdrew {amount:C}. New balance: {_currentAccount.Balance:C}");
            }
            else
            {
                Console.WriteLine("Withdrawal failed.");
            }
        }
        
        static void MakeTransfer()
        {
            if (_currentAccount == null) return;
            
            Console.Write("\nRecipient name: ");
            string? name = Console.ReadLine();
            var recipient = _bank.FindAccountByName(name ?? "");
            
            if (recipient == null)
            {
                Console.WriteLine("Recipient not found.");
                return;
            }
            
            Console.Write("Amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Console.WriteLine("Invalid amount.");
                return;
            }
            
            Console.Write("PIN: ");
            string? pin = Console.ReadLine();
            
            if (_bank.Transfer(_currentAccount.Id, recipient.Id, amount, pin ?? ""))
            {
                Console.WriteLine($"Transferred {amount:C} to {recipient.Name}");
                Console.WriteLine($"New balance: {_currentAccount.Balance:C}");
            }
            else
            {
                Console.WriteLine("Transfer failed.");
            }
        }
        
        static void ViewTransactionHistory()
        {
            if (_currentAccount == null) return;
            
            Console.WriteLine("\n1. All\n2. Filter by date");
            Console.Write("Choice: ");
            
            List<Transaction> transactions;
            if (Console.ReadLine() == "2")
            {
                Console.Write("Start date (yyyy-mm-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime start))
                {
                    Console.WriteLine("Invalid date.");
                    return;
                }
                
                Console.Write("End date (yyyy-mm-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime end))
                {
                    Console.WriteLine("Invalid date.");
                    return;
                }
                
                transactions = _currentAccount.GetTransactionHistory(start, end);
            }
            else
            {
                transactions = _currentAccount.GetTransactionHistory();
            }
            
            Console.WriteLine("\nDate | Type | Amount | Description");
            foreach (var t in transactions)
            {
                Console.WriteLine(t);
            }
            Console.ReadKey();
        }
        
        static void ChangePin()
        {
            if (_currentAccount == null) return;
            
            Console.Write("\nCurrent PIN: ");
            string? current = Console.ReadLine();
            
            Console.Write("New PIN: ");
            string? newPin = Console.ReadLine();
            
            if (_bank.ChangePin(_currentAccount.Id, current ?? "", newPin ?? ""))
            {
                Console.WriteLine("PIN changed.");
            }
            else
            {
                Console.WriteLine("Failed to change PIN.");
            }
        }
        
        static void ChangeAccountType()
        {
            if (_currentAccount == null) return;
            
            Console.WriteLine("\n1. Savings\n2. Cheque\n3. Business");
            Console.Write("New type: ");
            
            AccountType newType = Console.ReadLine() switch
            {
                "2" => AccountType.Cheque,
                "3" => AccountType.Business,
                _ => AccountType.Savings
            };
            
            Console.Write("PIN: ");
            string? pin = Console.ReadLine();
            
            if (_bank.ConvertAccountType(_currentAccount.Id, newType, pin ?? ""))
            {
                Console.WriteLine($"Account converted to {newType}.");
            }
            else
            {
                Console.WriteLine("Conversion failed.");
            }
        }
        
        static void CalculateInterest()
        {
            if (_currentAccount == null) return;
            
            decimal interest = _currentAccount.CalculateInterest();
            Console.WriteLine($"\nMonthly interest: {interest:C}");
            
            Console.Write("Apply interest? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                Console.Write("PIN: ");
                string? pin = Console.ReadLine();
                
                if (_currentAccount.ApplyMonthlyInterest(pin ?? ""))
                {
                    Console.WriteLine($"Interest applied. New balance: {_currentAccount.Balance:C}");
                }
            }
        }
        
        #endregion
    }
}