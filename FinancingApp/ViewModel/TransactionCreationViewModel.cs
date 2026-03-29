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

    private Currency _amount = 0;

    private DateTime _date = DateTime.Today;

    private string _description = string.Empty;

    private Category _selectedCategory = Category.Null;

    public TransactionCreationViewModel(CategoryService categoryService, TransactionService transactionService)
    {
        _categoryService = categoryService;
        _transactionService = transactionService;

        AddTransactionCommand = new AsyncRelayCommand(AddTransactionAsync, () => IsCurrentTransactionValid);
    }

    public IAsyncRelayCommand AddTransactionCommand { get; }

    public string Amount
    {
        get => _amount.ToString();
        set => SetAmount(value);
    }

    public ReadOnlyObservableCollection<Category> AvailableCategories => _categoryService.AvailableCategories;

    public DateTime Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }

    public string Description
    {
        get => _description;
        set
        {
            SetProperty(ref _description, value); 
            AddTransactionCommand.NotifyCanExecuteChanged();
        }
    }

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
        Amount = _amount.Value(),
        Date = DateConverter.ToString(_date),
        Description = _description,
        Type = _selectedCategory
    };

    private bool IsCurrentTransactionValid => IsAmountValid && !string.IsNullOrEmpty(SelectedCategory.Name) && !string.IsNullOrEmpty(Description);

    private async Task AddTransactionAsync()
    {
        if (!IsCurrentTransactionValid)
        {
            return;
        }

        var transaction = CurrentTransaction;
        await _transactionService.CreateTransactionAsync(transaction);

        ResetTransaction();
    }

    private void ResetTransaction()
    {
        Date = DateTime.Today;
        Description = string.Empty;
        SelectedCategory = Category.Null;
        Amount = "0";
    }

    private void SetAmount(string amount)
    {
        if (Currency.TryParse(amount, out var currency))
        {
            _amount = currency;
            OnPropertyChanged(nameof(Amount));
            IsAmountValid = true;
        }
        else
        {
            IsAmountValid = false;
            _amount = 0;
            OnPropertyChanged(nameof(Amount));
        }
    }
}