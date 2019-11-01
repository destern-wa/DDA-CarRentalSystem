using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VehicleRentalSystem.View;
using VehicleRentalSystem;
using System.Collections.ObjectModel;

namespace VehicleRentalSystem.ViewModel
{
    class VehicleViewModel : IListen<Messaging>
    {
        //private List<Vehicle> _vehicleList;
        private ObservableCollection<Vehicle> _vehicleList;
        private EventAggregator eventAggregator;
        // Command to use instead of onclick event. Based on: https://blog.magnusmontin.net/2013/06/30/handling-events-in-an-mvvm-wpf-application/
        private readonly DelegateCommand<string> _addCommand;

        private AddVehicle addVehicleWin;

        public VehicleViewModel(ref EventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            //_vehicleList = new List<Vehicle>()
            _vehicleList = new ObservableCollection<Vehicle>()
            {
                new Vehicle("Make1", "mode1", 2001),
                new Vehicle("Make2", "mode2", 2002)
            };

            _addCommand = new DelegateCommand<string>(
            (s) => { ShowAddVehicleDialog(); }, //Execute
            (s) => { return true; } //CanExecute - in this case, always
            );
        }

        //public List<Vehicle> Vehicles
        public ObservableCollection<Vehicle> Vehicles
        {
            get => _vehicleList;
            set { _vehicleList = value; }
        }

        public DelegateCommand<string> AddVehicleClickCommand
        {
            get { return _addCommand; }
        }
        private string _input;
        public string Input
        {
            get { return _input; }
            set
            {
                _input = value;
                _addCommand.RaiseCanExecuteChanged();
            }
        }

        // Based on: https://www.c-sharpcorner.com/article/how-to-open-a-child-window-from-view-model-in-mvvm-in-wpf2/
        private void ShowAddVehicleDialog()
        {
            this.addVehicleWin = new AddVehicle(ref eventAggregator);
            //addVehicleWin.eventAggregator = eventAggregator;
            addVehicleWin.ShowDialog();
            //if (addVehicleWin.theVehicle != null)
            //{
            //    _vehicleList.Add(addVehicleWin.theVehicle);
            //}
        }

        public void Handle(Messaging obj)
        {
            Console.WriteLine("VehicleViewModel recieved a message!");
            Vehicle v = obj.Vehicle;
            Console.WriteLine("Vehicle: " + v.Manufacturer + " " + v.Model + " " + v.Year.ToString());
            //List<Vehicle> updatedVehicles = Vehicles.ToList();
            //updatedVehicles.Add(v);
            //Vehicles = updatedVehicles;
            Vehicles.Add(v);
        }




        private ICommand mUpdater;
        public ICommand UpdateCommand
        {
            get
            {
                if (mUpdater == null)
                    mUpdater = new Updater();
                return mUpdater;
            }
            set
            {
                mUpdater = value;
            }
        }



    }
    class Updater : ICommand
    {
        #region ICommand Members  

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            //Your Code  
        }
        #endregion
    }
}
