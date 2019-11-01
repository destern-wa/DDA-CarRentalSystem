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
using System.Windows.Shapes;
using VehicleRentalSystem.ViewModel;
namespace VehicleRentalSystem.View
{
    /// <summary>
    /// Interaction logic for AddVehicle.xaml
    /// </summary>
    public partial class AddVehicle : Window
    {
        //public EventAggregator eventAggregator { get; set; }

        //public AddVehicle(EventAggregator eventAggregator): base()
        //{
        //    this.eventAggregator = eventAggregator;
        //}

        public VehicleRentalSystem.Vehicle theVehicle;
        public AddVehicle(ref EventAggregator eventAggregator)
        {
            InitializeComponent();
            AddVehicleViewModel ViewModel = new AddVehicleViewModel(ref eventAggregator);
            ViewModel.RequestClose += (s, e) => this.Close();
            this.DataContext = ViewModel;
        }

        //private void SubmitButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        VehicleRentalSystem.Vehicle theVehicle = new VehicleRentalSystem.Vehicle(MakeTextbox.Text, ModelTextbox.Text, int.Parse(YearTextbox.Text));
        //        this.Close();
        //    } catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //    };
        //}
    }
}
