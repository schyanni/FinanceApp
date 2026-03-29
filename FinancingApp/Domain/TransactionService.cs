using System.Collections.ObjectModel;
using FinancingApp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinancingApp.Domain;

public class TransactionService
{
    private readonly FinancingAppContext _context;

    private readonly ObservableCollection<Transaction> _filteredTransactions = [];

    public TransactionService(FinancingAppContext context)
    {
        _context = context;
        FilteredTransactions = new ReadOnlyObservableCollection<Transaction>(_filteredTransactions);
    }

    public ReadOnlyObservableCollection<Transaction> FilteredTransactions { get; }

    public async Task LoadFilteredTransactionsAsync()
    {
        var transactions = await _context.Transactions.ToListAsync();
        _filteredTransactions.Clear();
        await Task.Run(() =>
        {
            foreach (var transaction in transactions)
            {
                _filteredTransactions.Add(transaction);
            }
        });
    }

    public async Task CreateTransactionAsync(Transaction transaction)
    {
        _filteredTransactions.Add(transaction);
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }
}