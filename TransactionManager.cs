
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PersonalExpenseTracker
{
    public class TransactionManager
    {
        private readonly List<Transaction> transactions = new();

        private readonly string dataDirectory =
            Path.Combine(AppContext.BaseDirectory, "data");

        private string DataFile =>
            Path.Combine(dataDirectory, "transactions.json");

        public void AddTransaction(Transaction transaction)
        {
            transaction.Id = transactions.Count > 0
                ? transactions.Max(t => t.Id) + 1
                : 1;

            transactions.Add(transaction);
            SaveTransactions();
        }

        public List<Transaction> GetTransactions()
        {
            return transactions;
        }

        public bool DeleteTransaction(int id)
        {
            Transaction? transaction =
                transactions.FirstOrDefault(t => t.Id == id);

            if (transaction == null)
            {
                return false;
            }

            transactions.Remove(transaction);
            SaveTransactions();

            return true;
        }

        public decimal GetTotalIncome()
        {
            return transactions
                .Where(t => t.Type == "Income")
                .Sum(t => t.Amount);
        }

        public decimal GetTotalExpenses()
        {
            return transactions
                .Where(t => t.Type == "Expense")
                .Sum(t => t.Amount);
        }

        public decimal GetBalance()
        {
            return GetTotalIncome() - GetTotalExpenses();
        }

        public decimal GetMonthlyExpenses(int year, int month)
        {
            return transactions
                .Where(t => t.Type == "Expense"
                    && t.Date.Year == year
                    && t.Date.Month == month)
                .Sum(t => t.Amount);
        }

        public void LoadTransactions()
        {
            try
            {
                if (!File.Exists(DataFile))
                {
                    return;
                }

                string json = File.ReadAllText(DataFile);

                List<Transaction>? loaded =
                    JsonSerializer.Deserialize<List<Transaction>>(json);

                transactions.Clear();

                if (loaded != null)
                {
                    transactions.AddRange(loaded);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading transactions: {ex.Message}");
            }
        }

        public void SaveTransactions()
        {
            try
            {
                Directory.CreateDirectory(dataDirectory);

                string json = JsonSerializer.Serialize(
                    transactions,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                File.WriteAllText(DataFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving transactions: {ex.Message}");
            }
        }
    }
}
