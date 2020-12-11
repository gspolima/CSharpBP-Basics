using Acme.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Biz
{
    public class Vendor 
    {
        public int VendorId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }

        public bool PlaceOrder(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException($"{nameof(product)} cannot be null");
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(quantity)} must be positive");

            var success = false;

            var orderText = $"Order from Acme, Inc " +
                            $"{Environment.NewLine}Product: {product.ProductCode}" +
                            $"{Environment.NewLine}Quantity: {quantity}";

            var email = new EmailService();
            var confirmation = email.SendMessage("New Order", orderText, product.Vendor.Email);

            if (confirmation.Contains("sent"))
                success = true;

            return success;
        }

        public string SendWelcomeEmail(string message)
        {
            var emailService = new EmailService();
            var subject = ("Hello " + this.CompanyName).Trim();
            var confirmation = emailService.SendMessage(subject,
                                                        message, 
                                                        this.Email);
            return confirmation;
        }
    }
}
