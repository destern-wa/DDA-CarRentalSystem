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
    }
}
