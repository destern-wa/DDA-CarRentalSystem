using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VehicleRentalSystem.View;
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
            EventAggregator eventAggregator = new EventAggregator();
            MainView MainView = new MainView(ref eventAggregator);
            MainViewModel vehicleViewModel = new MainViewModel(ref eventAggregator);
            vehicleViewModel.RequestClose += (s, ea) => MainView.Close();
            MainView.DataContext = vehicleViewModel;
            MainView.Show();
        }
    }
}
