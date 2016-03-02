using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Threading;

namespace AshlinCustomerEnquiry.supportingClasses.brightpearl
{
    /*
     * A class that do API integration with Brightpearl
     */
    public class BPconnect
    {
        // field for getting response from brightpearl
        private GetRequest get;

        /* constructor that initialize GetRequest class */
        public BPconnect()
        {
            using (SqlConnection authenticationConnection = new SqlConnection(Properties.Settings.Default.ASCMcs))
            {
                SqlCommand getAuthetication = new SqlCommand("SELECT Field3_Value, Field1_Value FROM ASCM_Credentials WHERE Source = \'Brightpearl Testing\';", authenticationConnection);
                authenticationConnection.Open();
                SqlDataReader reader = getAuthetication.ExecuteReader();
                reader.Read();
                get = new GetRequest(reader.GetString(0), reader.GetString(1));
            }
        }

        #region Get
        /* a method that accept name as parameters to return the customer detail */
        public BPvalues[] getCustomerWithName(string firstName, string lastName, int choice)
        {
            // get all the id that correspond to the name given
            string[] idSet = get.getCustomerIdWithName(firstName, lastName, choice);

            // the case if there is no result or there are too many result
            if (idSet == null)
                return null;
            else if (idSet[0] == "-1")
            {
                BPvalues[] invalid = new BPvalues[1];
                invalid[0] = new BPvalues();
                invalid[0].FirstName = "-1";
                return invalid;
            }

            return get.getCustomerDetail(idSet);
        }

        /* a method that accept email / company parameters to return the customer detail -> [1] email only, [2] company only, [3] both */
        public BPvalues[] getCustomerWithInfo(string email, string company, int choice)
        {
            // get all the id that correspond to the informaiton given
            string[] idSet = get.getCustomerIdWithInfo(email, company, choice);

            // the case if no result or there are too many result
            if (idSet == null)
            {
                return null;
            }
            else if (idSet[0] == "-1")
            {
                BPvalues[] invalid = new BPvalues[1];
                invalid[0] = new BPvalues();
                invalid[0].FirstName = "-1";
                return invalid;
            }

            return get.getCustomerDetail(idSet);
        }
        #endregion

        #region ID Return
        /* return the list of customer id */
        public string[] getContactId(string firstName, string lastName)
        {
            return get.getCustomerIdWithName(firstName, lastName, 3);
        }

        /* return the product id of the given sku */
        public string getProductId(string sku)
        {
            return get.getProductId(sku);
        }
        #endregion

        #region Supporting Methods
        /* a method that substring the given string */
        private static string substringMethod(string original, string startingString, int additionIndex)
        {
            string copy = original;
            copy = original.Substring(original.IndexOf(startingString) + additionIndex);

            return copy;
        }

        /* a method that get the next target token */
        private static string getTarget(string text)
        {
            int i = 0;
            while (text[i] != '"' && text[i] != ',')
            {
                i++;
            }

            return text.Substring(0, i);
        }

        /* a method that replace the space with %20 from the given string */
        private static string replaceSpace(string englishString)
        {
            List<char> englishList = new List<char>();

            // replace space with %20
            foreach (char ch in englishString)
            {
                if (ch == ' ')
                {
                    englishList.Add('%');
                    englishList.Add('2');
                    englishList.Add('0');
                }
                else
                {
                    englishList.Add(ch);
                }
            }

            return englishString;
        }
        #endregion

        /*
         * A class that get JSON response
         */
        private class GetRequest
        {
            private WebRequest request;
            private HttpWebResponse response;
            private string appRef;
            private string appToken;

            /* constructor to initialize the web request of app reference and app token */
            public GetRequest(string appRef, string appToken)
            {
                this.appRef = appRef;
                this.appToken = appToken;
            }

            #region Customer Search
            /* get the customer id from the given first name and last name -> [1] first name only, [2] last name only, [3] both */
            public string[] getCustomerIdWithName(string firstName, string lastName, int choice)
            {
                // generate uri from user's choice
                string uri;
                switch (choice)
                {
                    case 1:
                        uri = "https://ws-use.brightpearl.com/public-api/ashlintest/contact-service/contact-search?firstName=" + firstName;
                        break;
                    case 2:
                        uri = "https://ws-use.brightpearl.com/public-api/ashlintest/contact-service/contact-search?lastName=" + lastName;
                        break;
                    case 3:
                        uri = "https://ws-use.brightpearl.com/public-api/ashlintest/contact-service/contact-search?firstName=" + firstName + "&lastName=" + lastName;
                        break;
                    default:
                        return null;
                }

                // post request to uri
                request = WebRequest.Create(uri);
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);
                request.Method = "GET";

