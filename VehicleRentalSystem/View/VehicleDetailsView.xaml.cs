using System.Windows;
using VehicleRentalSystem.ViewModel;

namespace VehicleRentalSystem.View
{
    /// <summary>
    /// Interaction logic for VehicleDetailsView.xaml
    /// </summary>
    public partial class VehicleDetailsView : Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vehicle">Vehicle to show details of</param>
        public VehicleDetailsView(Vehicle vehicle)
        {
            InitializeComponent();
            VehicleDetailsViewModel ViewModel = new VehicleDetailsViewModel(vehicle);
            ViewModel.RequestClose += (s, e) => this.Close();
            this.DataContext = ViewModel;
        }

    }
}
