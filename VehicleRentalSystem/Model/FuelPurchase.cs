using System;

namespace VehicleRentalSystem
{
    public class FuelPurchase
    {
        private double fuelEconomy;
        private double litres = 0;
        private double cost = 0;

        public double getFuelEconomy()
        {
            return fuelEconomy;
            //return this.cost / this.litres;
        }

        public double getFuel()
        {
            return this.litres;
        }

        public void setFuelEconomy(double fuelEconomy)
        {
            if (fuelEconomy <= 0)
            {
                throw new Exception("Fuel economy must be greater than 0");
            }
            this.fuelEconomy = fuelEconomy;
        }
        public void purchaseFuel(double amount, double price)
        {
            if (amount <= 0)
            {
                throw new Exception("Amount must be greater than 0");
            }
            if (price < 0)
            {
                throw new Exception("Price can not be negative");
            }
            this.litres += amount;
            this.cost += price;
        }
    }
}