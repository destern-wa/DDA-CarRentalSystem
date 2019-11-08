using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalSystem.Model
{
    class Rental
    {
        private DateTime rentedDate;
        private DateTime expectedReturnDate;
        private DateTime returnedDate;
        private List<string> rentalTypes = new List<string> { "byDay", "byKilometer" };
        private string rentalType;

        public Rental(DateTime from, DateTime to, string type)
        {
            this.rentedDate = from;
            this.expectedReturnDate = to;
            this.rentalType = type;
        }

        public string status() {
            if (returnedDate == null)
            {
                return "Rented";
            }
            else
            {
                return "available"
            }
        }
    }
}
