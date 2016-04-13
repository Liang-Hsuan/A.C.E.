using System;
using System.Collections.Generic;

namespace AshlinCustomerEnquiry.supportingClasses.brightpearl
{
    /* 
    * A supporting class for BPsearch that store all necessary data 
    */
    [Serializable]
    public class BPvalues : IComparable<BPvalues>
    {
        // fields for customer information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        // field for order information
        public List<string> Sku { get; set; }
        public List<string> Description { get; set; }
        public List<int> Quantity { get; set; }
        public List<double> BasePrice { get; set; }
        public List<int> PricingTier { get; set; }
        public bool Logo { get; set; }
        public bool Rush { get; set; }
        public string Comment { get; set; }
        public DateTime DeliveryDate { get; set; }

        /* first constructor with no arguments */
        public BPvalues()
        {
            // customer field
            FirstName = "";
            LastName = "";
            Company = "";
            Phone = "";
            Email = "";
            Address1 = "";
            Address2 = "";
            City = "";
            Province = "";
            PostalCode = "";
            Country = "";

            // order field
            Sku = new List<string>();
            Description = new List<string>();
            Quantity = new List<int>();
            BasePrice = new List<double>();
            PricingTier = new List<int>();
            Logo = true;
            Rush = false;
            Comment = "";
            DeliveryDate = DateTime.Today;
        }

        /* seconde constructor that accept all fields parameters */
        public BPvalues(string firstName, string lastName, string company, string phone, string email, string address1, string address2, string city, string province, string postalCode, string country,
                        List<string> sku, List<string> description, List<int> quantity, List<double> basePrice, List<int> pricingTier, bool logo, bool rush, string comment, DateTime deliveryDate)
        {
            // customer information
            FirstName = firstName;
            LastName = lastName;
            Company = company;
            Phone = phone;
            Email = email;
            Address1 = address1;
            Address2 = address2;
            City = city;
            Province = province;
            PostalCode = postalCode;
            Country = country;

            // order information
            Sku = sku;
            Description = description;
            Quantity = quantity;
            BasePrice = basePrice;
            PricingTier = pricingTier;
            Logo = logo;
            Rush = rush;
            Comment = comment;
            DeliveryDate = deliveryDate;
        }

        /* compare method */
        public int CompareTo(BPvalues other)
        {
            return string.CompareOrdinal(FirstName, other.FirstName);
        }
    }
}