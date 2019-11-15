using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace VehicleRentalSystem
{
    /// <summary>
    /// Class for vehicles which will be rented out
    /// </summary>
    public class Vehicle : INotifyPropertyChanged
    {
        /// <summary>
        /// Event for MVVM-style property change handling
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Method that notifies ViewModel that a property has changed
        /// </summary>
        /// <param name="propertyName">Name of property</param>
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Manufacturer name
        /// </summary>
        private string manufacturer;
        /// <summary>
        /// Model name
        /// </summary>
        private string model;
        /// <summary>
        /// Year of vehicle
        /// </summary>
        private int makeYear;
        /// <summary>
        /// Registration (licence plate) number
        /// </summary>
        private string registration;
        /// <summary>
        /// Odometer reading, in kilometers
        /// </summary>
        private int odometer;
        /// <summary>
        /// Fuel tank capacity, in litres
        /// </summary>
        private double tankCapacity;
        /// <summary>
        /// The vehicle has a fuel tank
        /// </summary>
        private bool hasTank;
        /// <summary>
        /// Rentals for this vehicle
        /// </summary>
        private List<Rental> rentals = new List<Rental>();
        /// <summary>
        /// Getter/setter mthods for manufacturer name
        /// </summary>
        public string Manufacturer
        {
            get => manufacturer;
            set
            {
                manufacturer = value;
                OnPropertyChanged("Manufacturer");
            }
        }
        /// <summary>
        /// Getter/setter mthods for model name
        /// </summary>
        public string Model
        {
            get => model;
            set
            {
                model = value;
                OnPropertyChanged("Model");
            }
        }
        /// <summary>
        /// Getter/setter mthods for year of vehicle
        /// </summary>
        public int Year
        {
            get => makeYear;
            set
            {
                makeYear = value;
                OnPropertyChanged("Year");
            }
        }
        /// <summary>
        /// Getter/setter mthods for registration number
        /// </summary>
        public string Registration {
            get => registration;
            set {
                registration = value;
                OnPropertyChanged("Registration");
            }
        }
        /// <summary>
        /// Getter/setter mthods for odometer reading
        /// </summary>
        public int Odometer
        {
            get => odometer;
            set
            {
                odometer = value;
                OnPropertyChanged("Odometer");
            }
        }
        /// <summary>
        /// Getter/Setter methods for fuel tank capacity, in litres
        /// </summary>
        public double TankCapacity
        {
            get => tankCapacity;
            set
            {
                tankCapacity = value;
                OnPropertyChanged("TankCapacity");
            }
        }
        /// <summary>
        /// Getter/setter methods for if the vehicle has a fuel tank
        /// </summary>
        public bool HasTank
        {
            get => hasTank;
            set
            {
                hasTank = value;
                OnPropertyChanged("HasTank");
            }
        }
        /// <summary>
        /// Fuel level of tank, in litres
        /// </summary>
        private double fuelInTank = 0;
        /// <summary>
        /// Getter/setter methods for fuel level of tank, in litres
        /// </summary>
        public double FuelInTank
        {
            get => fuelInTank;
            set
            {
                fuelInTank = value;
                OnPropertyChanged("FuelInTank");
            }
        }
        /// <summary>
        /// Current status/availability of vehicle
        /// </summary>
        public string Status
        {
            get
            {
                if (needsService()) return "Needs service";
                if (rentals == null) return "Available";
                int countRental = rentals.Count;
                if (countRental == 0)
                {
                    return "Available";
                }
                string rentalStatus = rentals[countRental - 1].status();
                if (rentalStatus == "Returned")
                {
                    return "Available";
                }
                return rentalStatus;
            }
        }

        /// <summary>
        /// Fuel pruchases for the vehicle
        /// </summary>
        private FuelPurchase fuelPurchase;

        /// <summary>
        /// Services for the vehicle
        /// </summary>
        private Service service;

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
            // Validate registration
            if (string.IsNullOrWhiteSpace(registration))
            {
                throw new Exception("Registration must be specified");
            }
            // Validate odometer
            if (odometer < 0)
            {
                throw new Exception("Odometer reading can not be negative");
            }
            // Validate tankCapacity
            if (tankCapacity != null && tankCapacity < 0)
            {
                throw new Exception("Tank capacity reading can not be negative");
            }

            this.manufacturer = manufacturer;
            this.model = model;
            this.makeYear = makeYear;
            this.registration = registration;
            this.odometer = odometer;
            if (tankCapacity != null)
            {
                this.tankCapacity = (double)tankCapacity;
                this.hasTank = true;
            } else
            {
                this.hasTank = false;
            }

            fuelPurchase = new FuelPurchase();
            service = new Service();
        }

        /// <summary>
        /// Prints details for <see cref="Vehicle"/>
        /// </summary>
        public string printDetails()
        {
            return makeYear + " " + manufacturer + " " + model + " (" + registration + ")";
        }

        /// <summary>
        /// Increment the Odometer by a distance
        /// </summary>
        /// <param name="distance">Distance in kilometers</param>
        public void addKilometers(int distance)
        {
            if (distance < 0)
            {
                throw new Exception("Distance to add to odometer must not be negative");
            }
            Odometer += distance;
        }

        /// <summary>
        /// Adds fuel to the car
        /// </summary>
        /// <param name="litres"></param>
        /// <param name="price"></param>
        public void addFuel(double litres, double price)
        {
            fuelPurchase.purchaseFuel(litres, price);
            fuelInTank += litres;
        }

        /// <summary>
        /// Check if car needs fuel
        /// </summary>
        /// <returns>Car needs fuel</returns>
        public bool needsFuel()
        {
            if (!hasTank) return false;
            double FILLED_TANK_TOLERANCE = 5;
            return fuelInTank < (tankCapacity - FILLED_TANK_TOLERANCE);
        }

        /// <summary>
        /// Add a new rental record to the vehicle
        /// </summary>
        /// <param name="r">Rental record</param>
        public void AddRental(Rental r)
        {
            this.rentals.Add(r);
            OnPropertyChanged("Status");
        }

        /// <summary>
        /// Mark the last rental as returned
        /// </summary>
        /// <param name="returnDate">Date of return</param>
        /// <param name="kmTravlled">Kilometres travelled</param>
        /// <param name="fuel">Amount of fuel purchased, in litres</param>
        /// <param name="fuelcost">Cost of fuel, in dollars</param>
        public void ReturnRental(DateTime returnDate, double kmTravlled, double fuel, double fuelcost)
        {
            rentals[rentals.Count - 1].returnVehicle(returnDate, kmTravlled);
            addKilometers((int)kmTravlled);
            addFuel(fuel, fuelcost);
            OnPropertyChanged("Status");
        }

        /// <summary>
        /// Check if the vehicle needs a service
        /// </summary>
        /// <returns>Vehicle needs a service</returns>
        public bool needsService()
        {
            return this.getKmSinceLastService() > Service.SERVICE_KILOMETER_LIMIT;
        }

        /// <summary>
        /// Record that vehcile has been serviced
        /// </summary>
        public void recordService()
        {
            service.recordService(odometer);
            OnPropertyChanged("Status");
        }

        /// <summary>
        /// Get the number of services the vehicle has had
        /// </summary>
        public int getServicesCount => this.service.getServiceCount();

        /// <summary>
        /// Get the number of kilometres travelled since last service
        /// </summary>
        /// <returns>kilometres travelled since last service</returns>
        public int getKmSinceLastService() => odometer - service.getLastServiceOdometerKm();

        /// <summary>
        /// Calculate the revenue from this vehicle's rentals
        /// </summary>
        /// <returns>Rental revenue, in dollars</returns>
        public double calculateRevenue()
        {
            double revenue = 0;

            rentals.ForEach(rental =>
            {
                if (rental.IsReturned) revenue += rental.calculateCost();
            });

            return revenue;
        }

        /// <summary>
        /// Calculate fuel economy based on total fuel consumption and total distance travelled
        /// </summary>
        /// <returns>fuel economy, litres per 100km</returns>
        public string calculateFuelEconomy()
        {
            double totalFuel = fuelPurchase.getFuel();
            double totalKm = 0;
            rentals.ForEach(rental =>
            {
                if (rental.IsReturned) totalKm += rental.getKilometers();
            });

            if (totalFuel == 0 || totalKm == 0) return "Unknown";

            double economy = totalFuel / totalKm * 100;
            return String.Format("{0:00}L / 100km", economy);
        }
    }
}