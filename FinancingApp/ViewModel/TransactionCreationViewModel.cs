using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinancingApp.Common;
using FinancingApp.Domain;

namespace FinancingApp.ViewModel;

public class TransactionCreationViewModel : ObservableObject
{
    private readonly CategoryService _categoryService;

    private readonly TransactionService _transactionService;

    private Currency _amountInCurrency = 0;
    private Category _selectedCategory = Category.Null;

    public TransactionCreationViewModel(CategoryService categoryService, TransactionService transactionService)
    {
        _categoryService = categoryService;
        _transactionService = transactionService;

        AddTransactionCommand = new AsyncRelayCommand(AddTransactionAsync, () => IsCurrentTransactionValid);
        ConvertToCurrencyCommand = new RelayCommand(ConvertToCurrency, () => true);
        ResetTransaction();
    }

    public IAsyncRelayCommand AddTransactionCommand { get; }

    public IRelayCommand ConvertToCurrencyCommand { get; }

    public string Amount
    {
        get;
        set => SetProperty(ref field, value);
    } = new Currency(0).ToString();

    public ReadOnlyObservableCollection<Category> AvailableCategories => _categoryService.AvailableCategories;

    public DateTime Date
    {
        get;
        set => SetProperty(ref field, value);
    } = DateTime.Today;

    public string Description
    {
        get;
        set
        {
            SetProperty(ref field, value); 
            AddTransactionCommand.NotifyCanExecuteChanged();
        }
    } = string.Empty;

    public bool IsActiveForNewTransaction
    {
        get;
        set => SetProperty(ref field, value);
    }

    public bool IsAmountValid
    {
        get;
        set
        {
            SetProperty(ref field, value);
            AddTransactionCommand.NotifyCanExecuteChanged();
        }
    } = true;

    public Category SelectedCategory
    {
        get => _selectedCategory; set
        {
            SetProperty(ref _selectedCategory, value);
            AddTransactionCommand.NotifyCanExecuteChanged();
        }
    }

    private Transaction CurrentTransaction => new()
    {
        Amount = _amountInCurrency.Value(),
        Date = DateConverter.ToString(Date),
        Description = Description,
        Type = _selectedCategory
    };

    private bool IsCurrentTransactionValid => IsAmountValid && !string.IsNullOrEmpty(SelectedCategory.Name) && !string.IsNullOrEmpty(Description);

    private async Task AddTransactionAsync()
    {
        ConvertToCurrency();
        if (!IsCurrentTransactionValid)
        {
            return;
        }

        var transaction = CurrentTransaction;
        await _transactionService.CreateTransactionAsync(transaction);
    }

    private void ConvertToCurrency()
    {
        if (Currency.TryParse(Amount, out var currency))
        {
            _amountInCurrency = currency;
            OnPropertyChanged(nameof(Amount));
            IsAmountValid = true;
            Amount = _amountInCurrency.ToString();
        }
        else
        {
            IsAmountValid = false;
            _amountInCurrency = 0;
            OnPropertyChanged(nameof(Amount));
        }
    }

    private void ResetTransaction()
    {
        Date = DateTime.Today;
        Description = string.Empty;
        SelectedCategory = Category.Null;
        Amount = "0";
    }
}