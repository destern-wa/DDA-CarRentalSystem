using System;

namespace VehicleRentalSystem.ViewModel
{
    class EditVehicleViewModel : ViewModelBase
    {
        private EventAggregator eventAggregator;
        private readonly DelegateCommand<string> _saveCommand;
        private readonly DelegateCommand<string> _cancelCommand;

        // MVVM window closing per Andrew's comment in https://social.msdn.microsoft.com/Forums/en-US/17aabea0-4aca-478f-9205-fcd56080b22a/how-to-close-a-window-by-clicking-the-button-using-mvvm?forum=wpf
        public event EventHandler RequestClose;
        private Vehicle oldVehicle;

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
            }
        }

        private string _errorMessageMake;
        public string ErrorMessageMake
        {
            get => _errorMessageMake;
            set
            {
                SetProperty(ref _errorMessageMake, value);
            }
        }
        private string _errorMessageModel;
        public string ErrorMessageModel
        {
            get => _errorMessageModel;
            set
            {
                SetProperty(ref _errorMessageModel, value);
            }
        }
        private string _errorMessageYear;
        public string ErrorMessageYear
        {
            get => _errorMessageYear;
            set
            {
                SetProperty(ref _errorMessageYear, value);
            }
        }
        private string _errorMessageRegistration;
        public string ErrorMessageRegistration
        {
            get => _errorMessageRegistration;
            set
            {
                SetProperty(ref _errorMessageRegistration, value);
            }
        }
        private string _errorMessageOdometer;
        public string ErrorMessageOdometer
        {
            get => _errorMessageOdometer;
            set
            {
                SetProperty(ref _errorMessageOdometer, value);
            }
        }
        private string _errorMessageTankCapacity;
        public string ErrorMessageTankCapacity
        {
            get => _errorMessageTankCapacity;
            set
            {
                SetProperty(ref _errorMessageTankCapacity, value);
            }
        }

        private string _makeName;
        public string MakeName
        {
            get => _makeName;
            set
            {
                SetProperty(ref _makeName, value);
            }
        }
        private string _modelName;
        public string ModelName
        {
            get => _modelName;
            set
            {
                SetProperty(ref _modelName, value);
            }
        }
        private string _year;
        public string Year
        {
            get => _year;
            set
            {
                SetProperty(ref _year, value);
            }
        }
        private string _registration;
        public string Registration
        {
            get => _registration;
            set
            {
                SetProperty(ref _registration, value);
            }
        }
        private string _odometer;
        public string Odometer
        {
            get => _odometer;
            set
            {
                SetProperty(ref _odometer, value);
            }
        }
        private string _tankCapacity;
        public string TankCapacity
        {
            get => _tankCapacity;
            set
            {
                SetProperty(ref _tankCapacity, value);
            }
        }

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


        public DelegateCommand<string> SaveCommand
        {
            get => _saveCommand;
        }
        public DelegateCommand<string> CancelCommand
        {
            get => _cancelCommand;
        }

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

        protected void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
