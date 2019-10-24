using System;

namespace VehicleRentalSystem
{
    public class Journey
    {
        private double kilometers;

        /// <summary>
        /// Class constructor
        /// </summary>
        public Journey()
        {
            this.kilometers = 0;
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
