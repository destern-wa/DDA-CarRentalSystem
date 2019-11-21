using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalSystem.ViewModel
{
    /// <summary>
    /// View model for "view vehicle" view
    /// </summary>
    class VehicleDetailsViewModel : ViewModelBase
    {
        /// <summary>
        /// Command for closing
        /// </summary>
        private readonly DelegateCommand<string> _closeCommand;

        // MVVM window closing per Andrew's comment in https://social.msdn.microsoft.com/Forums/en-US/17aabea0-4aca-478f-9205-fcd56080b22a/how-to-close-a-window-by-clicking-the-button-using-mvvm?forum=wpf
        /// <summary>
        /// Handler to call to tell the view to close
        /// </summary>
        public event EventHandler RequestClose;

        // Private and public properties for the view to use in bindings

        /// <summary>
        /// Manufacturer, model, and year
        /// </summary>
        private string _manModelYear;
        /// <summary>
        /// Getter/setter for Manufacturer, model, and year
        /// </summary>
        public string ManModelYear
        {
            get => _manModelYear;
            set => SetProperty(ref _manModelYear, value);
        }
        /// <summary>
        /// Registration (licence plate) number
        /// </summary>
        private string _registartion;
        /// <summary>
        /// Getter/setter for Registration (licence plate) number
        /// </summary>
        public string Registration
        {
            get => _registartion;
            set => SetProperty(ref _registartion, value);
        }
        /// <summary>
        /// Total kilometers travelled
        /// </summary>
        private string _totalKm;
        /// <summary>
        /// Getter/setter for total kilometers travelled
        /// </summary>
        public string TotalKm
        {
            get => _totalKm;
            set => SetProperty(ref _totalKm, value);
        }
        /// <summary>
        /// Number of services
        /// </summary>
        private string _services;
        /// <summary>
        /// Getter/setter for number of services
        /// </summary>
        public string Services
        {
            get => _services;
            set => SetProperty(ref _services, value);
        }
        /// <summary>
        /// Revenue ($)
        /// </summary>
        private string _revenue;
        /// <summary>
        /// Getter/setter for Revenue ($)
        /// </summary>
        public string Revenue
        {
            get => _revenue;
            set => SetProperty(ref _revenue, value);
        }
        /// <summary>
        /// Kilometres driven since the last service
        /// </summary>
        private string _kmSinceLastService;
        /// <summary>
        /// Getter/setter for Kilometres driven since the last service
        /// </summary>
        public string KmSinceLastService
        {
            get => _kmSinceLastService;
            set => SetProperty(ref _kmSinceLastService, value);
        }
        /// <summary>
        /// Fuel economy
        /// </summary>
        private string _fuelEconomy;
        /// <summary>
        /// Getter/setter for Fuel economy
        /// </summary>
        public string FuelEconomy
        {
            get => _fuelEconomy;
            set => SetProperty(ref _fuelEconomy, value);
        }
        /// <summary>
        /// Vehicle requires a service (yes/no)
        /// </summary>
        private string _requiresService;
        /// <summary>
        /// Getter/setter for Vehicle requires a service (yes/no)
        /// </summary>
        public string RequiresService
        {
            get => _requiresService;
            set => SetProperty(ref _requiresService, value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vehicle">Vehicle to show details of</param>
        public VehicleDetailsViewModel(Vehicle vehicle)
        {
            _closeCommand = new DelegateCommand<string>(
                (execute) => { OnRequestClose(); },
                (canExcute) => { return true; }
            );

            ManModelYear = vehicle.Manufacturer + " " + vehicle.Model + " " + vehicle.Year.ToString();
            Registration = vehicle.Registration;
            TotalKm = vehicle.Odometer.ToString() + " km";
            Services = vehicle.getServicesCount.ToString();
            Revenue = String.Format("{0:C2}", vehicle.calculateRevenue()); // Format as currency
            KmSinceLastService = vehicle.getKmSinceLastService().ToString();
            FuelEconomy = vehicle.calculateFuelEconomy();
            RequiresService = vehicle.needsService() ? "Yes" : "No";
        }

        /// <summary>
        /// Public getter for close command
        /// </summary>
        public DelegateCommand<string> CloseCommand
        {
            get => _closeCommand;
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
