using CSharpBankingApp.Models;

namespace CSharpBankingApp.Services
{
    public class BankService
    {
        private readonly Bank _bank;

        public BankService()
        {
            _bank = new Bank();
        }

        public List<Account> GetAllAccounts()
        {
            return _bank.GetAllAccounts();
        }

        public Account? GetAccount(string id)
        {
            return _bank.GetAccount(id);
        }

        public Account? FindAccountByName(string name)
        {
            return _bank.FindAccountByName(name);
        }

        public Account? CreateAccount(string name, string pin, AccountType type)
        {
            return _bank.CreateAccount(name, pin, type);
        }

        public bool VerifyPin(string accountId, string pin)
        {
            return _bank.VerifyPin(accountId, pin);
        }

        public bool ChangePin(string accountId, string currentPin, string newPin)
        {
            return _bank.ChangePin(accountId, currentPin, newPin);
        }

        public bool Deposit(string accountId, decimal amount, string pin)
        {
            return _bank.Deposit(accountId, amount, pin);
        }

        public bool Withdraw(string accountId, decimal amount, string pin)
        {
            return _bank.Withdraw(accountId, amount, pin);
        }

        public bool Transfer(string fromAccountId, string toAccountId, decimal amount, string pin)
        {
            return _bank.Transfer(fromAccountId, toAccountId, amount, pin);
        }

        public bool ConvertAccountType(string accountId, AccountType newType, string pin)
        {
            return _bank.ConvertAccountType(accountId, newType, pin);
        }

        public List<string> GetAvailableBackups()
        {
            return _bank.GetAvailableBackups();
        }

        public void CreateBackup()
        {
            _bank.CreateBackup();
        }

        public bool RestoreFromBackup(string backupFileName)
        {
            return _bank.RestoreFromBackup(backupFileName);
        }

        public void Dispose()
        {
            _bank?.Dispose();
        }
    }
} 