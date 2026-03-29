using FinancingApp.Common;
using FinancingApp.Domain;
using FinancingApp.Persistence;
using FinancingApp.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FinancingApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            InitializeDependencyInjection();

            var window = DI.ServiceProvider.GetService<MainWindow>();
            window?.Show();
        }


        private void InitializeDependencyInjection()
        {
            var services = new ServiceCollection();

            // views and view models
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<TransactionCreationViewModel>();
            services.AddSingleton<TransactionsListViewModel>();

            // common

            // domain
            services.AddSingleton<CategoryService>();
            services.AddSingleton<TransactionService>();

            // persistence
            services.AddSingleton<FinancingAppContext>();

            DI.ServiceProvider = services.BuildServiceProvider();
        }

    }
}