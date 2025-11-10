# FinanceTrack - C# Console Personal Finance Tracker

## Project Overview
**FinanceTrack** is a console-based personal finance tracking application written in C# using **.NET 8.0** (supporting C# 12.0 features). It fulfills the requirements of the **CSE 310 – Applied Programming** module (Language – C#). The app allows users to:

- Manage income and expenses (transactions)
- View categorized summaries
- Calculate running balances
- Persist data to a JSON file across sessions

This project builds on a previous web-based finance tracker (HTML, CSS, JavaScript, Vite) but emphasizes C# console application strengths:

- Object-Oriented Programming (OOP) with inheritance
- LINQ for data querying
- File I/O for persistence
- Exception handling for robust input validation
- Events/delegates for notifications (e.g., low-balance alerts)

It demonstrates core C# elements (variables, expressions, conditionals, loops, functions, structs/enums) and advanced features like polymorphic serialization.

**Repository:** [FinanceTrack GitHub](https://github.com/munyaradzichiondegwa/FinanceTrack)  
**Module:** Language – C# (Module 1)  
**Author:** B. Munyaradzi Chiondegwa  
**Date:** 31 October 2025 (Sprint Completion)  
**Total Effort:** ~21 hours  
**License:** MIT  

The app is an MVP, with potential future extensions such as async I/O, unit tests (xUnit), or GUI integration (Blazor).

---

## Features

### Transaction Management
- Add **Income** (positive amounts) or **Expense** (negative impact on balance)
- Include **Description**, **Category** (enum: Food, Transport, Salary, etc.), and **Timestamp**

### Viewing and Summaries
- Display all transactions (sorted by date)
- Filter by category or date range using LINQ
- Categorized summaries and overall balance calculation

### Data Persistence
- Auto saves/loads transactions to/from `transactions.json` using `System.Text.Json`
- Supports polymorphic types (Income/Expense)

### Notifications
- Low-balance alerts triggered via events/delegates if balance < $100

### Error Handling
- Validates inputs (positive income, valid categories/dates)
- Handles file I/O, JSON parsing, and user errors

### Console UI
- Menu-driven interface using loops and switch statements

### OOP Design
- Abstract `Transaction` class with `Income` and `Expense` derived classes
- Polymorphic `Amount` property and serialization support

### Key Learning Outcomes
**Basic:** Variables, expressions, conditionals, loops, methods, enums  
**Advanced:** OOP, LINQ (`Sum`, `Where`, `OrderBy`, `ToDictionary`), delegates/events, file I/O, exception handling

---

## Requirements
- **.NET SDK:** 8.0 or later  
- **IDE/Editor:** VS Code (with C# extension) or Visual Studio Community  
- **OS:** Cross-platform (Windows, macOS, Linux)  
- **Dependencies:** Only built-in .NET libraries  
- **Hardware:** Standard (console app)

---

## Installation

```bash
git clone https://github.com/munyaradzichiondegwa/FinanceTrack.git
cd FinanceTrack

Install .NET SDK (if needed):
Download .NET 8.0

Verify: dotnet --version (should show 8.0.x or higher)

Open in VS Code (install C# extension for IntelliSense & debugging)

Build the Project:

dotnet build

Project file FinanceTrack.csproj:

<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <LangVersion>12.0</LangVersion>
  </PropertyGroup>
</Project>

Run the App:

dotnet run

Loads transactions.json if present

Data persists in project root (add to .gitignore if desired)

Usage

Menu Navigation (choose options 1–8):

Add Income – Enter description, positive amount, category

Add Expense – Enter description, non-negative amount (treated as negative in balance)

View All Transactions – Sorted newest first

View by Category – Filter by category

View Category Summary & Balance – Totals per category + overall balance

View by Date Range – Filter transactions by date

View Current Balance – Quick total

Exit – Saves data and quits

Example Session:

Add Income: Salary, 1000 → Balance $1000

Add Expense: Dinner, 75 → Balance $925

View Summary: Food -$75, Salary $1000, Total $925

Low-balance alert triggers if below $100

Edge Cases Handled:

Invalid inputs (negative income, wrong date format)

Empty data

Corrupt JSON resets gracefully

Code Structure
Models (Transaction.cs)

Category enum: Defines categories

Transaction (abstract): Base class with Date, Description, Category

Income/Expense: Derived classes override Amount, support polymorphism & JSON serialization

Services (FinanceManager.cs)

Handles business logic: add, display, filter, summaries

LINQ for queries, events for low-balance alerts

Persistence via JsonSerializer with try-catch

Program (Program.cs)

Entry point & menu UI

Loops and switch statements for user interaction

Delegates for flexible event handling

Build & Run Flow: Program → Manager → Transactions → LINQ queries → Console output → Save on exit

Total Lines: ~450

Potential Improvements

Unit tests with xUnit

Async file I/O

User-defined categories

Export to CSV/PDF

Video Demo

Duration: 4–5 minutes

Demonstrates live run, summaries, low-balance alert, JSON persistence

Highlights OOP, LINQ, menu/events

Video Link: FinanceTrack Demo

Challenges & Lessons Learned

LINQ & Events: Learned declarative queries and event-driven notifications

File I/O & JSON: Polymorphic serialization required C# 12.0 features

Scope Management: Console UI simplified focus on core logic

Exception Handling: Prevented crashes from invalid data

Total Time: 21 hours
Outcome: Strong C# proficiency; solid backend portfolio project

Contributions & Contact

Fork & submit PRs for improvements

Email: bchiondegwa@ybyupathway.edu

Peer reviews encouraged 

Last Updated: 10 November 2025
Powered by: .NET 8.0 | C# 12.0