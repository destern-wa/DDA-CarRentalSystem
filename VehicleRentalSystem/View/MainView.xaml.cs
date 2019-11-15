using System.Windows;
namespace VehicleRentalSystem.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eventAggregator">Event aggregator for passing information between viewmodels</param>
        public MainView(ref EventAggregator eventAggregator)
        {
            InitializeComponent();
        }
    }
}
