using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
//using VehicleRentalSystem.Model;

namespace VehicleRentalSystem.ViewModel
{
    class MainWindowViewModel
    {
        private IList<Vehicle> _vehicleList;
        public MainWindowViewModel()
        {
            _vehicleList = new List<Vehicle>()
            {
                new Vehicle("Make1", "mode1", 2001),
                new Vehicle("Make2", "mode2", 2002)
            };

        }

        public IList<Vehicle> Vehicles
        {
            get => _vehicleList;
            set { _vehicleList = value; }
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
