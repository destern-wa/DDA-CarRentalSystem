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
            EventAggregator eventAggregator = new EventAggregator();
            View.Vehicle VehicleView = new View.Vehicle(ref eventAggregator);
            VehicleViewModel vehicleViewModel = new VehicleViewModel(ref eventAggregator);
            VehicleView.DataContext = vehicleViewModel;
            VehicleView.Show();
        }
    }
}
