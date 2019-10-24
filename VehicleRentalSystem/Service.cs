using System;

namespace VehicleRentalSystem
{
    public class Service
    {
        // Constant to indicate that the vehicle needs to be serviced every 10,000km
        public static readonly int SERVICE_KILOMETER_LIMIT = 10000;

        private int lastServiceOdometerKm = 0;
        private int serviceCount = 0;
        // TODO add lastServiceDate

        // return the last service
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
        }

        // return how many services the car has had
        public int getServiceCount()
        {
            return this.serviceCount;
        }
 
        /// <summary>
        /// Calculates the total services by dividing kilometers by
        /// <see cref="SERVICE_KILOMETER_LIMIT"/> and floors the value.
        /// </summary>
        /// <returns></returns>
        public int getTotalScheduledServices()
        {
            return (int)Math.Floor( (double)(lastServiceOdometerKm / SERVICE_KILOMETER_LIMIT) );
        }
    }
}
