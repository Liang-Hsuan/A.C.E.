using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AshlinCustomerQuery.SearchingClasses.Sage
{
    /* 
     * A supporting class for Sagesearch that store all necessary data 
     */
    public class SageValues
    {
        // fields for the infomation about the customer 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Sage { get; set; }

        /* first constructor that accept no agrument */
        public SageValues()
        {
            FirstName = "";
            LastName = "";
            Company = "";
            Phone = "";
            Email = "";
        }

        /* second constructor that accept all parameters */
        public SageValues(string firstName, string lastName, string company, string phone, string email, string sage)
        {
            FirstName = firstName;
            LastName = lastName;
            Company = company;
            Phone = phone;
            Email = email;
            Sage = sage;
        }
    }
}