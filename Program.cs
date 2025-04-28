using System;
using System.Collections.Generic;

namespace CSharpBankingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var bank = new Bank("Simple Bank");
            
            Console.WriteLine("Welcome to Simple Banking App!");
            Console.WriteLine("----------------------------");
            
            while (true)
            {
                Console.WriteLine("\nMain Menu:");
                Console.WriteLine("1. Create Account");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. Check Balance");
                Console.WriteLine("5. View Transactions");
                Console.WriteLine("6. View All Accounts");
                Console.WriteLine("7. Exit");
                
                Console.Write("Enter your choice (1-7): ");
                var choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        CreateAccount(bank);
                        break;
                    case "2":
                        Deposit(bank);
                        break;
                    case "3":
                        Withdraw(bank);
                        break;
                    case "4":
                        CheckBalance(bank);
                        break;
                    case "5":
                        ViewTransactions(bank);
                        break;
                    case "6":
                        ViewAllAccounts(bank);
                        break;
                    case "7":
                        Console.WriteLine("Thank you for using Simple Banking App!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void CreateAccount(Bank bank)
        {
            Console.Write("Enter your name: ");
            var name = Console.ReadLine();
            
            Console.Write("Enter initial deposit amount: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                var account = bank.CreateAccount(name, amount);
                Console.WriteLine($"Account created successfully! Account Number: {account.Number}");
            }
            else
            {
                Console.WriteLine("Invalid amount. Please try again.");
            }
        }

        static void Deposit(Bank bank)
        {
            Console.Write("Enter account number: ");
            var accountNumber = Console.ReadLine();
            
            var account = bank.GetAccount(accountNumber);
            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }
            
            Console.Write("Enter deposit amount: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                try
                {
                    account.MakeDeposit(amount, DateTime.Now, "Deposit via console");
                    Console.WriteLine($"Deposit successful. New balance: {account.Balance:C}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid amount. Please try again.");
            }
        }

        static void Withdraw(Bank bank)
        {
            Console.Write("Enter account number: ");
            var accountNumber = Console.ReadLine();
            
            var account = bank.GetAccount(accountNumber);
            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }
            
            Console.Write("Enter withdrawal amount: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                try
                {
                    account.MakeWithdrawal(amount, DateTime.Now, "Withdrawal via console");
                    Console.WriteLine($"Withdrawal successful. New balance: {account.Balance:C}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid amount. Please try again.");
            }
        }

        static void CheckBalance(Bank bank)
        {
            Console.Write("Enter account number: ");
            var accountNumber = Console.ReadLine();
            
            var account = bank.GetAccount(accountNumber);
            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }
            
            Console.WriteLine($"Account Balance: {account.Balance:C}");
        }

        static void ViewTransactions(Bank bank)
        {
            Console.Write("Enter account number: ");
            var accountNumber = Console.ReadLine();
            
            var account = bank.GetAccount(accountNumber);
            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }
            
            Console.WriteLine("\nTransaction History:");
            Console.WriteLine(account.GetAccountHistory());
        }

        static void ViewAllAccounts(Bank bank)
        {
            Console.WriteLine("\nAll Accounts:");
            foreach (var account in bank.GetAllAccounts())
            {
                Console.WriteLine($"Account#: {account.Number}, Owner: {account.Owner}, Balance: {account.Balance:C}");
            }
        }
    }
}