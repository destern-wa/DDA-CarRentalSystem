using System;

namespace VehicleRentalSystem
{
    /// <summary>
    /// Class for recording rentals (aka journeys)
    /// </summary>
    public class Rental
    {
        /// <summary>
        /// Date vehicle rented out
        /// </summary>
        private DateTime rentedDate;
        /// <summary>
        /// Date vehicle is expected back
        /// </summary>
        private DateTime expectedReturnDate;
        /// <summary>
        /// Date vehicle is actually returned
        /// </summary>
        private DateTime returnedDate;
        /// <summary>
        /// Is vehicle returned
        /// </summary>
        private bool isRetuned;
        /// <summary>
        /// Is vehicle rented per day. False means it is rented per kilometer
        /// </summary>
        private bool isPerDayRental;
        /// <summary>
        /// Rental cost, dollars per day
        /// </summary>
        private readonly double COST_PER_DAY = 100;
        /// <summary>
        /// Rental cost, dollars per kilometer
        /// </summary>
        private readonly double COST_PER_KM = 1;
        /// <summary>
        /// Kilometres travelled on the journey
        /// </summary>
        private double kilometers;

        /// <summary>
        /// Converts a DateTime object into an equvilent DateTime object with the time component set to midnight.
        /// This allows easier comparasion and duration calculations.
        /// </summary>
        /// <param name="d">Original DateTime object</param>
        /// <returns></returns>
        private DateTime DateWithoutTime(DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day);
        }

        /// <summary>
        /// Constructor for a rental.
        /// </summary>
        /// <param name="from">Date to rent from</param>
        /// <param name="to">Expected date of return</param>
        /// <param name="isPerDayRental">Is rented per day; false means rented per kilometer</param>
        public Rental(DateTime from, DateTime to, bool isPerDayRental)
        {
            rentedDate = DateWithoutTime(from);
            expectedReturnDate = DateWithoutTime(to);
            if (expectedReturnDate < rentedDate)
            {
                throw new Exception("Expected return date must not be prior to the rental start date");
            }
            this.isPerDayRental = isPerDayRental;
            this.kilometers = 0;
        }

        /// <summary>
        /// Gets the current rental status
        /// </summary>
        /// <returns>Rental status</returns>
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

        /// <summary>
        /// Mark rental as returned
        /// </summary>
        /// <param name="returnDate">Date vehicle returned</param>
        /// <param name="kmTravelled">Kilomters travelled</param>
        public void returnVehicle(DateTime returnDate, double kmTravelled)
        {
            this.returnedDate = DateWithoutTime(returnDate);
            if (returnedDate < rentedDate)
            {
                throw new Exception("Return date must not be prior to the rental start date");
            }
            addKilometers(kmTravelled);
            isRetuned = true;
        }

        /// <summary>
        /// Calculate cost of the rental
        /// </summary>
        /// <returns>Cost in dollars</returns>
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
        /// Appends the distance to the <see cref="kilometers"/>
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

        /// <summary>
        /// Getter method for if the vehicle has been returned
        /// </summary>
        public bool IsReturned
        {
            get => this.isRetuned;
        }
    }
}
