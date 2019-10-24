using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VehicleRentalSystem.ViewModel;

namespace VehicleRentalSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            View.MainWindow main = new View.MainWindow();
            MainWindowViewModel mainWinViewModel = new MainWindowViewModel();
            main.DataContext = mainWinViewModel;
            main.Show();
        }
    }
}
