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
    /// Interaction logic for ReturnVehicleView.xaml
    /// </summary>
    public partial class ReturnVehicleView : Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vehicle">Vehicle to be returned</param>
        /// <param name="eventAggregator">Event aggregator for passing information to another viewmodel</param>
        public ReturnVehicleView(Vehicle vehicle, ref EventAggregator eventAggregator)
        {
            InitializeComponent();
            ReturnVehicleViewModel ViewModel = new ReturnVehicleViewModel(vehicle, ref eventAggregator);
            ViewModel.RequestClose += (s, e) => this.Close();
            this.DataContext = ViewModel;
        }
    }
}
