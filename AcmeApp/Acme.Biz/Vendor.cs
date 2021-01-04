﻿using Acme.Common;
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

            var orderTextBuilder = ComposeOrderText(product.ProductCode, quantity);

            if (deliverBy.HasValue)
                orderTextBuilder = AppendDeliverDate(deliverBy.Value, orderTextBuilder);
            if (!String.IsNullOrWhiteSpace(instructions))
                orderTextBuilder = ApppendInstructions(instructions, orderTextBuilder);

            var orderText = orderTextBuilder.ToString();
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

        private StringBuilder ApppendInstructions(string instructions, StringBuilder orderText)
        {
            orderText.Append($"{Environment.NewLine}Instructions: {instructions}");
            return orderText;
        }

        private StringBuilder AppendDeliverDate(DateTimeOffset? deliverBy, StringBuilder orderText)
        {
            orderText.Append($"{Environment.NewLine}Deliver by: {deliverBy.Value.ToString("d")}");
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

        internal StringBuilder ComposeOrderText(string productCode, int quantity)
        {
            return new StringBuilder($"Order from Acme, Inc" +
                    $"{Environment.NewLine}Product: {productCode}" +
                    $"{Environment.NewLine}Quantity: {quantity}");
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

            if (!String.IsNullOrWhiteSpace(vendorInfo))
            {
                result = vendorInfo.ToLower();
                result = vendorInfo.ToUpper();
                result = vendorInfo.Replace("Vendor", "Supplier");

                var lenght = vendorInfo.Length;
                var index = vendorInfo.IndexOf(":");
                var begins = vendorInfo.StartsWith("Vendor");
            }
            
            return vendorInfo;
        }

        public string PrepareDirections()
        {
            var directions = @"Insert \r\n to define a new line in Windows systems";
            return directions;
        }

        public string PrepareDirectionsOnTwoLines()
        {
            var directions ="First do this " +
                            Environment.NewLine +
                            "then do that";
            return directions;
        }
    }
}
