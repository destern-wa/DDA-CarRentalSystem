using System;
using System.Windows.Input;
using VehicleRentalSystem.View;
using System.Collections.ObjectModel;
using System.Windows;

namespace VehicleRentalSystem.ViewModel
{
    /// <summary>
    /// View model for main view
    /// </summary>
    class MainViewModel : ViewModelBase<Message>
    {
        /// <summary>
        /// Vehicles to display in view
        /// </summary>
        private ObservableCollection<Vehicle> _vehicleList;
        /// <summary>
        /// Event aggregator for passing messages to other view models
        /// </summary>
        private EventAggregator eventAggregator;
        /// <summary>
        /// Handler to call to tell the view to close
        /// </summary>
        public event EventHandler RequestClose;
        /// <summary>
        /// Currently selected vehicle
        /// </summary>
        private Vehicle _selectedVehicle;

        // Commands to use instead of onclick event. Based on: https://blog.magnusmontin.net/2013/06/30/handling-events-in-an-mvvm-wpf-application/
        private readonly DelegateCommand<string> _addCommand;
        private readonly DelegateCommand<string> _editCommand;
        private readonly DelegateCommand<string> _deleteCommand;
        private readonly DelegateCommand<string> _viewCommand;
        private readonly DelegateCommand<string> _rentCommand;
        private readonly DelegateCommand<string> _returnCommand;
        private readonly DelegateCommand<string> _serviceCommand;
        private readonly DelegateCommand<string> _exitCommand;

        /// <summary>
        /// View for editing/adding a vehice
        /// </summary>
        private EditVehicleView editVehicleWin;
        /// <summary>
        /// View for veiwing a vehicle's details
        /// </summary>
        private VehicleDetailsView viewVehicleWin;
        /// <summary>
        /// View for renting a vehicle
        /// </summary>
        private RentVehicleView rentVehicleWin;
        /// <summary>
        /// View for returning a rented vehicle
        /// </summary>
        private ReturnVehicleView returnVehicleWin;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eventAggregator">Event aggregator for passing messages to other view models</param>
        public MainViewModel(ref EventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);

            // Add sample vehicle data
            _vehicleList = new ObservableCollection<Vehicle>()
            {
                new Vehicle("Make1", "mode1", 2001, "1REG011", 12001, 70.6),
                new Vehicle("Make2", "mode2", 2002, "2ABC123", 35235, 56)
            };

            // Delegate commands take two parameters: What to execute (function),
            // and a check if they can execute (function that returns a boolean)
            _addCommand = new DelegateCommand<string>(
                (s) => { ShowEditVehicleDialog(null); },
                (s) => true // can always execute
            );
            _editCommand = new DelegateCommand<string>(
                (s) => { ShowEditVehicleDialog(SelectedVehicle); },
                (s) => { return SelectedVehicle != null; } // execute if a vehicle is selected
            );
            _deleteCommand = new DelegateCommand<string>(
                (s) => { ShowDeleteVehicleDialog(); },
                (s) => { return SelectedVehicle != null; } // execute if a vehicle is selected
            );
            _viewCommand = new DelegateCommand<string>(
                (s) => { ShowVehicleDetailsDialog(SelectedVehicle); },
                (s) => { return SelectedVehicle != null; } // execute if a vehicle is selected
            );
            _rentCommand = new DelegateCommand<string>(
                (s) => { ShowRentVehicleDialog(SelectedVehicle); },
                (s) => {
                    // execute if a vehicle is selected and is available
                    if (SelectedVehicle == null) return false;
                    return SelectedVehicle.Status == "Available";
                }
            );
            _returnCommand = new DelegateCommand<string>(
                (s) => { ShowReturnVehicleDialog(SelectedVehicle); },
                (s) => {
                    // execute if a vehicle is selected and is rented out (may or may not be overdue)
                    if (SelectedVehicle == null) return false;
                    return SelectedVehicle.Status.StartsWith("Rent");
                }
            );
            _serviceCommand = new DelegateCommand<string>(
                (s) => { ShowServiceVehicleConfirmation(SelectedVehicle); },
                (s) => { return SelectedVehicle != null; } // execute if a vehicle is selected
            );
            _exitCommand = new DelegateCommand<string>(
                (s) => { OnRequestClose(); },
                (s) => true // can always execute
            );
        }

