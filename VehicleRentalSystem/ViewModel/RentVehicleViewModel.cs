using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalSystem.ViewModel
{
    class RentVehicleViewModel : ViewModelBase
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

        private DateTime _fromDate;
        public DateTime FromDate
        {
            get => _fromDate;
            set
            {
                SetProperty(ref _fromDate, value);
            }
        }

        private DateTime _toDate;
        public DateTime ToDate
        {
            get => _toDate;
            set
            {
                SetProperty(ref _toDate, value);
            }
        }

        private bool _isRentByDay;
        public bool IsRentByDay
        {
            get => _isRentByDay;
            set
            {
                SetProperty(ref _isRentByDay, value);
            }
        }
        public bool IsRentByKm
        {
            get => !_isRentByDay;
            set
            {
                SetProperty(ref _isRentByDay, !value);
            }
        }

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

        public DelegateCommand<string> SaveCommand
        {
            get => _saveCommand;
        }

        public DelegateCommand<string> CancelCommand
        {
            get => _cancelCommand;
        }

        private bool SaveRental()
        {
            try
            {
                Rental r = new Rental(FromDate, ToDate, IsRentByDay);
                this.eventAggregator.Publish(new Message { Vehicle = vehicle, Rental = r });
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
