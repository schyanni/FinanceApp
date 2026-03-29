using FinancingApp.ViewModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace FinancingApp.View
{
    /// <summary>
    /// Interaction logic for DatePicker.xaml
    /// </summary>
    public partial class DatePicker : UserControl
    {
        public DatePickerViewModel ViewModel { get; }
        public DateTime Date { get => (DateTime)GetValue(DateProperty); set => SetValue(DateProperty, value); }

        public static readonly DependencyProperty DateProperty = DependencyProperty.Register(
            name: nameof(Date), propertyType: typeof(DateTime), ownerType: typeof(DatePicker),
            typeMetadata: new FrameworkPropertyMetadata(defaultValue: DateTime.Today));

        public DatePicker()
        {
            ViewModel = new DatePickerViewModel();
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            ViewModel.Date = Date;

            DataContext = ViewModel;
            InitializeComponent();
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(DatePickerViewModel.Date))
            {
                Date = ViewModel.Date;
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if(e.Property == DateProperty)
            {
                ViewModel.Date = Date;
            }
        }
    }
}
