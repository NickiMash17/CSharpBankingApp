using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpBankingApp
{
    public class Bank
    {
        private readonly List<Account> _accounts = new List<Account>();
        
        public string Name { get; }

        public Bank(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public Account CreateAccount(string ownerName, decimal initialDeposit)
        {
            if (string.IsNullOrWhiteSpace(ownerName))
                throw new ArgumentException("Owner name cannot be empty", nameof(ownerName));

            var account = new Account(ownerName, initialDeposit);
            _accounts.Add(account);
            return account;
        }

        public Account? GetAccount(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return null;

            return _accounts.FirstOrDefault(a => a.Number == accountNumber);
        }

        public IReadOnlyList<Account> GetAllAccounts() => _accounts.AsReadOnly();
    }
}