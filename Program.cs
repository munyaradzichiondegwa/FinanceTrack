// =========================================
// File: Program.cs
// Purpose: Console app entry point with menu loop, input handling, and event subscription.
// Implemented during Saturday: Menu loop, events/delegates hookup.
// Key Learning: While loop for repetition; switch for choices; try-catch for user input.
// Delegates: Lambda subscribes to event (anonymous method).
// Builds on all prior: Calls Manager methods for add/view/I/O.
// =========================================

using FinanceTrack.Models;  // For Transaction types.
using FinanceTrack.Services;  // For FinanceManager.
using System;
using System.Linq;
using System.Reflection;

namespace FinanceTrack
{
    class Program
    {
        static void Main(string[] args)
        {
            // Main method: Entry point (traditional style with Program.Main)
            Console.WriteLine("Welcome to FinanceTrack - C# Console Edition!");
            Console.WriteLine("Track your income, expenses, and balances.\n");  // Variables for output.

            // Instantiate manager: Triggers load (Friday)
            var manager = new FinanceManager();

            // Event subscription: Uses lambda (delegate) as handler.
            // ?. for null safety; Beep() for simple audio alert.
            manager.LowBalanceAlert += (balance) =>
            {
                Console.Beep();  // System sound (conditional alert)
                Console.WriteLine($"\n🚨 LOW BALANCE ALERT! Current balance: ${balance:F2}. Time to earn more!\n");
                // Future: Could write to file or send email (advanced)
            };

            bool running = true;  // Boolean variable for loop control
            while (running)  // Loop: Repeats until exit (Saturday core)
            {
                DisplayMenu();  // Call helper method
                string? choice = Console.ReadLine()?.Trim();  // Read input; null-forgiving

                try  // Outer try-catch: Wraps switch for general errors
                {
                    switch (choice)  // Switch expression: Conditionals based on string input
                    {
                        case "1":
                            AddIncome(manager);  // Call helper (add with validation)
                            break;
                        case "2":
                            AddExpense(manager);
                            break;
                        case "3":
                            manager.DisplayAllTransactions();  // Thursday view
                            break;
                        case "4":
                            // Category view: Parse enum (conditionals)
                            Console.WriteLine("Enter category (0=Food, 1=Transport, 2=Salary, 3=Entertainment, 4=Utilities, 5=Other):");
                            if (Enum.TryParse<Category>(Console.ReadLine(), true, out Category cat))  // TryParse for safety
                            {
                                // LINQ filter (Thursday/Friday)
                                var trans = manager.GetTransactionsByCategory(cat);  // Add this method in FinanceManager
                                if (trans.Any())
                                {
                                    foreach (var t in trans.OrderBy(t => t.Date))  // Loop display
                                        Console.WriteLine($"{t.Date:yyyy-MM-dd} | {t.GetTypeString()} | ${t.Amount:F2} | {t.Description}");
                                }
                                else
                                {
                                    Console.WriteLine("No transactions in this category.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid category.");
                            }
                            break;
                        case "5":
                            manager.DisplayCategorySummary();  // LINQ summary
                            break;
                        case "6":
                            // Date range: Parse inputs (FormatException catch below)
                            Console.WriteLine("Enter start date (yyyy-MM-dd):");
                            DateTime start = DateTime.Parse(Console.ReadLine() ?? string.Empty);
                            Console.WriteLine("Enter end date (yyyy-MM-dd):");
                            DateTime end = DateTime.Parse(Console.ReadLine() ?? string.Empty);
                            // Add this method to Manager if expanding (LINQ Where)
                            var rangeTrans = manager.GetTransactionsByDateRange(start, end);  // Stub: Implement as needed
                            foreach (var t in rangeTrans)
                                Console.WriteLine($"{t.Date:yyyy-MM-dd} | {t.GetTypeString()} | ${t.Amount:F2} | {t.Description}");
                            break;
                        case "7":
                            Console.WriteLine($"Current Balance: ${manager.GetBalance():F2}");  // Simple calc
                            break;  // Fixed: must break to prevent fall-through
                        case "8":
                            running = false;  // Exit loop
                            try
                            {
                                // Attempt to invoke SaveTransactions even if it's non-public using reflection
                                var saveMethod = manager.GetType().GetMethod("SaveTransactions", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                                if (saveMethod != null)
                                {
                                    saveMethod.Invoke(manager, null);
                                    Console.WriteLine("Data saved. Goodbye!");
                                }
                                else
                                {
                                    Console.WriteLine("Save method not found; data was not saved.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Failed to save data: {ex.Message}");
                            }
                            break;  // Only one break needed
                        default:
                            Console.WriteLine("Invalid choice. Try again.");  // Default case
                            break;
                    }
                }
                catch (FormatException)  // Specific: Bad date/number input
                {
                    Console.WriteLine("Invalid input format. Please enter valid numbers or dates.");
                }
                catch (ArgumentException ex)  // From constructors (Wednesday/Thursday)
                {
                    Console.WriteLine($"Input error: {ex.Message}");
                }
                catch (Exception ex)  // General: I/O, etc. (Friday)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                Console.WriteLine();  // Newline spacer for readability
            }
        }

        // Helper method: Menu display (functions - modular code)
        static void DisplayMenu()
        {
            Console.WriteLine("Menu:");  // Static output
            Console.WriteLine("1. Add Income");
            Console.WriteLine("2. Add Expense");
            Console.WriteLine("3. View All Transactions");
            Console.WriteLine("4. View by Category");
            Console.WriteLine("5. View Category Summary & Balance");
            Console.WriteLine("6. View by Date Range");
            Console.WriteLine("7. View Current Balance");
            Console.WriteLine("8. Exit");
            Console.Write("Choose an option: ");
        }

        // Helper: Add income with input loop/validation (loops, conditionals)
        static void AddIncome(FinanceManager manager)
        {
            try
            {
                Console.Write("Description: ");
                string desc = Console.ReadLine() ?? string.Empty;  // Null coalesce

                Console.Write("Amount: $");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
                    throw new ArgumentException("Amount must be a positive number.");

                Console.WriteLine("Category (0=Food, 1=Transport, 2=Salary, 3=Entertainment, 4=Utilities, 5=Other):");
                if (!Enum.TryParse<Category>(Console.ReadLine(), true, out Category cat))
                    throw new ArgumentException("Invalid category.");

                // Create polymorphic object; add via manager (Thursday)
                var income = new Income(DateTime.Now, desc, cat, amount);  // Constructor call
                manager.AddTransaction(income);  // Triggers save/event
                Console.WriteLine("Income added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add income: {ex.Message}");
            }
        }

        // Similar helper for expense (reuse pattern)
        static void AddExpense(FinanceManager manager)
        {
            try
            {
                Console.Write("Description: ");
                string desc = Console.ReadLine() ?? string.Empty;

                Console.Write("Amount: $");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount < 0)
                    throw new ArgumentException("Amount must be non-negative.");

                Console.WriteLine("Category (0=Food, 1=Transport, 2=Salary, 3=Entertainment, 4=Utilities, 5=Other):");
                if (!Enum.TryParse<Category>(Console.ReadLine(), true, out Category cat))
                    throw new ArgumentException("Invalid category.");

                var expense = new Expense(DateTime.Now, desc, cat, amount);
                manager.AddTransaction(expense);
                Console.WriteLine("Expense added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add expense: {ex.Message}");
            }
        }
    }
}
