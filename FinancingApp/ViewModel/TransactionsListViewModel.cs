using FinancingApp.Domain;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace FinancingApp.ViewModel
{
    public class TransactionsListViewModel
    {
        private readonly TransactionService _transactionService;

        private readonly CategoryService _categoryService;

        private readonly ObservableCollection<TransactionViewModel> _filteredTransactionViewModels = [];
        public TransactionsListViewModel(TransactionService transactionService, CategoryService categoryService)
        {
            _transactionService = transactionService;
            _categoryService = categoryService;

            var collection = _transactionService.FilteredTransactions as INotifyCollectionChanged;
            collection.CollectionChanged += OnFilteredTransactionCollectionChanged;

            Transactions = new ReadOnlyObservableCollection<TransactionViewModel>(_filteredTransactionViewModels);
        }

        private void OnFilteredTransactionCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add when e.NewItems is not null:
                {
                    foreach (var item in e.NewItems)
                    {
                        if (item is Transaction transaction)
                        {
                            _filteredTransactionViewModels.Add(new TransactionViewModel(transaction));
                        }
                    }
                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Reset:
                    throw new NotImplementedException();
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public ReadOnlyObservableCollection<Category> AvailableCategories => _categoryService.AvailableCategories;

        public ReadOnlyObservableCollection<TransactionViewModel> Transactions { get; }
    }
}
