using System;
using System.Windows.Input;
using VehicleRentalSystem.View;
using System.Collections.ObjectModel;
using System.Windows;

namespace VehicleRentalSystem.ViewModel
{
    class MainViewModel : ViewModelBase<Message>
    {
        private ObservableCollection<Vehicle> _vehicleList;
        private EventAggregator eventAggregator;
        public event EventHandler RequestClose;
        private Vehicle _selectedVehicle;

        // Command to use instead of onclick event. Based on: https://blog.magnusmontin.net/2013/06/30/handling-events-in-an-mvvm-wpf-application/
        private readonly DelegateCommand<string> _addCommand;
        private readonly DelegateCommand<string> _editCommand;
        private readonly DelegateCommand<string> _deleteCommand;
        private readonly DelegateCommand<string> _viewCommand;
        private readonly DelegateCommand<string> _rentCommand;
        private readonly DelegateCommand<string> _returnCommand;
        private readonly DelegateCommand<string> _exitCommand;

        private EditVehicleView editVehicleWin;
        private VehicleDetailsView viewVehicleWin;
        private RentVehicleView rentVehicleWin;
        private ReturnVehicleView returnVehicleWin;

        public MainViewModel(ref EventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            //_vehicleList = new List<VehicleView>()
            _vehicleList = new ObservableCollection<Vehicle>()
            {
                new Vehicle("Make1", "mode1", 2001, "1REG011", 12001, 70.6),
                new Vehicle("Make2", "mode2", 2002, "2ABC123", 35235, 56)
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
            _viewCommand = new DelegateCommand<string>(
                (s) => { ShowVehicleDetailsDialog(SelectedVehicle); },
                (s) => { return SelectedVehicle != null; }
            );
            _rentCommand = new DelegateCommand<string>(
                (s) => { ShowRentVehicleDialog(SelectedVehicle); },
                (s) => {
                    if (SelectedVehicle == null) return false;
                    return SelectedVehicle.Status == "Available";
                }
            );
            _returnCommand = new DelegateCommand<string>(
                (s) => { ShowReturnVehicleDialog(SelectedVehicle); },
                (s) => {
                    if (SelectedVehicle == null) return false;
                    return SelectedVehicle.Status.StartsWith("Rent");
                }
            );
            _exitCommand = new DelegateCommand<string>(
                (s) => { OnRequestClose(); },
                (s) => true
            );
        }

        public Vehicle SelectedVehicle {
            get => _selectedVehicle;
            set {
                _selectedVehicle = value;
                _editCommand.RaiseCanExecuteChanged();
                _deleteCommand.RaiseCanExecuteChanged();
                _viewCommand.RaiseCanExecuteChanged();
                _rentCommand.RaiseCanExecuteChanged();
                _returnCommand.RaiseCanExecuteChanged();
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
        public DelegateCommand<string> ViewVehicleClickCommand
        {
            get => _viewCommand;
        }
        public DelegateCommand<string> RentVehicleClickCommand
        {
            get => _rentCommand;
        }
        public DelegateCommand<string> ReturnVehicleClickCommand
        {
            get => _returnCommand;
        }

        public DelegateCommand<string> ExitCommand
        {
            get => _exitCommand;
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

        public void ShowVehicleDetailsDialog(Vehicle vehicle)
        {
            viewVehicleWin = new VehicleDetailsView(vehicle);
            viewVehicleWin.ShowDialog();
        }

        private void ShowRentVehicleDialog(Vehicle vehicleToRent)
        {
            this.rentVehicleWin = new RentVehicleView(vehicleToRent, ref eventAggregator);
            rentVehicleWin.ShowDialog();
        }

        private void ShowReturnVehicleDialog(Vehicle vehicleToRent)
        {
            this.returnVehicleWin = new ReturnVehicleView(vehicleToRent, ref eventAggregator);
            returnVehicleWin.ShowDialog();
        }

        public override void Handle(Message obj)
        {
            bool updated = obj.Updated;
            Vehicle v = obj.Vehicle;
            Vehicle old = obj.OldVehicle;
            if (old == null && !updated)
            {
                Vehicles.Add(v);
                return;
            }
            int selectedVehicleIndex = -1;
            for (int i = 0; i < Vehicles.Count; i++)
            {
                if (Vehicles[i].printDetails() == (old == null ? v : old).printDetails())
                {
                    selectedVehicleIndex = i;
                    break;
                }
            }
            Vehicles[selectedVehicleIndex] = v;
        }

        protected void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
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
