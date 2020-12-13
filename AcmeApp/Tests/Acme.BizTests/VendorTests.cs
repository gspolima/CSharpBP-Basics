using Microsoft.VisualStudio.TestTools.UnitTesting;
using Acme.Biz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.Common;

namespace Acme.Biz.Tests
{
    [TestClass()]
    public class VendorTests
    {
        [TestMethod()]
        public void SendWelcomeEmail_ValidCompany_Success()
        {
            // Arrange
            var vendor = new Vendor();
            vendor.CompanyName = "ABC Corp";
            var expected = "Message sent: Hello ABC Corp";

            // Act
            var actual = vendor.SendWelcomeEmail("Test Message");

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void SendWelcomeEmail_EmptyCompany_Success()
        {
            // Arrange
            var vendor = new Vendor();
            vendor.CompanyName = "";
            var expected = "Message sent: Hello";

            // Act
            var actual = vendor.SendWelcomeEmail("Test Message");

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void SendWelcomeEmail_NullCompany_Success()
        {
            // Arrange
            var vendor = new Vendor();
            vendor.CompanyName = null;
            var expected = "Message sent: Hello";

            // Act
            var actual = vendor.SendWelcomeEmail("Test Message");

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PlaceNewOrder()
        {
            var vendor = new Vendor();
            var product = new Product(12, "Bongos", "Latino instruments");

            var expected = true;
            var actual = vendor.PlaceOrder(product, 9);
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PlaceOrderWithNullProduct()
        {
            var vendor = new Vendor();

            var expected = false;
            var actual = vendor.PlaceOrder(null, 9);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PlaceOrderWithSubzeroQuantity()
        {
            var vendor = new Vendor();
            var product = new Product();

            var expected = false;
            var actual = vendor.PlaceOrder(product, -3);

            Assert.AreEqual(expected, actual);
        }
    }
}