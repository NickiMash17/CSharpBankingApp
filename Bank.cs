using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace CSharpBankingApp
{
    public class Bank : IDisposable
    {
        private List<Account> _accounts;
        private readonly object _lockObject = new object();
        private readonly string _dataFilePath;
        private readonly string _backupDirectory;
        private Timer _autoSaveTimer;
        
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public Bank(string dataFilePath = "bankdata.json", string backupDirectory = "backups")
        {
            _dataFilePath = dataFilePath;
            _backupDirectory = backupDirectory;
            _accounts = new List<Account>();
            
            if (!Directory.Exists(_backupDirectory))
                Directory.CreateDirectory(_backupDirectory);
            
            LoadAccounts();
            _autoSaveTimer = new Timer(AutoSave, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
        }

        public Account? CreateAccount(string name, string pin, AccountType type = AccountType.Savings)
        {
            try
            {
                var account = new Account(name, pin, type);
                
                lock (_lockObject)
                {
                    _accounts.Add(account);
                    SaveAccounts();
                }
                
                return account;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error creating account: {ex.Message}");
                return null;
            }
        }

        public Account? GetAccount(string id)
        {
            lock (_lockObject)
            {
                return _accounts.FirstOrDefault(a => a.Id == id);
            }
        }

        public Account? FindAccountByName(string name)
        {
            lock (_lockObject)
            {
                return _accounts.FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }
        }

        public List<Account> GetAllAccounts()
        {
            lock (_lockObject)
            {
                return _accounts.ToList();
            }
        }

        public bool VerifyPin(string accountId, string pin)
        {
            var account = GetAccount(accountId);
            return account?.ValidatePin(pin) ?? false;
        }
        
        public bool ChangePin(string accountId, string currentPin, string newPin)
        {
            var account = GetAccount(accountId);
            if (account == null)
                return false;
            
            bool success = account.ChangePin(currentPin, newPin);
            if (success)
                SaveAccounts();
            
            return success;
        }
        
        public bool Deposit(string accountId, decimal amount, string pin)
        {
            var account = GetAccount(accountId);
            if (account == null)
                return false;
            
            bool success = account.Deposit(amount, pin);
            if (success)
                SaveAccounts();
            
            return success;
        }

        public bool Withdraw(string accountId, decimal amount, string pin)
        {
            var account = GetAccount(accountId);
            if (account == null)
                return false;
            
            bool success = account.Withdraw(amount, pin);
            if (success)
                SaveAccounts();
            
            return success;
        }

        public bool Transfer(string fromAccountId, string toAccountId, decimal amount, string pin)
        {
            var fromAccount = GetAccount(fromAccountId);
            var toAccount = GetAccount(toAccountId);
            
            if (fromAccount == null || toAccount == null)
                return false;
            
            if (!fromAccount.Withdraw(amount, pin))
                return false;
            
            if (!toAccount.Deposit(amount, pin))
            {
                fromAccount.Deposit(amount, pin);
                return false;
            }
            
            SaveAccounts();
            return true;
        }
        
        public bool ConvertAccountType(string accountId, AccountType newType, string pin)
        {
            var account = GetAccount(accountId);
            if (account == null)
                return false;
            
            bool success = account.ConvertAccountType(newType, pin);
            if (success)
                SaveAccounts();
            
            return success;
        }
        
        public void ApplyInterestToAllAccounts()
        {
            lock (_lockObject)
            {
                bool anyInterestApplied = false;
                
                foreach (var account in _accounts)
                {
                    if (account.CalculateInterest() > 0)
                    {
                        account.ApplyMonthlyInterest("0000");
                        anyInterestApplied = true;
                    }
                }
                
                if (anyInterestApplied)
                    SaveAccounts();
            }
        }

        private void LoadAccounts()
        {
            try
            {
                if (File.Exists(_dataFilePath))
                {
                    string json = File.ReadAllText(_dataFilePath);
                    var accounts = JsonSerializer.Deserialize<List<Account>>(json, _jsonOptions);
                    if (accounts != null)
                    {
                        lock (_lockObject)
                        {
                            _accounts = accounts;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading accounts: {ex.Message}");
                TryLoadFromBackup();
            }
        }

        private void SaveAccounts()
        {
            try
            {
                lock (_lockObject)
                {
                    string json = JsonSerializer.Serialize(_accounts, _jsonOptions);
                    File.WriteAllText(_dataFilePath, json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving accounts: {ex.Message}");
            }
        }
        
        private void AutoSave(object? state)
        {
            try
            {
                SaveAccounts();
                CreateBackup();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during auto-save: {ex.Message}");
            }
        }

        public void CreateBackup()
        {
            try
            {
                string backupFileName = $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                string backupPath = Path.Combine(_backupDirectory, backupFileName);
                
                lock (_lockObject)
                {
                    string json = JsonSerializer.Serialize(_accounts, _jsonOptions);
                    File.WriteAllText(backupPath, json);
                }
                
                CleanupOldBackups();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating backup: {ex.Message}");
            }
        }
        
        private void CleanupOldBackups()
        {
            try
            {
                var backupFiles = Directory.GetFiles(_backupDirectory)
                    .OrderByDescending(f => File.GetLastWriteTime(f))
                    .Skip(10)
                    .ToList();
                
                foreach (var file in backupFiles)
                {
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cleaning up old backups: {ex.Message}");
            }
        }
        
        public List<string> GetAvailableBackups()
        {
            try
            {
                return Directory.GetFiles(_backupDirectory)
                    .Select(Path.GetFileName)
                    .OrderByDescending(f => f)
                    .ToList()!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting available backups: {ex.Message}");
                return new List<string>();
            }
        }
        
        public bool RestoreFromBackup(string backupFileName)
        {
            try
            {
                string backupPath = Path.Combine(_backupDirectory, backupFileName);
                if (!File.Exists(backupPath))
                    return false;
                
                string json = File.ReadAllText(backupPath);
                var accounts = JsonSerializer.Deserialize<List<Account>>(json, _jsonOptions);
                
                if (accounts != null)
                {
                    lock (_lockObject)
                    {
                        _accounts = accounts;
                        SaveAccounts();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error restoring from backup: {ex.Message}");
                return false;
            }
        }
        
        private bool TryLoadFromBackup()
        {
            try
            {
                var backups = GetAvailableBackups();
                if (backups.Count > 0)
                {
                    return RestoreFromBackup(backups[0]);
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        
        public void Dispose()
        {
            _autoSaveTimer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}