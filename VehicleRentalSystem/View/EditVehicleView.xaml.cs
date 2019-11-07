using System.Windows;
using VehicleRentalSystem.ViewModel;
namespace VehicleRentalSystem.View
{
    /// <summary>
    /// Interaction logic for EditVehicleView.xaml
    /// </summary>
    public partial class EditVehicleView : Window
    {
        public EditVehicleView(Vehicle oldVehicle, ref EventAggregator eventAggregator)
        {
            InitializeComponent();
            EditVehicleViewModel ViewModel = new EditVehicleViewModel(oldVehicle, ref eventAggregator);
            ViewModel.RequestClose += (s, e) => this.Close();
            this.DataContext = ViewModel;
        }
    }
}
