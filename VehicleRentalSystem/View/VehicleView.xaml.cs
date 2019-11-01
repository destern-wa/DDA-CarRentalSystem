using System.Windows;
using VehicleRentalSystem.ViewModel;
namespace VehicleRentalSystem.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class VehicleView : Window
    {
        public VehicleView(ref EventAggregator eventAggregator)
        {
            InitializeComponent();
            this.DataContext = new VehicleViewModel(ref eventAggregator);
        }
    }
}
