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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VehicleRentalSystem.ViewModel;
namespace VehicleRentalSystem.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Vehicle : Window
    {
        public Vehicle(ref EventAggregator eventAggregator)
        {
            InitializeComponent();
            this.DataContext = new VehicleViewModel(ref eventAggregator);
        }

        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    Vehicle v = new Vehicle("Ford", "T812", 2014);

        //    // Vehicle sample distance
        //    v.addFuel(new Random().NextDouble() * 10, 1.3);

        //    OutputTextBlock.Text = v.printDetails();
        //}
    }
}
