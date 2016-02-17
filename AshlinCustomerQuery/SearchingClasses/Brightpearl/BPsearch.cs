using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;

namespace AshlinCustomerQuery.SearchingClasses.Brightpearl
{
    /* 
     * A class that get the customer information on Brightpearl 
     */
    public class BPsearch
    {
        // field for getting response from brightpearl
        private GetRequest get;

        /* constructor that initialize GetRequest class */
        public BPsearch()
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

        /* a method that accept name as parameters to return the customer detail */
        public BPvalues[] getCustomersDetails(string firstName, string lastName)
        {
            // local field for storing data
            List<BPvalues> list = new List<BPvalues>();

            // get all the id that correspond to the name given
            string[] idSet = get.getCustomerId(firstName, lastName);

            // the case if there is no result
            if (idSet == null)
            {
                return null;
            }

            // generating details
            foreach (string id in idSet)
            {
                list.Add(get.getCustomerDetail(id));
            }

            return list.ToArray();
        }

        /* a method that accept email / company parameters to return the customer detail -> [1] email only, [2] company only, [3] both */
        public BPvalues[] getCustomersDetails(string email, string company, int choice)
        {
            // local field for storing data
            List<BPvalues> list = new List<BPvalues>();

            // get all the id that correspond to the informaiton given
            string[] idSet = get.getCustomerId(email, company, choice);

            // the case if no result
            if (idSet == null)
            {
                return null;
            }

            // generating details
            foreach (string id in idSet)
            {
                list.Add(get.getCustomerDetail(id));
            }

            return list.ToArray();
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
            /* get the customer id from the given first name and last name */
            public string[] getCustomerId(string firstName, string lastName)
            {
                string uri = "https://ws-use.brightpearl.com/public-api/ashlintest/contact-service/contact-search?firstName=" + firstName + "&lastName=" + lastName;

                // post request to uri
                request = WebRequest.Create(uri);
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);
                request.Method = "GET";

                // get the response from the server
                string textJSON;
                response = (HttpWebResponse) request.GetResponse();

                // read all the text from JSON response
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                   textJSON = streamReader.ReadToEnd();
                }
                int number = textJSON[textJSON.IndexOf("resultsReturned") + 17] - '0';

                // the case there is no customer exists
                if (number < 1)
                    return null;

                // start getting customer id
                // this is for the first id
                string[] list = new string[number];
                textJSON = textJSON.Substring(textJSON.LastIndexOf("results") + 11);
                int index = 0;
                int length = index;
                while (Char.IsNumber(textJSON[length]))
                {
                    length++;
                }
                list[0] = textJSON.Substring(index, length - index);
                textJSON = textJSON.Substring(textJSON.IndexOf('[') + 1);

                // proceed to next token and get the id (if have more than 1)
                for (int i = 1; i < number; i++)
                {
                    index = 0;
                    length = index;
                    while (Char.IsNumber(textJSON[length]))
                    {
                        length++;
                    }
                    list[i] = textJSON.Substring(index, length - index);

                    // proceed to next token
                    textJSON = textJSON.Substring(textJSON.IndexOf('[') + 1);
                }

                return list;
            }

            /* get the customer id from the given email address and company name -> [1] email only, [2] company only, [3] both */
            public string[] getCustomerId(string email, string company, int choice)
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

                textJSON = textJSON.Substring(textJSON.IndexOf("resultsReturned") + 17);
                int index = 0;
                while (Char.IsNumber(textJSON[index]))
                {
                    index++;
                }
                int number = Convert.ToInt32(textJSON.Substring(0, index));

                // the case there is no customer exists
                if (number < 1)
                    return null;

                // start getting customer id
                // this is for the first id
                string[] list = new string[number];
                textJSON = textJSON.Substring(textJSON.LastIndexOf("results") + 11);
                index = 0;
                while (Char.IsNumber(textJSON[index]))
                {
                    index++;
                }
                list[0] = textJSON.Substring(0, index);
                textJSON = textJSON.Substring(textJSON.IndexOf('[') + 1);

                // proceed to next token and get the id (if have more than 1)
                for (int i = 1; i < number; i++)
                {
                    index = 0;
                    while (Char.IsNumber(textJSON[index]))
                    {
                        index++;
                    }
                    list[i] = textJSON.Substring(0, index);

                    // proceed to next token
                    textJSON = textJSON.Substring(textJSON.IndexOf('[') + 1);
                }

