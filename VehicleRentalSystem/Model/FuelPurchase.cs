using System;

namespace VehicleRentalSystem
{
    public class FuelPurchase
    {
        private double litres = 0;
        private double cost = 0;

        public double getFuel()
        {
            return this.litres;
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