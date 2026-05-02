using System.Collections.ObjectModel;

namespace FinancingApp.Domain;

public interface ITransactionService
{
    Task CreateTransactionAsync(Transaction transaction);

    Task LoadFilteredTransactionsAsync();

    ReadOnlyObservableCollection<Transaction> FilteredTransactions { get; }
}
