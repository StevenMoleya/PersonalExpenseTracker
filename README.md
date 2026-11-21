# Personal Expense Tracker

A simple C# console application for managing personal income and expenses. The application allows users to record transactions, monitor spending and calculate their available balance.

## Features

- Record income transactions.
- Record expense transactions.
- Categorise transactions.
- View transaction history.
- View total income, expenses and current balance.
- Calculate monthly expenditure.
- Delete transactions.
- Save and load transaction data using JSON.
- Validate user input.

## Technologies Used

- C#
- .NET 10
- JSON
- xUnit

## Project Structure

```text
PersonalExpenseTracker/
│
├── Program.cs
├── Transaction.cs
├── TransactionManager.cs
├── PersonalExpenseTracker.csproj
└── README.md
```

## Requirements

- .NET 10 SDK
- Visual Studio Code or Visual Studio

## How to Run

1. Clone or download the repository.
2. Open the project directory in your terminal.
3. Restore dependencies:

```bash
dotnet restore
```

4. Run the application:

```bash
dotnet run
```

## Running Tests

The project includes automated unit tests using xUnit.

Navigate to the test project directory and execute:

```bash
dotnet test
```

## Data Storage

Transaction information is stored locally in a JSON file. This allows recorded transactions to remain available when the application is closed and reopened.

## Purpose

This project was developed to practise C# programming, object-oriented programming, data management, file handling and automated testing.

## Author

**Steven Moleya**
