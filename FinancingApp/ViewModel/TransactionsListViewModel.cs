using FinancingApp.Domain;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace FinancingApp.ViewModel
{
    public class TransactionsListViewModel
    {
        private readonly TransactionService _transactionService;

        private readonly CategoryService _categoryService;

        private readonly ObservableCollection<Transaction> _filteredTransactions = [];
        public TransactionsListViewModel(TransactionService transactionService, CategoryService categoryService)
        {
            _transactionService = transactionService;
            _categoryService = categoryService;
        }

        public async Task LoadDataFromDbAsync()
        {
            await _categoryService.LoadCategoriesAsync();
            await _transactionService.LoadFilteredTransactionsAsync(); 
        }

        public ReadOnlyObservableCollection<Category> AvailableCategories => _categoryService.AvailableCategories;

        public ReadOnlyObservableCollection<Transaction> Transactions => _transactionService.FilteredTransactions;
    }
}
