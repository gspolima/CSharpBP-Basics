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

            var actual = vendor.PlaceOrder(product, 9);
            
            Assert.IsTrue(actual.Success);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PlaceOrder_NullProduct()
        {
            var vendor = new Vendor();

            vendor.PlaceOrder(null, 9);
            // Assert exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PlaceOrder_SubzeroQuantity()
        {
            var vendor = new Vendor();
            var product = new Product();

            vendor.PlaceOrder(product, -3);
            // Assert exception
        }

        [TestMethod]
        public void PlaceOrder_DeliverBy_JustRight()
        {
            var vendor = new Vendor();
            var product = new Product();

            var expectedMessage = "Order from Acme, Inc"+
                                    $"{Environment.NewLine}Product: Tools-1"+
                                    $"{Environment.NewLine}Quantity: 4"+
                                    $"{Environment.NewLine}Deliver by: 24/12/2020";
            var actual = vendor.PlaceOrder(product, 4, 
                                            new DateTimeOffset(
                                                2020, 12, 24, 0, 0, 0, 
                                                new TimeSpan(-4, 0, 0)));
            Assert.AreEqual(expectedMessage, actual.Message);
            Assert.IsTrue(actual.Success);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PlaceOrder_DeliverBy_PastDate()
        {
            var vendor = new Vendor();
            var product = new Product();

            var actual = vendor.PlaceOrder(product, 4,
                                            new DateTimeOffset(
                                                2020, 12, 10, 0, 0, 0,
                                                new TimeSpan(-4, 0, 0)));
            // Assert exception
        }

        [TestMethod]
        public void PlaceOrder_DeliverBy_NullDate()
        {
            var vendor = new Vendor();
            var product = new Product();

            var actual = vendor.PlaceOrder(product, 4, null);
            var expectedMessage = "Order from Acme, Inc"+
                "\r\nProduct: Tools-1\r\nQuantity: 4";
            
            Assert.IsTrue(actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }

        [TestMethod]
        public void PlaceOrder_DeliverBy_Instructions()
        {
            var vendor = new Vendor();
            var product = new Product();

            var actual = vendor.PlaceOrder(product, 4, new DateTimeOffset(
                                                       2020, 12, 24, 0, 0, 0, 
                                                        new TimeSpan(-4, 0, 0)), 
                                                        "Handle with care");
            var expectedMessage = $"Order from Acme, Inc" +
                                    $"{Environment.NewLine}Product: Tools-1" +
                                    $"{Environment.NewLine}Quantity: 4" +
                                    $"{Environment.NewLine}Deliver by: 24/12/2020" +
                                    $"{Environment.NewLine}Instructions: Handle with care";

            Assert.IsTrue(actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }

        [TestMethod]
        public void PlaceOrder_DeliverBy_NullInstructions()
        {
            var vendor = new Vendor();
            var product = new Product();

            var actual = vendor.PlaceOrder(product, 4, new DateTimeOffset(
                                                       2020, 12, 24, 0, 0, 0,
                                                        new TimeSpan(-4, 0, 0)),
                                                        null);
            var expectedMessage = $"Order from Acme, Inc" +
                                    $"{Environment.NewLine}Product: Tools-1" +
                                    $"{Environment.NewLine}Quantity: 4" +
                                    $"{Environment.NewLine}Deliver by: 24/12/2020";

            Assert.IsTrue(actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }
    }
}