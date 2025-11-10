// =========================================
// File: Services/FinanceManager.cs
// Purpose: Manages transactions, including adding, viewing, summaries, 
//          persistence via JSON, and low-balance alerts via events.
// Key Learning: Polymorphism, List<T>, LINQ, delegates/events, file I/O, exception handling.
// =========================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FinanceTrack.Models;  // Import Transaction, Income, Expense, Category

namespace FinanceTrack.Services
{
    // Delegate type for low-balance event handlers
    public delegate void LowBalanceHandler(decimal balance);

    public class FinanceManager
    {
        // Polymorphic list of transactions (Income or Expense)
        private List<Transaction> _transactions = new();

        // Low-balance threshold constant
        private const decimal LowBalanceThreshold = 100.0m;

        // File path for JSON persistence
        private const string DataFile = "transactions.json";

        // Event triggered when balance falls below threshold
        public event LowBalanceHandler? LowBalanceAlert;

        // Constructor: loads transactions from JSON
        public FinanceManager()
        {
            LoadTransactions();  // Ensures data persists across runs
        }

        // Add a transaction, save to JSON, and check for low balance
        public void AddTransaction(Transaction transaction)
        {
            try
            {
                _transactions.Add(transaction);  // Add to collection
                SaveTransactions();              // Persist data
                CheckBalance();                  // Trigger low-balance event if needed
                Console.WriteLine("Transaction added!");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to add transaction: {ex.Message}");
            }
        }

        // Display all transactions, ordered by date descending
        public void DisplayAllTransactions()
        {
            if (!_transactions.Any())
            {
                Console.WriteLine("No transactions yet.");
                return;
            }

            foreach (var trans in _transactions.OrderByDescending(t => t.Date))
            {
                Console.WriteLine($"{trans.Date:yyyy-MM-dd} | {trans.GetTypeString()} | ${trans.Amount:F2} | {trans.Description} | {trans.Category}");
            }
        }

        // Compute current total balance
        public decimal GetBalance() => _transactions.Sum(t => t.Amount);

        // Get a summary of totals per category
        public Dictionary<Category, decimal> GetCategorySummary() =>
            Enum.GetValues<Category>()
                .ToDictionary(
                    cat => cat,
                    cat => _transactions
                                .Where(t => t.Category == cat)
                                .Sum(t => t.Amount)
                );

        // Display category summary and overall balance
        public void DisplayCategorySummary()
        {
            var summary = GetCategorySummary();
            foreach (var kvp in summary)
            {
                Console.WriteLine($"{kvp.Key}: ${kvp.Value:F2}");
            }
            Console.WriteLine($"\nTotal Balance: ${GetBalance():F2}");
        }

        // Get transactions filtered by category
        public List<Transaction> GetTransactionsByCategory(Category category) =>
            _transactions.Where(t => t.Category == category).OrderBy(t => t.Date).ToList();

        // Get transactions filtered by date range (inclusive)
        public List<Transaction> GetTransactionsByDateRange(DateTime start, DateTime end) =>
            _transactions.Where(t => t.Date >= start && t.Date <= end)
                         .OrderBy(t => t.Date)
                         .ToList();

        // Save transactions to JSON file
        private void SaveTransactions()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_transactions, options);
                File.WriteAllText(DataFile, json);
                Console.WriteLine($"// Saved {_transactions.Count} transactions to {DataFile}.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save data: {ex.Message}");
            }
        }

        // Load transactions from JSON file
        private void LoadTransactions()
        {
            try
            {
                if (File.Exists(DataFile))
                {
                    string json = File.ReadAllText(DataFile);
                    _transactions = JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
                    Console.WriteLine($"// Loaded {_transactions.Count} transactions from {DataFile}.");
                }
                else
                {
                    Console.WriteLine("// No data file found - starting fresh.");
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Warning: Invalid data file. Starting fresh. Error: {ex.Message}");
                _transactions = new List<Transaction>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load data: {ex.Message}");
            }
        }

        // Check current balance and invoke low-balance alert if below threshold
        private void CheckBalance()
        {
            decimal balance = GetBalance();
            if (balance < LowBalanceThreshold)
            {
                LowBalanceAlert?.Invoke(balance);  // Safe invocation using null-conditional operator
            }
        }
    }
}
