using System;
using System.Windows.Input;
using VehicleRentalSystem.View;
using System.Collections.ObjectModel;
using System.Windows;

namespace VehicleRentalSystem.ViewModel
{
    class VehicleViewModel : ViewModelBase<Message>
    {
        private ObservableCollection<Vehicle> _vehicleList;
        private EventAggregator eventAggregator;
        private Vehicle _selectedVehicle;

        // Command to use instead of onclick event. Based on: https://blog.magnusmontin.net/2013/06/30/handling-events-in-an-mvvm-wpf-application/
        private readonly DelegateCommand<string> _addCommand;
        private readonly DelegateCommand<string> _editCommand;
        private readonly DelegateCommand<string> _deleteCommand;

        private EditVehicleView editVehicleWin;

        public VehicleViewModel(ref EventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            //_vehicleList = new List<VehicleView>()
            _vehicleList = new ObservableCollection<Vehicle>()
            {
                new Vehicle("Make1", "mode1", 2001),
                new Vehicle("Make2", "mode2", 2002)
            };

            _addCommand = new DelegateCommand<string>(
                (s) => { ShowEditVehicleDialog(null); }, //Execute
                (s) => { return true; } //CanExecute - in this case, always
            );

            _editCommand = new DelegateCommand<string>(
                (s) => { ShowEditVehicleDialog(SelectedVehicle); },
                (s) => { return SelectedVehicle != null; }
            );
            _deleteCommand = new DelegateCommand<string>(
                (s) => { ShowDeleteVehicleDialog(); },
                (s) => { return SelectedVehicle != null; }
            );
        }


        public Vehicle SelectedVehicle {
            get => _selectedVehicle;
            set {
                _selectedVehicle = value;
                _editCommand.RaiseCanExecuteChanged();
                _deleteCommand.RaiseCanExecuteChanged();
            }
        }

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

        public DelegateCommand<string> EditVehicleClickCommand
        {
            get { return _editCommand; }
        }
        private string _editinput;
        public string Editinput
        {
            get { return _editinput; }
            set
            {
                _editinput = value;
                _editCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand<string> DeleteVehicleClickCommand
        {
            get { return _deleteCommand; }
        }
        private string _deleteinput;
        public string Deleteinput
        {
            get { return _deleteinput; }
            set
            {
                _deleteinput = value;
                _deleteCommand.RaiseCanExecuteChanged();
            }
        }


        // Based on: https://www.c-sharpcorner.com/article/how-to-open-a-child-window-from-view-model-in-mvvm-in-wpf2/
        //private void ShowAddVehicleDialog()
        //{
        //    this.editVehicleWin = new EditVehicleView(null, ref eventAggregator);
        //    addVehicleWin.ShowDialog();
        //}

        private void ShowEditVehicleDialog(Vehicle vehicleToEdit)
        {
            this.editVehicleWin = new EditVehicleView(vehicleToEdit, ref eventAggregator);
            editVehicleWin.ShowDialog();
        }

        private void ShowDeleteVehicleDialog()
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(
                "Are you sure?",
                "Delete confirmation",
                MessageBoxButton.YesNo
            );
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Vehicles.Remove(SelectedVehicle);
                SelectedVehicle = null;
            }
        }

        public override void Handle(Message obj)
        {
            Vehicle v = obj.Vehicle;
            Vehicle old = obj.OldVehicle;
            if (old == null)
            {
                Vehicles.Add(v);
            } else
            {
                for (int i=0; i<Vehicles.Count; i++ )
                {
                    if (Vehicles[i].printDetails() == old.printDetails())
                    {
                        Vehicles[i] = v;
                        break;
                    }
                }
            }
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
