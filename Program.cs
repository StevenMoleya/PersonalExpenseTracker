
using System;
using System.Globalization;
using System.Linq;
using PersonalExpenseTracker;

TransactionManager manager = new TransactionManager();

manager.LoadTransactions();

bool running = true;

while (running)
{
    Console.Clear();

    Console.WriteLine("====================================");
    Console.WriteLine("       PERSONAL EXPENSE TRACKER");
    Console.WriteLine("====================================");
    Console.WriteLine("1. Add Income");
    Console.WriteLine("2. Add Expense");
    Console.WriteLine("3. View Transactions");
    Console.WriteLine("4. View Financial Summary");
    Console.WriteLine("5. View Monthly Expenditure");
    Console.WriteLine("6. Delete Transaction");
    Console.WriteLine("7. Exit");
    Console.WriteLine("====================================");
    Console.Write("Select an option: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            AddTransaction("Income");
            break;

        case "2":
            AddTransaction("Expense");
            break;

        case "3":
            ViewTransactions();
            break;

        case "4":
            ViewFinancialSummary();
            break;

        case "5":
            ViewMonthlyExpenditure();
            break;

        case "6":
            DeleteTransaction();
            break;

        case "7":
            running = false;
            Console.WriteLine("Thank you for using Personal Expense Tracker!");
            break;

        default:
            Console.WriteLine("Invalid option. Please select 1-7.");
            break;
    }

    if (running)
    {
        Console.WriteLine("\nPress Enter to return to the menu...");
        Console.ReadLine();
    }
}

void AddTransaction(string type)
{
    Console.WriteLine($"\n===== ADD {type.ToUpper()} =====");

    Console.Write("Enter category: ");
    string category = Console.ReadLine()?.Trim() ?? "";

    if (string.IsNullOrWhiteSpace(category))
    {
        Console.WriteLine("Category cannot be empty.");
        return;
    }

    Console.Write("Enter amount (R): ");

    if (!decimal.TryParse(
        Console.ReadLine(),
        NumberStyles.Number,
        CultureInfo.CurrentCulture,
        out decimal amount) || amount <= 0)
    {
        Console.WriteLine("Please enter a valid positive amount.");
        return;
    }

    Console.Write("Enter description: ");
    string description = Console.ReadLine()?.Trim() ?? "";

    if (string.IsNullOrWhiteSpace(description))
    {
        Console.WriteLine("Description cannot be empty.");
        return;
    }

    Transaction transaction = new Transaction(
        0,
        type,
        category,
        amount,
        DateTime.Now,
        description
    );

    manager.AddTransaction(transaction);

    Console.WriteLine($"{type} recorded successfully!");
}

void ViewTransactions()
{
    Console.WriteLine("\n===== TRANSACTION HISTORY =====");

    var transactions = manager.GetTransactions();

    if (transactions.Count == 0)
    {
        Console.WriteLine("No transactions recorded yet.");
        return;
    }

    foreach (Transaction transaction in transactions.OrderBy(t => t.Date))
    {
        DisplayTransaction(transaction);
    }
}

void DisplayTransaction(Transaction transaction)
{
    Console.WriteLine("------------------------------------");
    Console.WriteLine($"ID:          {transaction.Id}");
    Console.WriteLine($"Type:        {transaction.Type}");
    Console.WriteLine($"Category:    {transaction.Category}");
    Console.WriteLine($"Amount:      R{transaction.Amount:F2}");
    Console.WriteLine($"Date:        {transaction.Date:dd/MM/yyyy HH:mm}");
    Console.WriteLine($"Description: {transaction.Description}");
}

void ViewFinancialSummary()
{
    Console.WriteLine("\n===== FINANCIAL SUMMARY =====");

    Console.WriteLine($"Total Income:   R{manager.GetTotalIncome():F2}");
    Console.WriteLine($"Total Expenses: R{manager.GetTotalExpenses():F2}");
    Console.WriteLine($"Balance:        R{manager.GetBalance():F2}");
}

void ViewMonthlyExpenditure()
{
    Console.Write("\nEnter month (1-12): ");

    if (!int.TryParse(Console.ReadLine(), out int month) ||
        month < 1 || month > 12)
    {
        Console.WriteLine("Invalid month.");
        return;
    }

    Console.Write("Enter year: ");

    if (!int.TryParse(Console.ReadLine(), out int year) ||
        year < 1 || year > 9999)
    {
        Console.WriteLine("Invalid year.");
        return;
    }

    decimal monthlyExpenses = manager.GetMonthlyExpenses(year, month);

    Console.WriteLine($"\n===== EXPENDITURE FOR {month:D2}/{year} =====");
    Console.WriteLine($"Total Expenses: R{monthlyExpenses:F2}");
}

void DeleteTransaction()
{
    Console.Write("\nEnter the transaction ID to delete: ");

    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Invalid transaction ID.");
        return;
    }

    bool deleted = manager.DeleteTransaction(id);

    if (!deleted)
    {
        Console.WriteLine("Transaction not found.");
        return;
    }

    Console.WriteLine("Transaction deleted successfully.");
}
