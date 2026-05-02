using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinancingApp.Domain;
using Microsoft.Win32;
using System.Windows;

namespace FinancingApp.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly CsvImportService _csvImportService;

        public MainWindowViewModel(
            TransactionCreationViewModel transactionCreationViewModel,
            TransactionsListViewModel transactionsListViewModel,
            CsvImportService csvImportService)
        {
            TransactionCreationViewModel = transactionCreationViewModel;
            TransactionsListViewModel = transactionsListViewModel;
            _csvImportService = csvImportService;

            OpenTransactionCreationCommand = new RelayCommand(OpenTransactionCreation);
            LoadInitialDataCommand = new AsyncRelayCommand(async () => await TransactionsListViewModel.LoadDataFromDbAsync());
            ImportDataCommand = new AsyncRelayCommand(ImportDataAsync);
            ExitCommand = new RelayCommand(() => { });
        }

        public IRelayCommand OpenTransactionCreationCommand { get; }

        public IAsyncRelayCommand LoadInitialDataCommand { get; }

        public IAsyncRelayCommand ImportDataCommand { get; }

        public IRelayCommand ExitCommand { get; }

        public TransactionCreationViewModel TransactionCreationViewModel { get; }

        public TransactionsListViewModel TransactionsListViewModel { get; }

        private void OpenTransactionCreation()
        {
            // Logic to open the transaction creation view
        }

        private async Task ImportDataAsync()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "CSV-Dateien (*.csv)|*.csv",
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            var (imported, skipped) = await _csvImportService.ImportAsync(dialog.FileName);
            MessageBox.Show($"{imported} Transaktionen importiert, {skipped} übersprungen.");
        }
    }
}
