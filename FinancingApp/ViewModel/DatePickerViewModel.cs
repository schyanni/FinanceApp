using CommunityToolkit.Mvvm.ComponentModel;

namespace FinancingApp.ViewModel
{
    public class DatePickerViewModel : ObservableObject
    {
        private bool _isDateValid;

        private DateTime _date;

        public string Day { get => _date.Day.ToString("D2"); set => SetDay(value); }

        public string Month { get => _date.Month.ToString("D2"); set => SetMonth(value); }

        public string Year { get => _date.Year.ToString("D4"); set => SetYear(value); }

        public DateTime Date
        {
            get => _date;
            set
            {
                if (SetProperty(ref _date, value))
                {
                    OnPropertyChanged(nameof(Day));
                    OnPropertyChanged(nameof(Month));
                    OnPropertyChanged(nameof(Year));
                }

            }
        }

        private void SetDay(string day)
        {
            if (!int.TryParse(day, out var parsedDay))
            {
                IsDateValid = false;
                return;
            }

            IsDateValid = true;

            DateTime newDate;
            try
            {
                newDate = new DateTime(_date.Year, _date.Month, parsedDay);
            }
            catch
            {
                newDate = _date;
                IsDateValid = false;
            }

            Date = newDate;

        }

        private void SetMonth(string month)
        {
            if (!int.TryParse(month, out var parsedMonth))
            {
                IsDateValid = false;
                return;
            }

            IsDateValid = true;

            DateTime newDate;
            try
            {
                newDate = new DateTime(_date.Year, parsedMonth, _date.Day);
            }
            catch
            {
                newDate = _date;
                IsDateValid = false;
            }

            Date = newDate;
        }

        private void SetYear(string year)
        {
            if (!int.TryParse(year, out var parsedYear))
            {
                IsDateValid = false;
                return;
            }

            IsDateValid = true;

            DateTime newDate;
            try
            {
                newDate = new DateTime(parsedYear, _date.Month, _date.Day);
            }
            catch
            {
                newDate = _date;
                IsDateValid = false;
            }

            Date = newDate;
        }

        public bool IsDateValid { get => _isDateValid; private set => SetProperty(ref _isDateValid, value); }
    }
}
