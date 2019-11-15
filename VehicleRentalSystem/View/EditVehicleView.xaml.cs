using System.Windows;
using VehicleRentalSystem.ViewModel;
namespace VehicleRentalSystem.View
{
    /// <summary>
    /// Interaction logic for EditVehicleView.xaml
    /// </summary>
    public partial class EditVehicleView : Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oldVehicle">Vehicle to edit, or null for adding a new vehicle</param>
        /// <param name="eventAggregator">Event aggregator for passing information to another viewmodel</param>
        public EditVehicleView(Vehicle oldVehicle, ref EventAggregator eventAggregator)
        {
            InitializeComponent();
            EditVehicleViewModel ViewModel = new EditVehicleViewModel(oldVehicle, ref eventAggregator);
            ViewModel.RequestClose += (s, e) => this.Close();
            this.DataContext = ViewModel;
        }
    }
}
