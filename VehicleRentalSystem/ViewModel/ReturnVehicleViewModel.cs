using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalSystem.ViewModel
{
    /// <summary>
    /// View model for retrurn vehicle view
    /// </summary>
    class ReturnVehicleViewModel : ViewModelBase
    {
        /// <summary>
        /// Event aggregator for passing messages to other view models
        /// </summary>
        private EventAggregator eventAggregator;
        /// <summary>
        /// Command for saving
        /// </summary>
        private readonly DelegateCommand<string> _saveCommand;
        /// <summary>
        /// Command for closing
        /// </summary>
        private readonly DelegateCommand<string> _cancelCommand;

        // MVVM window closing per Andrew's comment in https://social.msdn.microsoft.com/Forums/en-US/17aabea0-4aca-478f-9205-fcd56080b22a/how-to-close-a-window-by-clicking-the-button-using-mvvm?forum=wpf
        /// <summary>
        /// Handler to call to tell the view to close
        /// </summary>
        public event EventHandler RequestClose;

        /// <summary>
        /// Vehicle to be returned
        /// </summary>
        private Vehicle vehicle;

        // Private and public properties for the view to use in bindings

        /// <summary>
        /// Vehicle name
        /// </summary>
        private string _vehicleName;
        /// <summary>
        /// Getter/setter methods for vehicle name 
        /// </summary>
        public string VehicleName
        {
            get => _vehicleName;
            set
            {
                SetProperty(ref _vehicleName, value);
            }
        }

        /// <summary>
        /// Error message to show below buttons
        /// </summary>
        private string _errorMessage;
        /// <summary>
        /// Getter/setter methods for errorMessage
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
            }
        }
        /// <summary>
        /// Error message for date
        /// </summary>
        private string _errorMessageDate;
        /// <summary>
        /// Getter/setter methods for error message for date field
        /// </summary>
        public string ErrorMessageDate
        {
            get => _errorMessageDate;
            set
            {
                SetProperty(ref _errorMessageDate, value);
            }
        }
        /// <summary>
        /// Error message for kilometres
        /// </summary>
        private string _errorMessageKm;
        /// <summary>
        /// Getter/setter methods for error message for kilometres
        /// </summary>
        public string ErrorMessageKm
        {
            get => _errorMessageKm;
            set
            {
                SetProperty(ref _errorMessageKm, value);
            }
        }
        /// <summary>
        /// Error message for fuel purchase
        /// </summary>
        private string _errorMessageFuel;
        /// <summary>
        /// Getter/setter methods for error message for fuel purchase
        /// </summary>
        public string ErrorMessageFuel
        {
            get => _errorMessageFuel;
            set
            {
                SetProperty(ref _errorMessageFuel, value);
            }
        }

        /// <summary>
        /// Date of return
        /// </summary>
        private DateTime _returnDate;
        /// <summary>
        /// Getter/setter methods for date of return
        /// </summary>
        public DateTime ReturnDate
        {
            get => _returnDate;
            set
            {
                SetProperty(ref _returnDate, value);
            }
        }
        /// <summary>
        /// Kilometres travelled
        /// </summary>
        private string _kmTravelled;
        /// <summary>
        /// Getter/setter methods for  kilometres travelled
        /// </summary>
        public string KmTravelled
        {
            get => _kmTravelled;
            set
            {
                SetProperty(ref _kmTravelled, value);
            }
        }
        /// <summary>
        /// Amount of fuel purchased (L)
        /// </summary>
        private string _fuelAmount;
        /// <summary>
        /// Getter/setter for amount of fuel purchased (L)
        /// </summary>
        public string FuelAmount
        {
            get => _fuelAmount;
            set
            {
                SetProperty(ref _fuelAmount, value);
            }
        }
        /// <summary>
        /// Fuel cost ($)
        /// </summary>
        private string _fuelCost;
        /// <summary>
        /// Getter/setter for fuel cost ($)
        /// </summary>
        public string FuelCost
        {
            get => _fuelCost;
            set
            {
                SetProperty(ref _fuelCost, value);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vehicle">Vehicle to be returned</param>
        /// <param name="eventAggregator">Event aggregator for passing messages to other view models</param>
        public ReturnVehicleViewModel(Vehicle vehicle, ref EventAggregator eventAggregator)
        {
            ErrorMessage = "";
            this.vehicle = vehicle;
            this.eventAggregator = eventAggregator;
            VehicleName = vehicle.printDetails();
            ReturnDate = DateTime.Now;
            _saveCommand = new DelegateCommand<string>(
                (s) =>
                {
                    bool saved = SaveReturn();
                    if (saved)
                    {
                        OnRequestClose();
                    }
                }, //Execute
                (s) => { return true; } //CanExecute //TODO: should be based upon inputted values
            );
            _cancelCommand = new DelegateCommand<string>(
                (s) => { OnRequestClose(); },
                (s) => { return true; } //CanExecute -- always
            );
        }

        /// <summary>
        /// Public getter for save command
        /// </summary>
        public DelegateCommand<string> SaveCommand
        {
            get => _saveCommand;
        }

        /// <summary>
        /// Public getter for cancel command
        /// </summary>
        public DelegateCommand<string> CancelCommand
        {
            get => _cancelCommand;
        }

        /// <summary>
        /// Validate properties and set error messages as appropriate
        /// </summary>
        /// <returns>True if everything is valid, false otherwise</returns>
        private bool validate()
        {
            // Reset any old error message set from SaveReturn() method
            ErrorMessage = "";

            // Validate return date
            ErrorMessageDate = ReturnDate > DateTime.Now ? "Return date can not be in the future" : "";

            // Validate km travelled
            double km = -1;
            ErrorMessageKm = String.IsNullOrWhiteSpace(KmTravelled)
                ? "Distance is required"
                : (double.TryParse(KmTravelled, out km) ? "" : "Distance must be numeric");
            if (ErrorMessageKm == "")
            {
                ErrorMessageKm = km <= 0 ? "Distance must be a positive number" : "";
            }

            // Validate fuel amount and cost
            double fuel = -1;
            double cost = -1;
            ErrorMessageFuel = String.IsNullOrWhiteSpace(FuelAmount) || String.IsNullOrWhiteSpace(FuelCost)
                ? "Fuel quantity and cost are required"
                : ( double.TryParse(FuelAmount, out fuel) && double.TryParse(FuelCost, out cost)
                    ? "" : "Values must be numeric" );
            if (ErrorMessageFuel == "")
            {
                ErrorMessageFuel = fuel <= 0 || cost < 0 ? "Amounts must be positive numbers" : "";
            }

            // Is valid if all error messages are empty
            return ErrorMessageDate == "" && ErrorMessageKm == "" && ErrorMessageFuel == "";

        }

        /// <summary>
        /// Save the return
        /// </summary>
        /// <returns>True if it was saved, false if there was an error</returns>
        private bool SaveReturn()
        {
            bool isValid = validate();
            if (!isValid) return false;
            try
            {
                double km = double.Parse(KmTravelled);
                double fuel = double.Parse(FuelAmount);
                double fuelcost = double.Parse(FuelCost);
                vehicle.ReturnRental(ReturnDate, km, fuel, fuelcost);
                this.eventAggregator.Publish(new Message { Vehicle = vehicle, Updated = true });
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
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
