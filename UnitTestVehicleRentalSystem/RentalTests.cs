using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VehicleRentalSystem;

namespace UnitTestVehicleRentalSystem
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class RentalTests
    {
        Rental j = new Rental(DateTime.Now, DateTime.Now, true);
        readonly double costPerDay = 100;
        readonly double costPerKm = 1;

        [TestMethod]
        public void TestConstructor()
        {
            Assert.IsNotNull(j);
        }

        [TestMethod]
        public void TestConstructorBackwardsDates()
        {
            bool gaveError = false;
            try
            {
                Rental r = new Rental(new DateTime(2019, 11, 16), new DateTime(2019, 11, 14), true);
            }
            catch (Exception ex)
            {
                gaveError = true;
            }
            finally
            {
                Assert.IsTrue(gaveError);
            }
        }

        [TestMethod]
        public void TestReturnVehicle()
        {
            double km = 123;
            Rental r = new Rental(DateTime.Now, DateTime.Now, true);
            r.returnVehicle(DateTime.Now, km);
            Assert.IsTrue(r.IsReturned);
            Assert.AreEqual(km, r.getKilometers());
        }

        [TestMethod]
        public void TestReturnVehicleBadDate()
        {
            bool gaveError = false;
            Rental r = new Rental(new DateTime(2019, 11, 10), new DateTime(2019, 11, 12), true);
            try
            {
                r.returnVehicle(new DateTime(2019, 11, 5), 88);
            }
            catch (Exception ex)
            {
                gaveError = true;
            }
            finally
            {
                Assert.IsTrue(gaveError);
            }
        }

        [TestMethod]
        public void TestReturnVehicleNegenativeKm()
        {
            bool gaveError = false;
            Rental r = new Rental(new DateTime(2019, 11, 10), new DateTime(2019, 11, 12), true);
            try
            {
                r.returnVehicle(new DateTime(2019, 11, 12), -35);
            }
            catch (Exception ex)
            {
                gaveError = true;
            }
            finally
            {
                Assert.IsTrue(gaveError);
            }
        }

        [TestMethod]
        public void TestReturnVehicleZeroKm()
        {
            bool gaveError = false;
            Rental r = new Rental(new DateTime(2019, 11, 10), new DateTime(2019, 11, 12), true);
            try
            {
                r.returnVehicle(new DateTime(2019, 11, 12), 0);
            }
            catch (Exception ex)
            {
                gaveError = true;
            }
            finally
            {
                Assert.IsTrue(gaveError);
            }
        }

        [TestMethod]
        public void TestGetKilometers()
        {
            double expectedKm = 0;
            double actualKm = j.getKilometers();
            Assert.AreEqual(expectedKm, actualKm);
        }

        [TestMethod]
        public void TestStatusRented()
        {
            Rental r = new Rental(DateTime.Now, DateTime.Now, true);
            string expectdStatus = "Rented";
            string actualStatus = r.status();
            Assert.AreEqual(expectdStatus, actualStatus);
        }

        [TestMethod]
        public void TestStatusRentedOverdue()
        {
            Rental r = new Rental(new DateTime(2000, 1, 1), new DateTime(2000, 1, 2), true);
            string expectdStatus = "Rented (overdue)";
            string actualStatus = r.status();
            Assert.AreEqual(expectdStatus, actualStatus);
        }

        [TestMethod]
        public void TestStatusReturned()
        {
            Rental r = new Rental(DateTime.Now, DateTime.Now, true);
            r.returnVehicle(DateTime.Now, 5);
            string expectdStatus = "Returned";
            string actualStatus = r.status();
            Assert.AreEqual(expectdStatus, actualStatus);
        }

        [TestMethod]
        public void TestCalculateCostSameDay()
        {
            Rental r = new Rental(DateTime.Now, DateTime.Now, true);
            r.returnVehicle(DateTime.Now, 5);
            double expectdCost = costPerDay * 1;
            double actualCost = r.calculateCost();
            Assert.AreEqual(expectdCost, actualCost);
        }

        [TestMethod]
        public void TestCalculateCostNextDay()
        {
            Rental r = new Rental(new DateTime(2019, 11, 10), new DateTime(2019, 11, 11), true);
            r.returnVehicle(new DateTime(2019, 11, 11), 5);
            double expectdCost = costPerDay * 1;
            double actualCost = r.calculateCost();
            Assert.AreEqual(expectdCost, actualCost);
        }

        [TestMethod]
        public void TestCalculateCostTwoDays()
        {
            Rental r = new Rental(new DateTime(2019, 11, 10), new DateTime(2019, 11, 12), true);
            r.returnVehicle(new DateTime(2019, 11, 12), 5);
            double expectdCost = costPerDay * 2;
            double actualCost = r.calculateCost();
            Assert.AreEqual(expectdCost, actualCost);
        }

        [TestMethod]
        public void TestCalculateCostPerKm()
        {
            double km = 57.2;
            Rental r = new Rental(DateTime.Now, DateTime.Now, false);
            r.returnVehicle(DateTime.Now, km);
            double expectdCost = costPerKm * km;
            double actualCost = r.calculateCost();
            Assert.AreEqual(expectdCost, actualCost);
        }

        [TestMethod]
        public void TestAddKilometers()
        {
            j.addKilometers(1.23);
            double expectedKm = 1.23;
            double actualKm = j.getKilometers();
            Assert.AreEqual(expectedKm, actualKm);
        }

        [TestMethod]
        public void TestAddKilometersTwice()
        {
            j.addKilometers(1.23);
            j.addKilometers(4.56);
            double expectedKm = 1.23+4.56;
            double actualKm = j.getKilometers();
            Assert.AreEqual(expectedKm, actualKm);
        }

        [TestMethod]
        public void TestAddZeroKilometers()
        {
            bool gaveError = false;
            try
            {
                j.addKilometers(0);
            }
            catch
            {
                gaveError = true;
            }
            finally
            {
                Assert.IsTrue(gaveError);
            }
        }

        [TestMethod]
        public void TestAddNegativeKilometers()
        {
            bool gaveError = false;
            try
            {
                j.addKilometers(-697);
            }
            catch
            {
                gaveError = true;
            }
            finally
            {
                Assert.IsTrue(gaveError);
            }
        }
    }
}
