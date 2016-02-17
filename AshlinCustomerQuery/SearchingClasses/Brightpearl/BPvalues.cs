using System;

namespace AshlinCustomerQuery.SearchingClasses.Brightpearl
{
    /* 
     * A supporting class for BPsearch that store all necessary data 
     */
    [Serializable()]
    public class BPvalues
    {
        // fields for the infomation about the product 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }

        /* first constructor with no arguments */
        public BPvalues()
        {
            FirstName = "";
            LastName = "";
            Company = "";
            Phone = "";
            Email = "";
            Address1 = "";
            Address2 = "";
            City = "";
            Province = "";
            Country = "";
        }

        /* seconde constructor that accept all fields parameters */
        public BPvalues(string firstName, string lastName, string company, string phone, string email, string address1, string address2, string city, string province, string country)
        {
            FirstName = firstName;
            LastName = lastName;
            Company = company;
            Phone = phone;
            Email = email;
            Address1 = address1;
            Address2 = address2;
            City = city;
            Province = province;
            Country = country;
        }
    }
}