                // get the response from the server
                string textJSON;
                response = (HttpWebResponse)request.GetResponse();

                // read all the text from JSON response
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    textJSON = streamReader.ReadToEnd();
                }

                // get the number of result
                textJSON = substringMethod(textJSON, "resultsAvailable", 18);
                int number = Convert.ToInt32(getTarget(textJSON));

                // the case there is no customer exists or the result is too many
                if (number < 1)
                    return null;
                else if (number > 500)
                {
                    string[] invalid = { "-1" };
                    return invalid;
                }
                    

                // start getting customer id
                // this is for the first id
                string[] list = new string[number];
                textJSON = substringMethod(textJSON, "results\":", 11);
                list[0] = getTarget(textJSON);
                textJSON = substringMethod(textJSON, "],[", 3);

                // proceed to next token and get the id (if have more than 1)
                for (int i = 1; i < number; i++)
                {
                    list[i] = getTarget(textJSON);

                    // proceed to next token
                    textJSON = substringMethod(textJSON, "],[", 3);
                }

                return list;
            }

            /* get the customer id from the given email address and company name -> [1] email only, [2] company only, [3] both */
            public string[] getCustomerIdWithInfo(string email, string company, int choice)
            {
                // generate uri from user's choice
                string uri;
                switch (choice)
                {
                    case 1:
                        uri = "https://ws-use.brightpearl.com/public-api/ashlintest/contact-service/contact-search?primaryEmail=" + email;
                        break;
                    case 2:
                        company = replaceSpace(company);
                        uri = "https://ws-use.brightpearl.com/public-api/ashlintest/contact-service/contact-search?companyName=" + company;
                        break;
                    case 3:
                        company = replaceSpace(company);
                        uri = "https://ws-use.brightpearl.com/public-api/ashlintest/contact-service/contact-search?primaryEmail=" + email + "&companyName=" + company;
                        break;
                    default:
                        return null;
                }

                // post request to uri
                request = WebRequest.Create(uri);
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);
                request.Method = "GET";

                // get the response from the server
                string textJSON;
                response = (HttpWebResponse)request.GetResponse();

