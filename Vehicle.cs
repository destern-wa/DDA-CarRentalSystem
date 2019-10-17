using System;

namespace ConsoleApp_Assignment2
{
    public class Vehicle
    {

        private string manufacturer;
        private string model;
        private int makeYear;
        // TODO add Registration Number 
        // TODO add variable for OdometerReading (in KM), 
        // TODO add variable for TankCapacity (in litres)

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
            this.manufacturer = manufacturer;
            this.model = model;
            this.makeYear = makeYear;
            fuelPurchase = new FuelPurchase();
        }

        // TODO Add missing getter and setter methods

        /// <summary>
        /// Prints details for <see cref="Vehicle"/>
        /// </summary>
        public void printDetails()
        {
            Console.WriteLine("Vehicle: " + makeYear + " " + manufacturer + " " + model);
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