using System;

namespace VehicleRentalSystem.ViewModel
{
    /// <summary>
    /// View model for EditVehicleView
    /// </summary>
    class EditVehicleViewModel : ViewModelBase
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
        /// Original Vehicle object for the vehicle being edited
        /// </summary>
        private Vehicle oldVehicle;

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
        /// Error message for Make field
        /// </summary>
        private string _errorMessageMake;
        /// <summary>
        /// Getters/setters for Error message for Make field
        /// </summary>
        public string ErrorMessageMake
        {
            get => _errorMessageMake;
            set
            {
                SetProperty(ref _errorMessageMake, value);
            }
        }
        /// <summary>
        /// Error message for Model field
        /// </summary>
        private string _errorMessageModel;
        /// <summary>
        /// Getters/setters for Error message for Model field
        /// </summary>
        public string ErrorMessageModel
        {
            get => _errorMessageModel;
            set
            {
                SetProperty(ref _errorMessageModel, value);
            }
        }
        /// <summary>
        /// Error message for year field
        /// </summary>
        private string _errorMessageYear;
        /// <summary>
        /// Getters/setters for Error message for Year field
        /// </summary>
        public string ErrorMessageYear
        {
            get => _errorMessageYear;
            set
            {
                SetProperty(ref _errorMessageYear, value);
            }
        }
        /// <summary>
        /// Error message for registration field
        /// </summary>
        private string _errorMessageRegistration;
        /// <summary>
        /// Getters/setters for Error message for registration field
        /// </summary>
        public string ErrorMessageRegistration
        {
            get => _errorMessageRegistration;
            set
            {
                SetProperty(ref _errorMessageRegistration, value);
            }
        }
        /// <summary>
        /// Error message for odometer field
        /// </summary>
        private string _errorMessageOdometer;
        /// <summary>
        /// Getters/setters for Error message for odometer field
        /// </summary>
        public string ErrorMessageOdometer
        {
            get => _errorMessageOdometer;
            set
            {
                SetProperty(ref _errorMessageOdometer, value);
            }
        }
        /// <summary>
        /// Error message for tank capacity field
        /// </summary>
        private string _errorMessageTankCapacity;
        /// <summary>
        /// Getters/setters for Error message for tank capacity field
        /// </summary>
        public string ErrorMessageTankCapacity
        {
            get => _errorMessageTankCapacity;
            set
            {
                SetProperty(ref _errorMessageTankCapacity, value);
            }
        }

