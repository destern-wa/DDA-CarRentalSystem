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
        // TODO add Registration Number 
        // TODO add variable for OdometerReading (in KM), 
        // TODO add variable for TankCapacity (in litres)

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
        public string Registration { get; set; }
        public string Odometer { get; set; }
        public string TankCapacity { get; set; }



        private FuelPurchase fuelPurchase;

        /// <summary>
        /// Class constructor specifying name of make (manufacturer), model and year
        /// of make.
        /// </summary>
        /// <param name="manufacturer"></param>
        /// <param name="model"></param>
        /// <param name="makeYear"></param>
        public Vehicle(string manufacturer, string model, int makeYear)
        {
            if (string.IsNullOrWhiteSpace(manufacturer))
            {
                throw new Exception("Manufacture must be specified");
            }
            if (string.IsNullOrWhiteSpace(model))
            {
                throw new Exception("Model must be specified");
            }
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
            fuelPurchase = new FuelPurchase();
        }

        // TODO Add missing getter and setter methods

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