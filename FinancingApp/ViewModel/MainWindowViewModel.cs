using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace FinancingApp.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel(TransactionCreationViewModel transactionCreationViewModel, TransactionsListViewModel transactionsListViewModel)
        {
            TransactionCreationViewModel = transactionCreationViewModel;
            TransactionsListViewModel = transactionsListViewModel;

            OpenTransactionCreationCommand = new RelayCommand(OpenTransactionCreation);

        }

        public ICommand OpenTransactionCreationCommand { get; }

        public TransactionCreationViewModel TransactionCreationViewModel { get; }

        public TransactionsListViewModel TransactionsListViewModel { get; }

        private void OpenTransactionCreation()
        {
            // Logic to open the transaction creation view
        }
    }
}