        /// <summary>
        /// Name of vehicle make
        /// </summary>
        private string _makeName;
        /// <summary>
        /// Getters/setters for name of vehcile make
        /// </summary>
        public string MakeName
        {
            get => _makeName;
            set
            {
                SetProperty(ref _makeName, value);
            }
        }
        /// <summary>
        /// Name of vehicle model
        /// </summary>
        private string _modelName;
        /// <summary>
        /// Getters/setters for name of vehicle model
        /// </summary>
        public string ModelName
        {
            get => _modelName;
            set
            {
                SetProperty(ref _modelName, value);
            }
        }
        /// <summary>
        /// Year of vehicle
        /// </summary>
        private string _year;
        /// <summary>
        /// Getetrs/setters for year of vehicle
        /// </summary>
        public string Year
        {
            get => _year;
            set
            {
                SetProperty(ref _year, value);
            }
        }
        /// <summary>
        /// Vehicle registration number
        /// </summary>
        private string _registration;
        /// <summary>
        /// Getters/senders for Vehicle registration number
        /// </summary>
        public string Registration
        {
            get => _registration;
            set
            {
                SetProperty(ref _registration, value);
            }
        }
        /// <summary>
        /// Vehicle odometer
        /// </summary>
        private string _odometer;
        /// <summary>
        /// Getters/setters for vehicle odometer
        /// </summary>
        public string Odometer
        {
            get => _odometer;
            set
            {
                SetProperty(ref _odometer, value);
            }
        }
        /// <summary>
        /// Vehicle's tank capacity
        /// </summary>
        private string _tankCapacity;
        /// <summary>
        /// Getters/setters for vehicle tank capacity
        /// </summary>
        public string TankCapacity
        {
            get => _tankCapacity;
            set
            {
                SetProperty(ref _tankCapacity, value);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oldVehicle">Vehicle to be edited, or null if adding a new vehicle</param>
        /// <param name="eventAggregator">Event aggregator for passing messages to other view models</param>
        public EditVehicleViewModel(Vehicle oldVehicle, ref EventAggregator eventAggregator)
        {
            ErrorMessage = "";
            this.oldVehicle = oldVehicle;
            if (oldVehicle != null)
            {
                MakeName = oldVehicle.Manufacturer;
                ModelName = oldVehicle.Model;
                Year = oldVehicle.Year.ToString();
                Registration = oldVehicle.Registration;
                Odometer = oldVehicle.Odometer.ToString();
                TankCapacity = oldVehicle.HasTank ? oldVehicle.TankCapacity.ToString() : "";
            }

            this.eventAggregator = eventAggregator;
            _saveCommand = new DelegateCommand<string>(
                (s) =>
                {
                    bool saved = SaveVehicle();
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
            // Reset any old error message set from SaveVehicle() method
            ErrorMessage = ""; 

            // Validate make
            ErrorMessageMake = String.IsNullOrWhiteSpace(MakeName) ? "Make name is required" : "";

            // Validate model
            ErrorMessageModel = String.IsNullOrWhiteSpace(ModelName) ? "Model name is required" : "";

            // Validate year
            int yearInt = -1;
            ErrorMessageYear = String.IsNullOrWhiteSpace(Year)
                ? "Year is required"
                : (int.TryParse(Year, out yearInt) ? "" : "Year must be an integer numer");
            if (ErrorMessageYear == "")
            {
                ErrorMessageYear = yearInt < 1900
                    ? "Invalid year (too old)"
                    : (yearInt > DateTime.Now.Year ? "Year can not be in the future" : "");
            }

            // Validate registration
            ErrorMessageRegistration = String.IsNullOrWhiteSpace(Registration) ? "Registration is required" : "";

            // Validate odometer
            int odometerInt = -1;
            ErrorMessageOdometer = String.IsNullOrWhiteSpace(Odometer)
                ? "Odometer reading is required"
                : (int.TryParse(Odometer, out odometerInt) ? "" : "Odometer must be an ineteger number");
            if (ErrorMessageOdometer == "")
            {
                ErrorMessageOdometer = odometerInt < 0 ? "Odometer reading can not be negative" : "";
            }

            // Validate trank capacity
            bool hasTank = !(String.IsNullOrWhiteSpace(TankCapacity));
            if (hasTank)
            {
                double tankCapacityNum = -1;
                ErrorMessageTankCapacity = double.TryParse(TankCapacity, out tankCapacityNum) ? "" : "Odometer must be numeric";
                if (ErrorMessageTankCapacity == "")
                {
                    ErrorMessageTankCapacity = tankCapacityNum < 0 ? "Tank capacity can not be negative" : "";
                }
            } else {
                ErrorMessageTankCapacity = "";
            }

            // Is valid if all error messages are empty
            return (
                ErrorMessageMake == "" &&
                ErrorMessageModel == "" &&
                ErrorMessageYear == "" &&
                ErrorMessageRegistration == "" &&
                ErrorMessageOdometer == "" &&
                ErrorMessageTankCapacity == ""
            );
        }

        /// <summary>
        /// Save the changes made
        /// </summary>
        /// <returns>True if changes were saved, false if there was an error</returns>
        private bool SaveVehicle()
        {
            bool valid = validate();
            if (!valid) return false;
            try
            {
                Vehicle v;
                bool hasTank = !String.IsNullOrWhiteSpace(TankCapacity);

                if (hasTank) {
                    v = new Vehicle(MakeName, ModelName, int.Parse(Year), Registration, int.Parse(Odometer), Double.Parse(TankCapacity));
                } else
                {
                    v = new Vehicle(MakeName, ModelName, int.Parse(Year), Registration, int.Parse(Odometer));
                }
                this.eventAggregator.Publish(new Message { Vehicle = v, OldVehicle = oldVehicle });
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
