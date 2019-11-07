using System.Windows;
using VehicleRentalSystem.ViewModel;
namespace VehicleRentalSystem.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(ref EventAggregator eventAggregator)
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(ref eventAggregator);
        }
    }
}
