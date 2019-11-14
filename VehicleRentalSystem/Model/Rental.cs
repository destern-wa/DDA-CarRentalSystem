using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalSystem
{
    public class Rental // aka Journey
    {
        private DateTime rentedDate;
        private DateTime expectedReturnDate;
        private DateTime returnedDate;
        private bool isRetuned;
        private bool isPerDayRental; // false means is rented per km
        private readonly double COST_PER_DAY = 100; // dollars
        private readonly double COST_PER_KM = 1; // dollars
        private double kilometers;

        private DateTime DateWithoutTime(DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day);
        }

        public Rental(DateTime from, DateTime to, bool isPerDayRental)
        {
            rentedDate = DateWithoutTime(from);
            expectedReturnDate = DateWithoutTime(to);
            this.isPerDayRental = isPerDayRental;
            this.kilometers = 0;
        }

        public string status() {
            if (isRetuned)
            {
                return "Returned";
            } else
            {
                string status = "Rented";
                if ((DateWithoutTime(DateTime.Now) - expectedReturnDate).Days > 0)
                {
                    status += " (overdue)";
                }
                return status;
            }
        }

        public void returnVehicle(DateTime returnDate, double kmTravelled)
        {
            returnedDate = DateWithoutTime(DateTime.Now);
            isRetuned = true;
            addKilometers(kmTravelled);
        }

        public double calculateCost()
        {
            if (isPerDayRental)
            {
                int days = (returnedDate - rentedDate).Days;
                if (days == 0) days = 1; // Min period is 1 day
                return days * COST_PER_DAY;
            } else
            {
                return kilometers * COST_PER_KM;
            }
        }

        /// <summary>
        /// Appends the distance parameter to <see cref="kilometers"/>
        /// </summary>
        /// <param name="kilometers"></param>
        public void addKilometers(double kilometers)
        {
            if (kilometers <= 0)
            {
                throw new Exception("Distance to add must be greater than 0");
            }
            this.kilometers += kilometers;
        }

        /// <summary>
        /// Getter method for total Kilometers traveled in this journey.
        /// </summary>
        /// <returns><see cref="kilometers"/></returns>
        public double getKilometers()
        {
            return kilometers;
        }
    }
}
