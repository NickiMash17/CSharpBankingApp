using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpBankingApp
{
    public class Bank
    {
        private List<Account> _accounts = new List<Account>();
        
        public string Name { get; }

        public Bank(string name)
        {
            this.Name = name;
        }

        public Account CreateAccount(string ownerName, decimal initialDeposit)
        {
            var account = new Account(ownerName, initialDeposit);
            _accounts.Add(account);
            return account;
        }

        public Account GetAccount(string accountNumber)
        {
            return _accounts.FirstOrDefault(a => a.Number == accountNumber);
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return _accounts.AsReadOnly();
        }
    }
}