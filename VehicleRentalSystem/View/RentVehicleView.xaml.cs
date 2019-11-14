using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VehicleRentalSystem.ViewModel;

namespace VehicleRentalSystem.View
{
    /// <summary>
    /// Interaction logic for RentVehicleView.xaml
    /// </summary>
    public partial class RentVehicleView : Window
    {
        public RentVehicleView(Vehicle vehicle, ref EventAggregator eventAggregator)
        {
            InitializeComponent();
            RentVehicleViewModel ViewModel = new RentVehicleViewModel(vehicle, ref eventAggregator);
            ViewModel.RequestClose += (s, e) => this.Close();
            this.DataContext = ViewModel;
        }
    }
}
