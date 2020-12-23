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

        public enum IncludeAddress { Yes, No };

        public enum SendCopy { Yes, No };

        /// <summary>
        /// Places a new product order
        /// </summary>
        /// <param name="product">product to be ordered</param>
        /// <param name="quantity">quantity of the product</param>
        /// <param name="deliverBy">The estimated time arrival of the order</param>
        /// <param name="instructions">optional further instructions</param>
        /// <returns>success flag and order text</returns>
        public OperationResult PlaceOrder(Product product, int quantity, 
                                            DateTimeOffset? deliverBy = null, 
                                            string instructions = "standard delivery")
        {
            if (IsProductNull(product))
                throw new ArgumentNullException($"{nameof(product)} is null");

            if (!IsQuantityValid(quantity))
                throw new ArgumentOutOfRangeException($"{nameof(quantity)} is out of range");

            if (deliverBy <= DateTimeOffset.Now)
                throw new ArgumentOutOfRangeException($"{nameof(deliverBy)} is invalid");

            string orderText = ComposeOrderText(product.ProductCode, quantity);

            if (deliverBy.HasValue)
                orderText = AppendDeliverDate(deliverBy.Value, orderText);

            if (!String.IsNullOrWhiteSpace(instructions))
                orderText = ApppendInstructions(instructions, orderText);

            string confirmation = SendEmailConfirmation(orderText, product.Vendor.Email);
            var success = VerifyEmailSent(confirmation);

            var result = new OperationResult(success, orderText);
            return result;
        }

        public OperationResult PlaceOrder(Product product, int quantity, 
                                            IncludeAddress includeAddress, SendCopy sendCopy)
        {
            var ordertext = "Test";

            if (includeAddress == Vendor.IncludeAddress.Yes)
                ordertext += " With Address";
            if (sendCopy == Vendor.SendCopy.Yes)
                ordertext += " With Copy";

            var operationResult = new OperationResult(true, ordertext);
            return operationResult;
        }

        private string ApppendInstructions(string instructions, string orderText)
        {
            orderText += $"{Environment.NewLine}Instructions: {instructions}";
            return orderText;
        }

        private string AppendDeliverDate(DateTimeOffset? deliverBy, string orderText)
        {
            orderText += $"{Environment.NewLine}Deliver by: {deliverBy.Value.ToString("d")}";
            return orderText;
        }

        internal string SendEmailConfirmation(string orderText, string emailAddress)
        {
            var email = new EmailService();
            var confirmation = email.SendMessage("New Order", orderText, emailAddress);
            return confirmation;
        }

        internal bool VerifyEmailSent(string confirmation)
        {
            if (confirmation.StartsWith("Message sent: "))
                return true;
            else
                return false;
        }

        internal string ComposeOrderText(string productCode, int quantity)
        {
            return $"Order from Acme, Inc" +
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

        public override string ToString()
        {
            string vendorInfo = $"Vendor: {this.CompanyName}";

            string result;
            result = vendorInfo.ToLower();
            result = vendorInfo.ToUpper();
            result = vendorInfo.Replace("Vendor", "Supplier");

            var lenght = vendorInfo.Length;
            var index = vendorInfo.IndexOf(":");
            var begins = vendorInfo.StartsWith("Vendor");
            
            return result;
        }
    }
}
