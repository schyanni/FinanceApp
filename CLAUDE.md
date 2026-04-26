# CLAUDE.md

This file provides guidance to Claude Code when working with code in this repository.

## Project Overview

A WPF desktop application (C# / .NET 10, Windows-only) for personal finance tracking — recording expenses and income in CHF. UI text is in German. There are no tests.

## Build & Run

```bash
# Build
dotnet build FinancingApp/FinancingApp.csproj

# Run
dotnet run --project FinancingApp/FinancingApp.csproj
```

The solution file uses the modern `.slnx` format (`FinancingApp.slnx`).

## Architecture

**Pattern**: MVVM with Dependency Injection (CommunityToolkit.Mvvm + Microsoft.Extensions.DependencyInjection).

```text
View (XAML/code-behind)
  └─ ViewModel (ObservableObject, RelayCommand/AsyncRelayCommand)
       └─ Service (business logic + observable data)
            └─ DbContext (EF Core / SQLite → financing.db)
```

All services and ViewModels are registered as singletons in `App.OnStartup()`. The static `DI` class holds the `IServiceProvider` for use outside the composition root.

### Key classes

| Class                          | Role                                                                                                          |
|--------------------------------|---------------------------------------------------------------------------------------------------------------|
| `FinancingAppContext`          | EF Core DbContext; SQLite path hardcoded to `"financing.db"`                                                  |
| `TransactionService`           | CRUD for `Transaction`; exposes `ReadOnlyObservableCollection<Transaction>`                                   |
| `CategoryService`              | CRUD for `Category`; exposes `ReadOnlyObservableCollection<Category>`                                         |
| `TransactionCreationViewModel` | Input form state, validation, category selection with combobox                                                           |
| `TransactionsListViewModel`    | Wraps each `Transaction` in a `TransactionViewModel` by subscribing to `CollectionChanged`                    |
| `Currency`                     | `readonly struct` — stores amounts as integer cents; parses "CHF 10.50", "10,50", etc.; formats as "CHF X.YY" |
| `DateConverter`                | Converts `DateTime` ↔ `"yyyy-MM-dd"` string                                                                   |

### Domain entities

- **`Transaction`**: `TransactionId`, `Type` (FK → Category), `Description`, `Date` (string "yyyy-MM-dd"), `Amount` (int cents).
- **`Category`**: `CategoryId`, `Name`. Has a `Category.Null` singleton (null-object pattern).

## Conventions

- Amounts are **always stored as integer cents** in the database and in domain objects. The `Currency` struct is the only place that converts to/from decimal display values.
- ViewModels **never hold an internal `ObservableCollection` directly exposed to the view** — they expose `ReadOnlyObservableCollection` and reference service collections
- Database migrations live in `FinancingApp/Persistence/Migrations/` and are managed via EF Core Package Manager Console (`Add-Migration`, `Update-Database`).
