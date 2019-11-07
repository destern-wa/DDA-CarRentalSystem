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
            //MainViewModel ViewModel = new MainViewModel(ref eventAggregator, (s, e) => this.Close());
            ////ViewModel.RequestClose += (s, e) => this.Close();
            //this.DataContext = ViewModel;
        }
    }
}
