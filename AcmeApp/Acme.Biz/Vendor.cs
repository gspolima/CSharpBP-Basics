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

        public OperationResult PlaceOrder(Product product, int quantity)
        {            
            if (IsProductNull(product)) 
                throw new ArgumentNullException($"{nameof(product)} is null");
            if (!IsQuantityValid(quantity))
                throw new ArgumentOutOfRangeException($"{nameof(quantity)} is out of range");

            string orderText = ComposeOrderText(product.ProductCode, quantity);
            string confirmation = SendEmailConfirmation(orderText, product.Vendor.Email);
            var success = verifyEmailSent(confirmation);

            var result = new OperationResult(success, confirmation); 
            return result;
        }

        internal string SendEmailConfirmation(string orderText, string emailAddress)
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
