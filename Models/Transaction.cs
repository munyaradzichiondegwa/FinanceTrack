// =========================================
// File: Models/Transaction.cs
// Purpose: Defines OOP classes for transactions using inheritance.
// Implements polymorphism with an abstract base class (Transaction) and derived classes (Income, Expense).
// Enum Category provides type-safe transaction categorization.
// =========================================

using System;
using System.Text.Json.Serialization;  

namespace FinanceTrack.Models  
{
    // Enum for categories - simple way to categorize transactions (uses integers under the hood).
    // Provides type safety vs. strings; can be extended to a struct if needed.
    public enum Category
    {
        Food,         
        Transport,    
        Salary,       
        Entertainment,
        Utilities,    
        Other         
    }

    // Abstract base class - cannot be instantiated directly; enforces common structure.
    // JSON polymorphism attributes specify how derived types are serialized/deserialized.
    [JsonDerivedType(typeof(Income), "income")]
    [JsonDerivedType(typeof(Expense), "expense")]
    public abstract class Transaction
    {
        // Common properties inherited by all transactions.
        public DateTime Date { get; set; }               // When the transaction occurred.
        public string Description { get; set; } = string.Empty; // Human-readable note (defaults to empty).
        public Category Category { get; set; }           // Categorized using enum.

        // Abstract property: Forces derived classes to implement their own Amount logic.
        // Enables polymorphism so Income/Expense can be treated uniformly in collections.
        public abstract decimal Amount { get; }

        // Protected constructor: Only accessible by derived classes; initializes shared fields.
        protected Transaction(DateTime date, string description, Category category)
        {
            Date = date;
            Description = description;
            Category = category;
        }

        // Abstract method: For type-specific display (e.g., "Income" vs. "Expense").
        // Overridden in derived classes for polymorphism.
        public abstract string GetTypeString();
    }

    // Derived class for income - inherits from Transaction.
    // Key OOP: Uses base constructor; overrides Amount (positive).
    public class Income : Transaction
    {
        // Backing field for amount (positive only).
        public decimal AmountField { get; set; }

        // Constructor: Calls base constructor; validates amount > 0.
        [JsonConstructor]  // Required for JSON deserialization in C# 12.0
        public Income(DateTime date, string description, Category category, decimal amount)
            : base(date, description, category)
        {
            if (amount <= 0)
                throw new ArgumentException("Income amount must be positive.");  // Early validation
            AmountField = amount;
        }

        // Override abstract property: Returns positive amount for calculations.
        public override decimal Amount => AmountField; // Expression-bodied property

        // Override for polymorphism: Returns type as string
        public override string GetTypeString() => "Income";
    }

    // Derived class for expenses - inherits and overrides for negative amounts.
    // Similar to Income, but Amount is negated for balance calculations.
    public class Expense : Transaction
    {
        // Backing field (positive input, negated in usage).
        public decimal AmountField { get; set; }

        // Constructor: Validates amount >= 0 (expenses can't be negative).
        [JsonConstructor]
        public Expense(DateTime date, string description, Category category, decimal amount)
            : base(date, description, category)
        {
            if (amount < 0)
                throw new ArgumentException("Expense amount cannot be negative.");
            AmountField = amount;
        }

        // Override abstract property: Negates the amount for balance purposes.
        public override decimal Amount => -AmountField; // Polymorphic behavior

        // Override for polymorphism: Returns type as string
        public override string GetTypeString() => "Expense";
    }
} // End of namespace FinanceTrack.Models
