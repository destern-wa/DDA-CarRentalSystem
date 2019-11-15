using System;

namespace VehicleRentalSystem
{
    /// <summary>
    /// Class for recording fuel purchases
    /// </summary>
    public class FuelPurchase
    {
        /// <summary>
        /// Total litres of fuel purchased
        /// </summary>
        private double litres = 0;

        /// <summary>
        /// Total cost of fuel purchased 
        /// </summary>
        private double cost = 0;

        /// <summary>
        /// Get total litres of fuel purchased in litres
        /// </summary>
        /// <returns></returns>
        public double getFuel()
        {
            return this.litres;
        }

        /// <summary>
        /// Add a fuel purchase
        /// </summary>
        /// <param name="amount">Litres of fuel purchases</param>
        /// <param name="price">Cost of purchase in dollars</param>
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