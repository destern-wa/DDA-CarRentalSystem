using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalSystem.ViewModel
{
    class VehicleDetailsViewModel : ViewModelBase
    {
        private readonly DelegateCommand<string> _closeCommand;

        // MVVM window closing per Andrew's comment in https://social.msdn.microsoft.com/Forums/en-US/17aabea0-4aca-478f-9205-fcd56080b22a/how-to-close-a-window-by-clicking-the-button-using-mvvm?forum=wpf
        public event EventHandler RequestClose;

        //private Vehicle vehicle;

        private string _manModelYear;
        public string ManModelYear
        {
            get => _manModelYear;
            set => SetProperty(ref _manModelYear, value);
        }

        private string _registartion;
        public string Registration
        {
            get => _registartion;
            set => SetProperty(ref _registartion, value);
        }

        private string _totalKm;
        public string TotalKm
        {
            get => _totalKm;
            set => SetProperty(ref _totalKm, value);
        }

        private string _services;
        public string Services
        {
            get => _services;
            set => SetProperty(ref _services, value);
        }


        private string _revenue;
        public string Revenue
        {
            get => _revenue;
            set => SetProperty(ref _revenue, value);
        }

        private string _kmSinceLastService;
        public string KmSinceLastService
        {
            get => _kmSinceLastService;
            set => SetProperty(ref _kmSinceLastService, value);
        }

        private string _fuelEconomy;
        public string FuelEconomy
        {
            get => _fuelEconomy;
            set => SetProperty(ref _fuelEconomy, value);
        }

        private string _requiresService;
        public string RequiresService
        {
            get => _requiresService;
            set => SetProperty(ref _requiresService, value);
        }

        public VehicleDetailsViewModel(Vehicle vehicle)
        {
           // this.vehicle = vehicle;
            _closeCommand = new DelegateCommand<string>(
                (execute) => { OnRequestClose(); },
                (canExcute) => { return true; }
            );

            ManModelYear = vehicle.Manufacturer + " " + vehicle.Model + " " + vehicle.Year.ToString();
            Registration = vehicle.Registration;
            TotalKm = vehicle.Odometer.ToString() + " km";
            Services = vehicle.getServicesCount.ToString();
            Revenue = String.Format("{0:C2}", vehicle.calculateRevenue());
            KmSinceLastService = vehicle.getKmSinceLastService().ToString();
            FuelEconomy = vehicle.calculateFuelEconomy();
            RequiresService = vehicle.needsService() ? "Yes" : "No";
        }

        public DelegateCommand<string> CloseCommand
        {
            get => _closeCommand;
        }

        protected void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
