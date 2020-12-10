using Microsoft.VisualStudio.TestTools.UnitTesting;
using Acme.Biz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.BizTests
{
    [TestClass]
    public class ProductTests
    {
        [TestMethod]
        public void DisplaySayHelloMessage()
        {
            var product = new Product();
            product.Id = 1;
            product.Name = "Kindle";
            product.Description = "Waterproof e-reader";
            product.Vendor.CompanyName = "Amazon";
            var actual = product.SayHello();

            var expected = "Hello Kindle (1): Waterproof e-reader. Available on: ";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SetValuesFromConstructorParameters()
        {
            var product = new Product(1, "Office Chair", "Adjustable executive chair");

            Assert.AreEqual(product.Id, 1);
            Assert.AreEqual(product.Name, "Office Chair");
            Assert.AreEqual(product.Description, "Adjustable executive chair");
        }

        [TestMethod]
        public void AssignNullWhenAnyObjectIsNull()
        {
            Product product = null;

            var vendorEmail = product?.Vendor?.Email;
            var actual = vendorEmail;
            Product expected = null;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void InchesPerMeterValueIsRight()
        {
            var expected = 78.74;
            var actual = 2 * Product.InchesPerMeter;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SetDefaultMinimumPrice()
        {
            var product = new Product();

            var expected = .24m;
            var actual = product.MinimumPrice;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SetMinimumPriceIfBulk()
        {
            var product = new Product(1, "Bulk of books", "A library");

            var expected = .69m;
            var actual = product.MinimumPrice;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NameIsTooShort()
        {
            var product = new Product();

            product.Name = "M1";

            var expected = "The name must be at least 3 characters long";
            var actual = product.ValidationMessage;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NameIsTooLong()
        {
            var product = new Product();

            product.Name = "JetBrains Resharper for Visual Studio 2019";

            var expected = "The name cannot be more than 20 characters long";
            var actual = product.ValidationMessage;
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void NameIsJustRight()
        {
            var product = new Product();

            product.Name = "Apple Watch";

            string expected = null;
            var actual = product.ValidationMessage;
            Assert.AreEqual("Apple Watch", product.Name);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SetDefaultCategory()
        {
            var product = new Product();

            var expected = "Tools";
            var actual = product.Category;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SetNewCategory()
        {
            var product = new Product();

            product.Category = "Tech";

            var expected = "Tech";
            var actual = product.Category;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SetDefaultSequenceNumber()
        {
            var product = new Product();            

            var expected = 1;
            var actual = product.SequenceNumber;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SetNewSequenceNumber()
        {
            var product = new Product();
            product.SequenceNumber = 20023091;

            var expected = 20023091;
            var actual = product.SequenceNumber;
            Assert.AreEqual(expected, actual);
        }
    }
}
