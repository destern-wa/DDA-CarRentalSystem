using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VehicleRentalSystem;

namespace UnitTestVehicleRentalSystem
{
    [TestClass]
    public class FuelPurchaseTests
    {
        FuelPurchase fp = new FuelPurchase();

        [TestMethod]
        public void TestConstructor()
        {
            Assert.IsNotNull(fp);
        }

        [TestMethod]
        public void TestPurchaseFuel()
        {
            fp.purchaseFuel(1.01, 2.02);
            double expectedFuel = 1.01;
            double actualFuel = fp.getFuel();
            Assert.AreEqual(expectedFuel, actualFuel);
        }

        [TestMethod]
        public void TestPurchaseFuelTwice()
        {
            fp.purchaseFuel(1.01, 2.02);
            fp.purchaseFuel(5.55, 2.56);
            double expectedFuel = 1.01+5.55;
            double actualFuel = fp.getFuel();
            Assert.AreEqual(expectedFuel, actualFuel);
        }

        [TestMethod]
        public void TestPurchaseZeroFuel()
        {
            bool gaveError = false;
            try
            {
                fp.purchaseFuel(0, 2.56);
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
        public void TestPurchaseNegativeFuel()
        {
            bool gaveError = false;
            try
            {
                fp.purchaseFuel(-6, 2.56);
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
        public void TestPurchaseNegativePrice()
        {
            bool gaveError = false;
            try
            {
                fp.purchaseFuel(6, -2.56);
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
