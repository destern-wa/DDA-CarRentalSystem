using System.Windows;
using VehicleRentalSystem.ViewModel;
namespace VehicleRentalSystem.View
{
    /// <summary>
    /// Interaction logic for AddVehicleView.xaml
    /// </summary>
    public partial class AddVehicleView : Window
    {
        public AddVehicleView(ref EventAggregator eventAggregator)
        {
            InitializeComponent();
            AddVehicleViewModel ViewModel = new AddVehicleViewModel(ref eventAggregator);
            ViewModel.RequestClose += (s, e) => this.Close();
            this.DataContext = ViewModel;
        }
    }
}
