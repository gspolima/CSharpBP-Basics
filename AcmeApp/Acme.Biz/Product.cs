﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.Common;
using static Acme.Common.LoggingService;

namespace Acme.Biz
{
    public class Product
    {
        public readonly decimal MinimumPrice;
        
        public const double InchesPerMeter = 39.37;

        public Product()
        {
            Console.WriteLine("Product instance created");
            this.MinimumPrice = .24m;
        }
        
        public Product(int id, string name, string description) : this()
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            if (this.Name.StartsWith("Bulk"))
                this.MinimumPrice = .69m;
            Console.WriteLine($"Product instance created passing parameters. Name: {Name}");
        }

        private DateTime? availabilityDate;

        public DateTime? AvailabilityDate
        {
            get { return availabilityDate; }
            set { availabilityDate = value; }
        }

        private Vendor vendor;

        public Vendor Vendor
        {
            get 
            {
                if (vendor == null)
                    vendor = new Vendor();
                return vendor;                
            }
            set { vendor = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string SayHello()
        {
            vendor.SendWelcomeEmail("Message from Product");

            var emailservice = new EmailService();
            emailservice.SendMessage("New Product", this.Name, "sales@ivia.com.br");

            var loggingService = LogAction("Saying hello");

            return  $"Hello {Name} ({Id}): {Description}. " +
                    $"Available on: {AvailabilityDate?.ToShortDateString()}";
        }
    }
}