        /// <summary>
        /// Public getter and setter for selected vehicle
        /// </summary>
        public Vehicle SelectedVehicle {
            get => _selectedVehicle;
            set {
                _selectedVehicle = value;
                // Recheck commands that need a selected vehicle to execute
                _editCommand.RaiseCanExecuteChanged();
                _deleteCommand.RaiseCanExecuteChanged();
                _viewCommand.RaiseCanExecuteChanged();
                _rentCommand.RaiseCanExecuteChanged();
                _returnCommand.RaiseCanExecuteChanged();
                _serviceCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Public getter and setter for vehicles list
        /// </summary>
        public ObservableCollection<Vehicle> Vehicles
        {
            get => _vehicleList;
            set { _vehicleList = value; }
        }

        /// <summary>
        /// Public getter for add vehicle command
        /// </summary>
        public DelegateCommand<string> AddVehicleClickCommand
        {
            get { return _addCommand; }
        }

        /// <summary>
        /// Public getter for edit vehicle command
        /// </summary>
        public DelegateCommand<string> EditVehicleClickCommand
        {
            get { return _editCommand; }
        }

        /// <summary>
        /// Public getter for delete vehicle command
        /// </summary>
        public DelegateCommand<string> DeleteVehicleClickCommand
        {
            get { return _deleteCommand; }
        }

        /// <summary>
        /// Public getter for view vehicle command
        /// </summary>
        public DelegateCommand<string> ViewVehicleClickCommand
        {
            get => _viewCommand;
        }

        /// <summary>
        /// Public getter for rent vehicle command
        /// </summary>
        public DelegateCommand<string> RentVehicleClickCommand
        {
            get => _rentCommand;
        }

        /// <summary>
        /// Public getter for return vehicle command
        /// </summary>
        public DelegateCommand<string> ReturnVehicleClickCommand
        {
            get => _returnCommand;
        }

        /// <summary>
        /// Public getter for service vehicle command
        /// </summary>
        public DelegateCommand<string> ServiceVehicleClickCommand
        {
            get => _serviceCommand;
        }


        /// <summary>
        /// Public getter for exit vehicle command
        /// </summary>
        public DelegateCommand<string> ExitCommand
        {
            get => _exitCommand;
        }


        // Functions to show dialogs with MVVM. Based on: https://www.c-sharpcorner.com/article/how-to-open-a-child-window-from-view-model-in-mvvm-in-wpf2/

        /// <summary>
        /// Show the edit vehicle view as a dialog
        /// </summary>
        /// <param name="vehicleToEdit">Vehicle to edit, or null to add a new vehicle</param>
        private void ShowEditVehicleDialog(Vehicle vehicleToEdit)
        {
            this.editVehicleWin = new EditVehicleView(vehicleToEdit, ref eventAggregator);
            editVehicleWin.ShowDialog();
        }

        /// <summary>
        /// Show a delete confirmation message box
        /// </summary>
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

        /// <summary>
        /// Shoe the "view vehicle" view as a dialog
        /// </summary>
        /// <param name="vehicle">Vehicle to show details of</param>
        public void ShowVehicleDetailsDialog(Vehicle vehicle)
        {
            viewVehicleWin = new VehicleDetailsView(vehicle);
            viewVehicleWin.ShowDialog();
        }

        /// <summary>
        /// Show the rent vehicle view as a dialog
        /// </summary>
        /// <param name="vehicleToRent">Vehicle to rent</param>
        private void ShowRentVehicleDialog(Vehicle vehicleToRent)
        {
            this.rentVehicleWin = new RentVehicleView(vehicleToRent, ref eventAggregator);
            rentVehicleWin.ShowDialog();
        }

        /// <summary>
        /// Show the return vehicle view as a dialog
        /// </summary>
        /// <param name="vehicleToReturn">Vehicle to return</param>
        private void ShowReturnVehicleDialog(Vehicle vehicleToReturn)
        {
            this.returnVehicleWin = new ReturnVehicleView(vehicleToReturn, ref eventAggregator);
            returnVehicleWin.ShowDialog();
        }

        /// <summary>
        /// Show a confirmation box for servicing a vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        private void ShowServiceVehicleConfirmation(Vehicle vehicle)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(
                "Mark this vehicle as serviced?",
                "Service confirmation",
                MessageBoxButton.YesNo
            );
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                vehicle.recordService();
                SelectedVehicle = vehicle;
            }
        }

        /// <summary>
        /// Handle messages sent from other view models (via the event aggregator)
        /// </summary>
        /// <param name="obj">Message sent</param>
        public override void Handle(Message obj)
        {
            bool updated = obj.Updated; // Vehicle has been updated, but basic details haven't been edited (e.g. it was rented out)
            Vehicle v = obj.Vehicle; // Vehicle the message refers to
            Vehicle old = obj.OldVehicle; // Previous vehicle object, that was edited

            if (old == null && !updated)
            {
                // This is a new vehicle to add
                Vehicles.Add(v);
                return;
            }

            // Find the vehicle that was either updated or edited
            int selectedVehicleIndex = -1;
            for (int i = 0; i < Vehicles.Count; i++)
            {
                if (Vehicles[i].printDetails() == (old == null ? v : old).printDetails())
                {
                    selectedVehicleIndex = i;
                    break;
                }
            }
            // Replace the found vehicle with the updated/edited vehicle
            Vehicles[selectedVehicleIndex] = v;
        }

        /// <summary>
        /// Handles requests to close the view
        /// </summary>
        protected void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
