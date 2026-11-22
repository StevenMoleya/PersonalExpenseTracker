
using System;
using System.IO;
using PersonalExpenseTracker;
using Xunit;

namespace PersonalExpenseTracker.Tests;

public class TransactionTests : IDisposable
{
    private readonly string testDataDirectory;

    public TransactionTests()
    {
        testDataDirectory = Path.Combine(
            Path.GetTempPath(),
            "PersonalExpenseTracker.Tests",
            Guid.NewGuid().ToString("N"));
    }

    private TransactionManager CreateManager()
    {
        return new TransactionManager(testDataDirectory);
    }

    public void Dispose()
    {
        if (Directory.Exists(testDataDirectory))
        {
            Directory.Delete(testDataDirectory, true);
        }
    }

    [Fact]
    public void Constructor_ShouldSetTransactionProperties()
    {
        var date = new DateTime(2026, 11, 19);

        var transaction = new Transaction(
            1, "Income", "Salary", 15000m, date, "November salary"
        );

        Assert.Equal(1, transaction.Id);
        Assert.Equal("Income", transaction.Type);
        Assert.Equal("Salary", transaction.Category);
        Assert.Equal(15000m, transaction.Amount);
        Assert.Equal(date, transaction.Date);
        Assert.Equal("November salary", transaction.Description);
    }

    [Fact]
    public void TransactionProperties_ShouldBeUpdatable()
    {
        var transaction = new Transaction(
            1, "Expense", "Food", 500m,
            DateTime.Now, "Groceries"
        );

        transaction.Amount = 750m;
        transaction.Category = "Transport";

        Assert.Equal(750m, transaction.Amount);
        Assert.Equal("Transport", transaction.Category);
    }

    [Fact]
    public void AddTransaction_ShouldAddTransactionAndAssignId()
    {
        var manager = CreateManager();

        var transaction = new Transaction(
            0, "Income", "Salary", 15000m,
            DateTime.Now, "Monthly salary"
        );

        manager.AddTransaction(transaction);

        Assert.Single(manager.GetTransactions());
        Assert.Equal(1, manager.GetTransactions()[0].Id);
    }

    [Fact]
    public void FinancialSummary_ShouldCalculateCorrectTotals()
    {
        var manager = CreateManager();

        manager.AddTransaction(new Transaction(
            0, "Income", "Salary", 15000m,
            DateTime.Now, "Salary"
        ));

        manager.AddTransaction(new Transaction(
            0, "Expense", "Food", 5000m,
            DateTime.Now, "Groceries"
        ));

        Assert.Equal(15000m, manager.GetTotalIncome());
        Assert.Equal(5000m, manager.GetTotalExpenses());
        Assert.Equal(10000m, manager.GetBalance());
    }

    [Fact]
    public void MonthlyExpenses_ShouldCalculateCorrectTotal()
    {
        var manager = CreateManager();

        manager.AddTransaction(new Transaction(
            0, "Expense", "Food", 1200m,
            new DateTime(2026, 11, 10), "Groceries"
        ));

        manager.AddTransaction(new Transaction(
            0, "Expense", "Transport", 800m,
            new DateTime(2026, 10, 10), "Fuel"
        ));

        Assert.Equal(1200m, manager.GetMonthlyExpenses(2026, 11));
    }

    [Fact]
    public void DeleteTransaction_ShouldRemoveTransaction()
    {
        var manager = CreateManager();

        manager.AddTransaction(new Transaction(
            0, "Expense", "Food", 500m,
            DateTime.Now, "Groceries"
        ));

        Assert.True(manager.DeleteTransaction(1));
        Assert.Empty(manager.GetTransactions());
        Assert.False(manager.DeleteTransaction(1));
    }

    [Fact]
    public void Transactions_ShouldPersistAfterLoading()
    {
        var manager = CreateManager();

        manager.AddTransaction(new Transaction(
            0, "Income", "Salary", 15000m,
            DateTime.Now, "Monthly salary"
        ));

        var secondManager = CreateManager();
        secondManager.LoadTransactions();

        Assert.Single(secondManager.GetTransactions());
        Assert.Equal(15000m, secondManager.GetTransactions()[0].Amount);
    }
}
