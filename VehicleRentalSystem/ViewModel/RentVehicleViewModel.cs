using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalSystem.ViewModel
{
    /// <summary>
    /// View model for the rent vehicle view
    /// </summary>
    class RentVehicleViewModel : ViewModelBase
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
        /// Vehicle to be rented
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
        /// Error message for To Date
        /// </summary>
        private string _errorMessageToDate;
        /// <summary>
        /// Getter/setter methods for error message for to date field
        /// </summary>
        public string ErrorMessageToDate
        {
            get => _errorMessageToDate;
            set
            {
                SetProperty(ref _errorMessageToDate, value);
            }
        }
        /// <summary>
        /// Error message for From Date
        /// </summary>
        private string _errorMessageFromDate;
        /// <summary>
        /// Getter/setter methods for error message for from date field
        /// </summary>
        public string ErrorMessageFromDate
        {
            get => _errorMessageFromDate;
            set
            {
                SetProperty(ref _errorMessageFromDate, value);
            }
        }

        /// <summary>
        /// Date to rent from
        /// </summary>
        private DateTime _fromDate;
        /// <summary>
        /// Public getter/setter methods for date to rent from
        /// </summary>
        public DateTime FromDate
        {
            get => _fromDate;
            set
            {
                SetProperty(ref _fromDate, value);
            }
        }

        /// <summary>
        /// Date to rent to (expected date of return)
        /// </summary>
        private DateTime _toDate;
        /// <summary>
        /// Public getter/setter methods for date to rent to
        /// </summary>
        public DateTime ToDate
        {
            get => _toDate;
            set
            {
                SetProperty(ref _toDate, value);
            }
        }

        /// <summary>
        /// Is renting by day (false is renting by kilometre)
        /// </summary>
        private bool _isRentByDay;
        /// <summary>
        /// Public getter/setter methods to set to renting by day
        /// </summary>
        public bool IsRentByDay
        {
            get => _isRentByDay;
            set
            {
                SetProperty(ref _isRentByDay, value);
            }
        }
        /// <summary>
        /// Public getter/setter methods to set to renting by kilometer
        /// </summary>
        public bool IsRentByKm
        {
            get => !_isRentByDay;
            set
            {
                SetProperty(ref _isRentByDay, !value);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vehicle">Vehicle to rent</param>
        /// <param name="eventAggregator">Event aggregator for passing messages to other view models</param>
        public RentVehicleViewModel(Vehicle vehicle, ref EventAggregator eventAggregator)
        {
            ErrorMessage = "";
            this.vehicle = vehicle;
            this.eventAggregator = eventAggregator;
            VehicleName = vehicle.printDetails();
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
            IsRentByDay = true;
            _saveCommand = new DelegateCommand<string>(
                (s) =>
                {
                    bool saved = SaveRental();
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
            // Reset any old error message set from SaveRental() method
            ErrorMessage = "";
            // Validate rent from date
            ErrorMessageFromDate = FromDate > DateTime.Now ? "Rental date can not be in the future" : "";
            // Validate rent to date
            ErrorMessageToDate = FromDate > ToDate ? "Vehicle can not be due back before it is rented" : "";

            // Is valid if both error messages are empty
            return ErrorMessageFromDate == "" && ErrorMessageToDate == "";
        }

        /// <summary>
        /// Save the rental
        /// </summary>
        /// <returns>True if it was saved, false if there was an error</returns>
        private bool SaveRental()
        {
            bool isValid = validate();
            if (!isValid) return false;
            try
            {
                Rental r = new Rental(FromDate, ToDate, IsRentByDay);
                vehicle.AddRental(r);
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
