using System.Windows;
using VehicleRentalSystem.ViewModel;

namespace VehicleRentalSystem.View
{
    /// <summary>
    /// Interaction logic for RentVehicleView.xaml
    /// </summary>
    public partial class RentVehicleView : Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vehicle">Vehicle to be rented</param>
        /// <param name="eventAggregator">Event aggregator for passing information to another viewmodel</param>
        public RentVehicleView(Vehicle vehicle, ref EventAggregator eventAggregator)
        {
            InitializeComponent();
            RentVehicleViewModel ViewModel = new RentVehicleViewModel(vehicle, ref eventAggregator);
            ViewModel.RequestClose += (s, e) => this.Close();
            this.DataContext = ViewModel;
        }
    }
}
