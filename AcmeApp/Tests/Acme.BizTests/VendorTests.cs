﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                                    $"{Environment.NewLine}Product: Tools-0001"+
                                    $"{Environment.NewLine}Quantity: 4"+
                                    $"{Environment.NewLine}Deliver by: 24/12/2021"+
                                    $"{Environment.NewLine}Instructions: standard delivery";
            var actual = vendor.PlaceOrder(product, 4, 
                                            new DateTimeOffset(
                                                2021, 12, 24, 0, 0, 0, 
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

            var actual = vendor.PlaceOrder(product, 4);
            var expectedMessage = "Order from Acme, Inc"+
                                    $"{Environment.NewLine}Product: Tools-0001" +
                                    $"{Environment.NewLine}Quantity: 4" +
                                    $"{Environment.NewLine}Instructions: standard delivery";

            Assert.IsTrue(actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }

        [TestMethod]
        public void PlaceOrder_DeliverBy_Instructions()
        {
            var vendor = new Vendor();
            var product = new Product();

            var actual = vendor.PlaceOrder(product, 4, new DateTimeOffset(
                                                       2021, 12, 24, 0, 0, 0, 
                                                        new TimeSpan(-4, 0, 0)), 
                                                        "Handle with care");
            var expectedMessage = $"Order from Acme, Inc" +
                                    $"{Environment.NewLine}Product: Tools-0001" +
                                    $"{Environment.NewLine}Quantity: 4" +
                                    $"{Environment.NewLine}Deliver by: 24/12/2021" +
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
                                                       2021, 12, 24, 0, 0, 0,
                                                        new TimeSpan(-4, 0, 0)),
                                                        null);
            var expectedMessage = $"Order from Acme, Inc" +
                                    $"{Environment.NewLine}Product: Tools-0001" +
                                    $"{Environment.NewLine}Quantity: 4" +
                                    $"{Environment.NewLine}Deliver by: 24/12/2021";

            Assert.IsTrue(actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }
        [TestMethod]
        public void PlaceOrder_IncludeAddress_SendCopy()
        {
            var vendor = new Vendor();
            var product = new Product();

            var actual = vendor.PlaceOrder(product, 4,
                                Vendor.IncludeAddress.Yes,
                                Vendor.SendCopy.Yes);
            var expectedOrderText = $"Test With Address With Copy";

            Assert.IsTrue(actual.Success);
            Assert.AreEqual(expectedOrderText, actual.Message);
        }

        [TestMethod]
        public void PlaceOrder_IncludeAddress()
        {
            var vendor = new Vendor();
            var product = new Product();

            var actual = vendor.PlaceOrder(product, 4,
                                Vendor.IncludeAddress.Yes,
                                Vendor.SendCopy.No);
            var expectedOrderText = $"Test With Address";

            Assert.IsTrue(actual.Success);
            Assert.AreEqual(expectedOrderText, actual.Message);
        }

        [TestMethod]
        public void PlaceOrder_SendCopy()
        {
            var vendor = new Vendor();
            var product = new Product();

            var actual = vendor.PlaceOrder(product, 4,
                                Vendor.IncludeAddress.No,
                                Vendor.SendCopy.Yes);
            var expectedOrderText = $"Test With Copy";

            Assert.IsTrue(actual.Success);
            Assert.AreEqual(expectedOrderText, actual.Message);
        }

        [TestMethod]
        public void PlaceOrder_NoAddress_NoCopy()
        {
            var vendor = new Vendor();
            var product = new Product();

            var actual = vendor.PlaceOrder(product, 4,
                                Vendor.IncludeAddress.No,
                                Vendor.SendCopy.No);
            var expectedOrderText = $"Test";

            Assert.IsTrue(actual.Success);
            Assert.AreEqual(expectedOrderText, actual.Message);
        }

        [TestMethod]
        public void PlaceOrder_NullDeliverBy_Instructions()
        {
            var vendor = new Vendor();
            var product = new Product();

            var actual = vendor.PlaceOrder(product, 7, instructions: "express delivery");

            var expectedOrderText = $"Order from Acme, Inc" +
                                    $"{Environment.NewLine}Product: Tools-0001" +
                                    $"{Environment.NewLine}Quantity: 7" +
                                    $"{Environment.NewLine}Instructions: express delivery";

            Assert.IsTrue(actual.Success);
            Assert.AreEqual(expectedOrderText, actual.Message);
        }

        [TestMethod]
        public void ToString_Override()
        {
            var vendor = new Vendor();

            vendor.CompanyName = "Apple Corps";
            var actual = vendor.ToString();

            var expectedOutput = $"Vendor: Apple Corps";
            var expectedLenght = 19;
            var expectedIndex = 6;

            Assert.AreEqual(expectedOutput, actual);
            Assert.AreEqual(expectedLenght, actual.Length);
            Assert.AreEqual(expectedIndex, actual.IndexOf(":"));
            Assert.IsTrue(actual.StartsWith("Vendor"));
        }

        [TestMethod]
        public void PrepareDirections_RightOutput()
        {
            var vendor = new Vendor();

            var expected = @"Insert \r\n to define a new line in Windows systems";
            var actual = vendor.PrepareDirections();
            Console.WriteLine(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PrepareDirections_OnTwoLines()
        {
            var vendor = new Vendor();

            var expected = "First do this " +
                            Environment.NewLine +
                            "then do that";
            var actual = vendor.PrepareDirectionsOnTwoLines();
            Console.WriteLine(actual);

            Assert.AreEqual(expected, actual);
        }
    }
}