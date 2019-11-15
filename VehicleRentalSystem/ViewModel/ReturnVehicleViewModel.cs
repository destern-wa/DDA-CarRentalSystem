using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalSystem.ViewModel
{
    class ReturnVehicleViewModel : ViewModelBase
    {
        private EventAggregator eventAggregator;
        private readonly DelegateCommand<string> _saveCommand;
        private readonly DelegateCommand<string> _cancelCommand;

        // MVVM window closing per Andrew's comment in https://social.msdn.microsoft.com/Forums/en-US/17aabea0-4aca-478f-9205-fcd56080b22a/how-to-close-a-window-by-clicking-the-button-using-mvvm?forum=wpf
        public event EventHandler RequestClose;
        private Vehicle vehicle;

        private string _vehicleName;
        public string VehicleName
        {
            get => _vehicleName;
            set
            {
                SetProperty(ref _vehicleName, value);
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
            }
        }
        private string _errorMessageDate;
        public string ErrorMessageDate
        {
            get => _errorMessageDate;
            set
            {
                SetProperty(ref _errorMessageDate, value);
            }
        }
        private string _errorMessageKm;
        public string ErrorMessageKm
        {
            get => _errorMessageKm;
            set
            {
                SetProperty(ref _errorMessageKm, value);
            }
        }
        private string _errorMessageFuel;
        public string ErrorMessageFuel
        {
            get => _errorMessageFuel;
            set
            {
                SetProperty(ref _errorMessageFuel, value);
            }
        }

        private DateTime _returnDate;
        public DateTime ReturnDate
        {
            get => _returnDate;
            set
            {
                SetProperty(ref _returnDate, value);
            }
        }

        private string _kmTravelled;
        public string KmTravelled
        {
            get => _kmTravelled;
            set
            {
                SetProperty(ref _kmTravelled, value);
            }
        }
        private string _fuelAmount;
        public string FuelAmount
        {
            get => _fuelAmount;
            set
            {
                SetProperty(ref _fuelAmount, value);
            }
        }
        private string _fuelCost;
        public string FuelCost
        {
            get => _fuelCost;
            set
            {
                SetProperty(ref _fuelCost, value);
            }
        }

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

            // Validate fuel
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

            return ErrorMessageDate == "" && ErrorMessageKm == "" && ErrorMessageFuel == "";

        }

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

        protected void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
