using System;

namespace VehicleRentalSystem
{
    /// <summary>
    /// Class for recording vehicles services
    /// </summary>
    public class Service
    {
        /// <summary>
        /// Constant to indicate that the vehicle needs to be serviced every 10,000km
        /// </summary>
        public static readonly int SERVICE_KILOMETER_LIMIT = 10000;
        /// <summary>
        /// Odometer reading when the last service occured, in kilometres
        /// </summary>
        private int lastServiceOdometerKm = 0;
        /// <summary>
        /// Number of services performed
        /// </summary>
        private int serviceCount = 0;
        /// <summary>
        /// Date of last service
        /// </summary>
        private DateTime lastServiceDate;

        // return the last service
        /// <summary>
        /// Getter method for the odometer reading when the last service occured
        /// </summary>
        /// <returns>odometer reading at last service, in kilometres</returns>
        public int getLastServiceOdometerKm()
        {
            return this.lastServiceOdometerKm;
        }

        /// <summary>
        /// The function recordService expects the total distance traveled by the car,
        /// saves it and increase serviceCount.
        /// </summary>
        /// <param name="distance"></param>
        public void recordService(int distance)
        {
            this.lastServiceOdometerKm = distance;
            this.serviceCount++;
            lastServiceDate = DateTime.Now;
        }

        /// <summary>
        /// Getter method for the number of services the vehicle has had
        /// </summary>
        /// <returns>number of serivces</returns>
        public int getServiceCount()
        {
            return this.serviceCount;
        }

        /// <summary>
        /// Calculates the total services by dividing kilometers by
        /// <see cref="SERVICE_KILOMETER_LIMIT"/> and floors the value.
        /// </summary>
        /// <returns>total number of scheduled services</returns>
        public int getTotalScheduledServices()
        {
            return (int)Math.Floor( (double)lastServiceOdometerKm / (double)SERVICE_KILOMETER_LIMIT );
        }

    }
}
