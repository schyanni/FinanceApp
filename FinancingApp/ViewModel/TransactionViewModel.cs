using CommunityToolkit.Mvvm.ComponentModel;
using FinancingApp.Common;
using FinancingApp.Domain;

namespace FinancingApp.ViewModel
{
    public class TransactionViewModel : ObservableObject
    {
        private readonly Transaction _transaction;

        public TransactionViewModel()
        {
            _transaction = new Transaction
            {
                Date = DateConverter.ToString(DateTime.Now)
            };
        }

        public TransactionViewModel(Transaction transaction)
        {
            _transaction = transaction;

        }

        public Category TransactionType
        {
            get => _transaction.Type;
            set => SetProperty(_transaction.Type, value, _transaction, (t, category) => t.Type = category);
        }

        public string Description
        {
            get => _transaction.Description;
            set => SetProperty(_transaction.Description, value, _transaction, (t, description) => t.Description = description);
        }

        public DateTime Date
        {
            get => DateConverter.ToDateTime(_transaction.Date);
            set => SetProperty(DateConverter.ToDateTime(_transaction.Date), value, _transaction, (t, date) => t.Date = DateConverter.ToString(date));

        }

        public string Amount
        {
            get => new Currency(_transaction.Amount).ToString();
            set => SetAmount(value);
        }

        private void SetAmount(string amount)
        {
            if (!Currency.TryParse(amount, out var currency))
            {
                return;
            }

            _transaction.Amount = currency.Value();
            OnPropertyChanged(nameof(Amount));
        }
    }
}

