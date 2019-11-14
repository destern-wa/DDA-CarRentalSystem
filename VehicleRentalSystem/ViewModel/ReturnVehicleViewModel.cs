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

        private bool SaveReturn()
        {
            try
            {
                double km = double.Parse(KmTravelled);
                vehicle.ReturnRental(ReturnDate, km);
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