                return list;
            }

            /* a method that get the detail info about the customer */
            public BPvalues getCustomerDetail(string customerId)
            {
                // generate search uri
                string uri = "https://ws-use.brightpearl.com/public-api/ashlintest/contact-service/contact/" +
                             customerId + "?includeOptional=customFields,postalAddresses";

                // post request to uri
                request = WebRequest.Create(uri);
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);
                request.Method = "GET";

                #region Getting Response
                // get the response from the server
                string textJSON = "";
                try
                {
                    response = (HttpWebResponse) request.GetResponse();

                    // read all the text from JSON response
                    using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        textJSON = streamReader.ReadToEnd();
                    }
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        response = e.Response as HttpWebResponse;
                        if ((int) response.StatusCode == 404)
                        {
                            return null; // web server 404 not found -> no contact found
                        }
                    }
                }
                #endregion

                #region Data Retrieve
                // starting putting data to the BPvalues object, initialize a BPvalues object first
                BPvalues value = new BPvalues();

                // first name get
                textJSON = textJSON.Substring(textJSON.IndexOf("firstName") + 12);
                int index = 0;
                while (textJSON[index] != '"')
                {
                    index++;
                }
                value.FirstName = textJSON.Substring(0, index);

                // last name get
                textJSON = textJSON.Substring(textJSON.IndexOf("lastName") + 11);
                index = 0;
                while (textJSON[index] != '"')
                {
                    index++;
                }
                value.LastName = textJSON.Substring(0, index);

                // address 1 get
                if (textJSON.Contains("addressLine1"))
                {
                    textJSON = textJSON.Substring(textJSON.IndexOf("addressLine1") + 15);
                    index = 0;
                    while (textJSON[index] != '"')
                    {
                        index++;
                    }
                    value.Address1 = textJSON.Substring(0, index);
                }

                // address 2 get
                if (textJSON.Contains("addressLine2"))
                {
                    textJSON = textJSON.Substring(textJSON.IndexOf("addressLine2") + 15);
                    index = 0;
                    while (textJSON[index] != '"')
                    {
                        index++;
                    }
                    value.Address2 = textJSON.Substring(0, index);
                }

                // city get
                if (textJSON.Contains("addressLine3"))
                {
                    textJSON = textJSON.Substring(textJSON.IndexOf("addressLine3") + 15);
                    index = 0;
                    while (textJSON[index] != '"')
                    {
                        index++;
                    }
                    value.City = textJSON.Substring(0, index);
                }

                // province get
                if (textJSON.Contains("addressLine4"))
                {
                    textJSON = textJSON.Substring(textJSON.IndexOf("addressLine4") + 15);
                    index = 0;
                    while (textJSON[index] != '"')
                    {
                        index++;
                    }
                    value.Province = textJSON.Substring(0, index);
                }

                // country get
                textJSON = textJSON.Substring(textJSON.IndexOf("countryIsoCode") + 17);
                index = 0;
                while (textJSON[index] != '"')
                {
                    index++;
                }
                value.Country = textJSON.Substring(0, index);

                // email get
                int length;
                if (textJSON[textJSON.IndexOf("emails") + 9] != '}')
                {
                    textJSON = textJSON.Substring(textJSON.IndexOf("emails") + 7);
                    index = textJSON.IndexOf("email") + 8;
                    length = index;
                    while (textJSON[length] != '"')
                    {
                        length++;
                    }
                    value.Email = textJSON.Substring(index, length - index);
                }

                // phone get
                if (textJSON[textJSON.IndexOf("telephones") + 13] != '}')
                {
                    textJSON = textJSON.Substring(textJSON.IndexOf("telephones") + 11);
                    index = textJSON.IndexOf("PRI") + 6;
                    length = index;
                    while (textJSON[length] != '"')
                    {
                        length++;
                    }
                    value.Phone = textJSON.Substring(index, length - index);
                }

                // company get
                string copy = textJSON.Substring(textJSON.IndexOf("organisation") + 13);
                if (copy.Contains("name"))
                {
                    index = copy.IndexOf("name") + 7;
                    length = index;
                    while (copy[length] != '"')
                    {
                        length++;
                    }
                    value.Company = copy.Substring(index, length - index);
                }
                #endregion

                return value;
            }
            #endregion
        }
    }
}