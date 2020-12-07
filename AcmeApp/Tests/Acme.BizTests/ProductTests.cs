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

            var expected = "Hello Kindle (1): Waterproof e-reader";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AssignValuesWhenPassingParametersToConstructor()
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
    }
}
