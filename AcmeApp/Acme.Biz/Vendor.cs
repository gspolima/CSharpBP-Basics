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
            var success = false;
            if (!IsProductNull(product) && IsQuantityValid(quantity))
            {
                string orderText = ComposeOrderText(product.ProductCode, quantity);
                string confirmation = ComposeEmailConfirmation(orderText, product.Vendor.Email);
                success = verifyEmailSent(confirmation);
            }
            return success;
        }

        internal string ComposeEmailConfirmation(string orderText, string emailAddress)
        {
            var email = new EmailService();
            var confirmation = email.SendMessage("New Order", orderText, emailAddress);
            return confirmation;
        }

        internal bool verifyEmailSent(string confirmation)
        {
            if (confirmation.StartsWith("Message sent: "))
                return true;
            else
                return false;
        }

        internal string ComposeOrderText(string productCode, int quantity)
        {
            return $"Order from Acme, Inc " +
                    $"{Environment.NewLine}Product: {productCode}" +
                    $"{Environment.NewLine}Quantity: {quantity}";
        }

        internal bool IsQuantityValid(int quantity)
        {
            if (quantity <= 0)
                return false;
            else
                return true;
        }

        internal bool IsProductNull(Product product)
        {
            if (product == null)
                return true;
            else
                return false;
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
