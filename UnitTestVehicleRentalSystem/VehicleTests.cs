using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VehicleRentalSystem;
namespace UnitTestVehicleRentalSystem
{
    /// <summary>
    /// Summary description for VehicleTests
    /// </summary>
    [TestClass]
    public class VehicleTests
    {
       
        [TestMethod]
        public void TestNormalConstructor()
        {
            bool gaveError = false;
            try
            {
                Vehicle v = new Vehicle("man", "mod", 2000, "1REG088", 1200, 70.5);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                gaveError = true;
            }
            finally
            {
                Assert.IsFalse(gaveError);
            }
        }

        [TestMethod]
        public void TestEmptyManufacturerConstructor()
        {
            bool gaveError = false;
            try
            {
                Vehicle v = new Vehicle("", "mod", 2000, "1REG088", 1200, 70.5);
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
        public void TestEmptyModelConstructor()
        {
            bool gaveError = false;
            try
            {
                Vehicle v = new Vehicle("man", "", 2000, "1REG088", 1200, 70.5);
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
        public void TestBadMakeYearConstructor()
        {
            bool gaveError = false;
            try
            {
                Vehicle v = new Vehicle("man", "mod", 1776, "1REG088", 1200, 70.5);
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
        public void TestFutureMakeYearConstructor()
        {
            bool gaveError = false;
            int currentYear = DateTime.Today.Year;
            try
            {
                Vehicle v = new Vehicle("man", "mod", currentYear+1, "1REG088", 1200, 70.5);
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
        public void TestEmptyRegistrationConstructor()
        {
            bool gaveError = false;
            try
            {
                Vehicle v = new Vehicle("man", "", 2007, "", 1200, 70.5);
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
        public void TestBadOdometerConstructor()
        {
            bool gaveError = false;
            try
            {
                Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", -5, 70.5);
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
        public void TestBadTankCapacityConstructor()
        {
            bool gaveError = false;
            try
            {
                Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", -5, 70.5);
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
        public void TestNoTankCapacityConstructor()
        {
            bool gaveError = false;
            try
            {
                Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", 7075);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                gaveError = true;
            }
            finally
            {
                Assert.IsFalse(gaveError);
            }
        }

        [TestMethod]
        public void TestAddKilomters()
        {
            int startKm = 100;
            int incrementKm = 5;
            Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", startKm);

            v.addKilometers(incrementKm);

            int expectedKm = startKm + incrementKm;
            int actualKm = v.Odometer;
            Assert.AreEqual(expectedKm, actualKm);
        }

        [TestMethod]
        public void TestAddNegativeKilomters()
        {
            bool gaveError = false;
            try
            {
                Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", 1000);
                v.addKilometers(-5);
            } catch
            {
                gaveError = true;
            }
            finally
            {
                Assert.IsTrue(gaveError);
            }
        }

        [TestMethod]
        public void TestNeedsFuelTrue()
        {
            double tankCapacity = 60;
            double addFuelAmount = 30;
            Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", 1000, tankCapacity);

            v.addFuel(addFuelAmount, 1.30);
            bool needsFuel = v.needsFuel();

            Assert.IsTrue(needsFuel);
        }

        [TestMethod]
        public void TestNeedsFuelFalse()
        {
            double tankCapacity = 60;
            double addFuelAmount = 58;
            Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", 1000, tankCapacity);

            v.addFuel(addFuelAmount, 1.30);
            bool needsFuel = v.needsFuel();

            Assert.IsFalse(needsFuel);
        }

        [TestMethod]
        public void TestStatusUnrented()
        {
            Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", 100, 60);
            Assert.AreEqual("Available", v.Status);

        }

        [TestMethod]
        public void TestStatusRented()
        {
            Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", 100, 60);
            Rental r = new Rental(DateTime.Now, DateTime.Now, true);
            v.AddRental(r);
            Assert.AreEqual("Rented", v.Status);

        }

        [TestMethod]
        public void TestStatusRentedOverdue()
        {
            Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", 100, 60);
            Rental r = new Rental(new DateTime(2019,10,1), new DateTime(2019, 10, 1), true);
            v.AddRental(r);
            Assert.AreEqual("Rented (overdue)", v.Status);

        }

        [TestMethod]
        public void TestStatusReturned()
        {
            Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", 100, 60);
            Rental r = new Rental(DateTime.Now, DateTime.Now, true);
            v.AddRental(r);
            v.ReturnRental(DateTime.Now, 100, 40, 70);
            Assert.AreEqual("Available", v.Status);
        }

        [TestMethod]
        public void TestStatusNeedsService()
        {
            Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", 11300, 60);
            Assert.AreEqual("Needs service", v.Status);
        }

        [TestMethod]
        public void TestStatusAfterService()
        {
            Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", 11300, 60);
            Assert.AreEqual("Needs service", v.Status);
            v.recordService();
            Assert.AreEqual("Available", v.Status);
        }

        [TestMethod]
        public void TestCalculateRevenueNone()
        {
            Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", 11300, 60);
            Assert.AreEqual(0, v.calculateRevenue());
        }

        [TestMethod]
        public void TestCalculateRevenueRentedNotReturned()
        {
            Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", 11300, 60);
            Rental r = new Rental(DateTime.Now, DateTime.Now, true);
            v.AddRental(r);
            Assert.AreEqual(0, v.calculateRevenue());
        }

        [TestMethod]
        public void TestCalculateRevenueOneRentalReturned()
        {
            Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", 11300, 60);
            Rental r = new Rental(DateTime.Now, DateTime.Now, true);
            v.AddRental(r);
            v.ReturnRental(DateTime.Now, 100, 40, 70);

            double expectedRevenue = r.calculateCost();
            double actualRevenue = v.calculateRevenue();

            Assert.AreEqual(expectedRevenue, actualRevenue);
        }

        [TestMethod]
        public void TestCalculateRevenueMultipleRentalsReturned()
        {
            Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", 11300, 60);
            Rental r1 = new Rental(DateTime.Now, DateTime.Now, true);
            v.AddRental(r1);
            v.ReturnRental(DateTime.Now, 100, 40, 70);
            Rental r2 = new Rental(DateTime.Now, DateTime.Now, false);
            v.AddRental(r2);
            v.ReturnRental(DateTime.Now, 100, 40, 70);

            double expectedRevenue = r1.calculateCost() + r2.calculateCost();
            double actualRevenue = v.calculateRevenue();

            Assert.AreEqual(expectedRevenue, actualRevenue);
        }

        [TestMethod]
        public void TestCalculateFuelEconomyUnknown()
        {
            Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", 11300, 60);
            Assert.AreEqual("Unknown", v.calculateFuelEconomy());
        }

        [TestMethod]
        public void TestCalculateFuelEconomySingleRental()
        {
            double km = 200;
            double litres = 20;
            Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", 11300, 60);
            Rental r1 = new Rental(DateTime.Now, DateTime.Now, true);
            v.AddRental(r1);
            v.ReturnRental(DateTime.Now, km, litres, 70);

            string expectedEconomy = "10L / 100km";
            string actualEconomy = v.calculateFuelEconomy();

            Assert.AreEqual(expectedEconomy, v.calculateFuelEconomy());
        }

        [TestMethod]
        public void TestCalculateFuelEconomyMutlipleRentals()
        {
            double km1 = 200;
            double litres1 = 10;
            double km2 = 100;
            double litres2 = 20;
            Vehicle v = new Vehicle("man", "mod", 2007, "1REG088", 11300, 60);
            Rental r1 = new Rental(DateTime.Now, DateTime.Now, true);
            v.AddRental(r1);
            v.ReturnRental(DateTime.Now, km1, litres1, 70);
            Rental r2 = new Rental(DateTime.Now, DateTime.Now, true);
            v.AddRental(r2);
            v.ReturnRental(DateTime.Now, km2, litres2, 70);

            string expectedEconomy = "10L / 100km";
            string actualEconomy = v.calculateFuelEconomy();

            Assert.AreEqual(expectedEconomy, v.calculateFuelEconomy());
        }
    }
}