                // read all the text from JSON response
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    textJSON = streamReader.ReadToEnd();
                }
                textJSON = substringMethod(textJSON, "resultsAvailable", 18);
                int number = Convert.ToInt32(getTarget(textJSON));

                // the case there is no customer exists or there are too many results
                if (number < 1)
                    return null;
                else if (number > 500)
                {
                    string[] invalid = { "-1" };
                    return invalid;
                }

                // start getting customer id
                // this is for the first id
                string[] list = new string[number];
                textJSON = substringMethod(textJSON, "results\":", 11);
                list[0] = getTarget(textJSON);
                textJSON = substringMethod(textJSON, "],[", 3);

                // proceed to next token and get the id (if have more than 1)
                for (int i = 1; i < number; i++)
                {
                    list[i] = getTarget(textJSON);

                    // proceed to next token
                    textJSON = substringMethod(textJSON, "],[", 3);
                }

                return list;
            }

            /* a method that get the detail info about the customer */
            public BPvalues[] getCustomerDetail(string[] customerId)
            {
                // generate search uri
                string uri = "https://ws-use.brightpearl.com/public-api/ashlintest/contact-service/contact/" +
                             customerId + "?includeOptional=customFields,postalAddresses";

                // post request to uri
                request = WebRequest.Create(uri);
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);
                request.Method = "GET";

                // local fields for storing data
                List<BPvalues> valueList = new List<BPvalues>();
                int already = 0;

                // start getting data
                for (int i = 1; i <= customerId.Length / 200 + 1; i++)
                {
                    // set uri and the tracking current number complete
                    uri = "https://ws-use.brightpearl.com/public-api/ashlintest/contact-service/contact/";
                    int current = 1;

                    // adding IDs to the uri
                    int currentAlready = already;
                    while (current <= 200 && current <= customerId.Length - currentAlready)
                    {
                        uri += customerId[already] + ',';
                        current++;
                        already++;
                    }

                    // complete the uri address
                    uri = uri.Remove(uri.LastIndexOf(',')) + "?includeOptional=customFields,postalAddresses";

                    #region Post and Get
                    // post request to uri
                    request = WebRequest.Create(uri);
                    request.Headers.Add("brightpearl-app-ref", "ashlintest_intern-1002");
                    request.Headers.Add("brightpearl-account-token", "aZroyMTQ7Lf3EygEbyvXYTsYnDB7S4HjgHjuxjbMA00=");
                    request.Method = "GET";

                    // get the response from the server
                    response = (HttpWebResponse)request.GetResponse();

                    // read all the text from JSON response
                    string textJSON;
                    using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        textJSON = streamReader.ReadToEnd();
                    }
                    #endregion

                    // putting data to BPvalue for each id
                    for (int j = 0; j < current - 1; j++)
                    {
                        BPvalues value = new BPvalues();

                        #region Data Retrieve
                        // starting putting data to the BPvalues object, initialize a BPvalues object first
                        int index;

                        // first name get
                        textJSON = substringMethod(textJSON, "firstName", 12);
                        value.FirstName = getTarget(textJSON);

                        // last name get
                        textJSON = substringMethod(textJSON, "lastName", 11);
                        value.LastName = getTarget(textJSON);

                        // get the text only for the current contact id
                        string copy;
                        if (textJSON.Contains("contactId"))
                            copy = textJSON.Remove(textJSON.IndexOf("contactId"));
                        else
                            copy = textJSON;

                        // address 1 get
                        if (copy.Contains("addressLine1"))
                        {
                            copy = substringMethod(copy, "addressLine1", 15);
                            value.Address1 = getTarget(copy);
                        }

                        // address 2 get
                        if (copy.Contains("addressLine2"))
                        {
                            copy = substringMethod(copy, "addressLine2", 15);
                            value.Address2 = getTarget(copy);
                        }

                        // city get
                        if (copy.Contains("addressLine3"))
                        {
                            copy = substringMethod(copy, "addressLine3", 15);
                            value.City = getTarget(copy);
                        }

                        // province get
                        if (copy.Contains("addressLine4"))
                        {
                            copy = substringMethod(copy, "addressLine4", 15);
                            value.Province = getTarget(copy);
                        }

                        // postal code get
                        if (copy.Contains("postalCode"))
                        {
                            copy = substringMethod(copy, "postalCode", 13);
                            value.PostalCode = getTarget(copy);
                        }

                        // country get
                        if (copy.Contains("countryIsoCode"))
                        {
                            copy = substringMethod(copy, "countryIsoCode", 17);
                            value.Country = getTarget(copy);
                        }

                        // email get
                        int length;
                        if (copy[copy.IndexOf("emails") + 9] != '}')
                        {
                            copy = substringMethod(copy, "emails", 7);
                            index = copy.IndexOf("email") + 8;
                            length = index;
                            while (copy[length] != '"')
                                length++;
                            value.Email = copy.Substring(index, length - index);
                        }

                        // phone get
                        if (copy[copy.IndexOf("telephones") + 13] != '}')
                        {
                            copy = substringMethod(copy, "telephones", 11);
                            index = copy.IndexOf("PRI") + 6;
                            length = index;
                            while (copy[length] != '"')
                                length++;
                            value.Phone = copy.Substring(index, length - index);
                        }

                        // company get
                        copy = substringMethod(copy, "organisation", 13);
                        if (copy.Contains("name"))
                        {
                            index = copy.IndexOf("name") + 7;
                            length = index;
                            while (copy[length] != '"')
                                length++;
                            value.Company = copy.Substring(index, length - index);
                        }
                        #endregion

                        // final correction and adding
                        textJSON = substringMethod(textJSON, "contactId", 10);
                        valueList.Add(value);
                    }
                }

                return valueList.ToArray();
            }
            #endregion

            /* get the product id from the given sku */
            public string getProductId(string sku)
            {
                string uri = "https://ws-use.brightpearl.com/2.0.0/ashlintest/product-service/product-search?SKU=" + sku;

                // post request to uri
                request = WebRequest.Create(uri);
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);
                request.Method = "GET";

                // get the response from the server
                response = (HttpWebResponse)request.GetResponse();

                // read all the text from JSON response
                string textJSON = "";
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    textJSON = streamReader.ReadToEnd();
                }

                // the case there is no product exists
                if (textJSON[textJSON.IndexOf("resultsReturned") + 17] - '0' < 1)
                {
                    return null;
                }

                // starting getting product id
                int index = textJSON.LastIndexOf("results") + 11;
                int length = index;
                while (Char.IsNumber(textJSON[length]))
                {
                    length++;
                }

                return textJSON.Substring(index, length - index);
            }
        }
    }
}