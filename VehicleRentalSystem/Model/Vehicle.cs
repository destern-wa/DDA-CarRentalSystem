using System;
using System.ComponentModel;

namespace VehicleRentalSystem
{
    public class Vehicle : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string manufacturer;
        private string model;
        private int makeYear;
        private string registration;
        private int odometer;
        private double tankCapacity;
        private bool hasTank;

        public string Manufacturer
        {
            get => manufacturer;
            set
            {
                manufacturer = value;
                OnPropertyChanged("Manufacturer");
            }
        }
        public string Model
        {
            get => model;
            set
            {
                model = value;
                OnPropertyChanged("Model");
            }
        }
        public int Year
        {
            get => makeYear;
            set
            {
                makeYear = value;
                OnPropertyChanged("Year");
            }
        }
        public string Registration {
            get => registration;
            set {
                registration = value;
                OnPropertyChanged("Registration");
            }
        }
        public int Odometer
        {
            get => odometer;
            set
            {
                odometer = value;
                OnPropertyChanged("Odometer");
            }
        }
        public double TankCapacity
        {
            get => tankCapacity;
            set
            {
                tankCapacity = value;
                OnPropertyChanged("TankCapacity");
            }
        }
        public bool HasTank
        {
            get => hasTank;
            set
            {
                hasTank = value;
                OnPropertyChanged("HasTank");
            }
        }

        private FuelPurchase fuelPurchase;

        /// <summary>
        /// Class constructor for a vehicle without a fuel tank  (e.g. electric vehicles)
        /// </summary>
        /// <param name="manufacturer"></param>
        /// <param name="model"></param>
        /// <param name="makeYear"></param>
        /// <param name="registration"></param>
        /// <param name="odometer"></param>
        public Vehicle(string manufacturer, string model, int makeYear, string registration, int odometer)
            : this(manufacturer, model, makeYear, registration, odometer, null) { }

        /// <summary>
        /// Class constructor for a vehicle
        /// </summary>
        /// <param name="manufacturer"></param>
        /// <param name="model"></param>
        /// <param name="makeYear"></param>
        /// <param name="registration"></param>
        /// <param name="odometer"></param>
        /// <param name="tankCapacity"></param>
        public Vehicle(string manufacturer, string model, int makeYear, string registration, int odometer, double? tankCapacity)
        {
            // Validate manufacturer
            if (string.IsNullOrWhiteSpace(manufacturer))
            {
                throw new Exception("Manufacture must be specified");
            }
            // Validate model
            if (string.IsNullOrWhiteSpace(model))
            {
                throw new Exception("Model must be specified");
            }
            // Validate makeYear
            const int OLDEST_MAKE_YEAR = 1900;
            int currentYear = DateTime.Today.Year;
            if (makeYear < OLDEST_MAKE_YEAR)
            {
                throw new Exception("Invalid year of make (too old)");
            }
            else if (makeYear > currentYear)
            {
                throw new Exception("Invalid year of make (can not be in the future)");
            }


            this.manufacturer = manufacturer;
            this.model = model;
            this.makeYear = makeYear;
            this.registration = registration;
            this.odometer = odometer;
            if (tankCapacity != null)
            {
                this.tankCapacity = (double)tankCapacity;
            }
            fuelPurchase = new FuelPurchase();
        }

        /// <summary>
        /// Prints details for <see cref="Vehicle"/>
        /// </summary>
        public string printDetails()
        {
            return "Vehicle: " + makeYear + " " + manufacturer + " " + model;
            // TODO Display additional information about this vehicle
        }


        // TODO Create an addKilometers method which takes a parameter for distance travelled 
        // and adds it to the odometer reading.

        // adds fuel to the car
        public void addFuel(double litres, double price)
        {
            fuelPurchase.purchaseFuel(litres, price);
        }
    }
}