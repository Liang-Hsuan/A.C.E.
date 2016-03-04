using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace AshlinCustomerEnquiry.supportingClasses.brightpearl
{
    /*
     * A class that do API integration with Brightpearl
     */
    [Serializable()]
    public class BPconnect
    {
        // fields for brightpearl integration
        private GetRequest get;
        private PostRequest post;

        /* constructor that initialize GetRequest class */
        public BPconnect()
        {
            // initialize API authentication
            SqlConnection authenticationConnection = new SqlConnection(Properties.Settings.Default.ASCMcs);
            SqlCommand getAuthetication = new SqlCommand("SELECT Field3_Value, Field1_Value FROM ASCM_Credentials WHERE Source = \'Brightpearl\';", authenticationConnection);
            authenticationConnection.Open();
            SqlDataReader reader = getAuthetication.ExecuteReader();
            reader.Read();
            string appRef = reader.GetString(0);
            string appToken = reader.GetString(1);
            authenticationConnection.Close();

            // initializes request fields
            get = new GetRequest(appRef, appToken);
            post = new PostRequest(appRef, appToken);
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

        /* a method that post new order / quote to brightpearl */
        public void postOrder(BPvalues value)
        {
            // get the contact id first
            string contactId = get.getCustomerId(value.FirstName, value.LastName, value.PostalCode);

            // if customer exists, add the current order under this customer
            if (contactId != null)
            {
                #region Customer Exist Case
                // post order
                string orderId = post.postOrderRequest(contactId, value);
                if (orderId == "Error")
                {
                    do
                    {
                        Thread.Sleep(5000);
                        orderId = post.postOrderRequest(contactId, value);
                    } while (orderId == "Error");
                }

                // post order row and reserve the item
                for (int i = 0; i < value.SKU.Length; i++)
                {
                    for (int j = 0; j < value.Quantity.Length; j++)
                    {
                        // post order row
                        string orderRowId = post.postOrderRowRequest(orderId, value, i, j);
                        if (orderRowId == "Error")
                        {
                            do
                            {
                                Thread.Sleep(5000);
                                orderRowId = post.postOrderRowRequest(orderId, value, i, j);
                            } while (orderRowId == "Error");
                        }

                        // post reservation 
                        string reservation = post.postReservationRequest(orderId, orderRowId, value, i , j);
                        if (reservation == "Error")
                        {
                            do
                            {
                                Thread.Sleep(5000);
                                post.postReservationRequest(orderId, orderRowId, value, i, j);
                            } while (reservation == "Error");
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region Cusomter Not Exist Case
                // post address with new customer
                string addressId = post.postAddressRequest(value);
                contactId = post.postContactRequest(addressId, value);

                // post order
                // post order
                string orderId = post.postOrderRequest(contactId, value);
                if (orderId == "Error")
                {
                    do
                    {
                        Thread.Sleep(5000);
                        orderId = post.postOrderRequest(contactId, value);
                    } while (orderId == "Error");
                }

                // post order row and reserve the item
                for (int i = 0; i < value.SKU.Length; i++)
                {
                    for (int j = 0; j < value.Quantity.Length; j++)
                    {
                        // post order row
                        string orderRowId = post.postOrderRowRequest(orderId, value, i, j);
                        if (orderRowId == "Error")
                        {
                            do
                            {
                                Thread.Sleep(5000);
                                orderRowId = post.postOrderRowRequest(orderId, value, i, j);
                            } while (orderRowId == "Error");
                        }

                        // post reservation 
                        string reservation = post.postReservationRequest(orderId, orderRowId, value, i, j);
                        if (reservation == "Error")
                        {
                            do
                            {
                                Thread.Sleep(5000);
                                post.postReservationRequest(orderId, orderRowId, value, i, j);
                            } while (reservation == "Error");
                        }
                    }
                }
                #endregion
            }
        }

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
            while (text[i] != '"' && text[i] != ',' && text[i] != '}')
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
         * A class that Get request from brightpearl
         */
        [Serializable()]
        private class GetRequest
        {
            // fields for web request
            private WebRequest request;
            private HttpWebResponse response;

            // fields for credentials
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
                        uri = "https://ws-use.brightpearl.com/public-api/ashlin/contact-service/contact-search?firstName=" + firstName;
                        break;
                    case 2:
                        uri = "https://ws-use.brightpearl.com/public-api/ashlin/contact-service/contact-search?lastName=" + lastName;
                        break;
                    case 3:
                        uri = "https://ws-use.brightpearl.com/public-api/ashlin/contact-service/contact-search?firstName=" + firstName + "&lastName=" + lastName;
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
                        uri = "https://ws-use.brightpearl.com/public-api/ashlin/contact-service/contact-search?primaryEmail=" + email;
                        break;
                    case 2:
                        company = replaceSpace(company);
                        uri = "https://ws-use.brightpearl.com/public-api/ashlin/contact-service/contact-search?companyName=" + company;
                        break;
                    case 3:
                        company = replaceSpace(company);
                        uri = "https://ws-use.brightpearl.com/public-api/ashlin/contact-service/contact-search?primaryEmail=" + email + "&companyName=" + company;
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

            /* a method that return the only corresponding customer */
            public string getCustomerId(string firstName, string lastName, string postalCode)
            {
                // get all the customer id that exist with the given first name and last name
                string[] list = getCustomerIdWithName(firstName, lastName, 3);

                // the case if there is no cusomter exist -> return nothing
                if (list == null)
                    return null;

                // generate uri for getting more information on the customer id found to compare result
                string uri = "https://ws-use.brightpearl.com/public-api/ashlin/contact-service/contact/";

                int number = list.Length;
                for (int i = 0; i < number; i++)
                    uri += list[i] + ',';

                uri = uri.Remove(uri.LastIndexOf(',')) + "?includeOptional=customFields,postalAddresses";

                request = WebRequest.Create(uri);
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);
                request.Method = "GET";

                // get the response from the server
                response = (HttpWebResponse)request.GetResponse();

                // read all the text from JSON response
                string textJSON;
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    textJSON = streamReader.ReadToEnd();
                }

                // looping through each customer's postal code to see if the cutomer exist in these IDs
                for (int i = 0; i < number; i++)
                {
                    // cut the string to the cloest id 
                    textJSON = substringMethod(textJSON, "\"contactId\":", 11);

                    // get the text only for the current contact id
                    string copy;
                    if (textJSON.Contains("contactId"))
                        copy = textJSON.Remove(textJSON.IndexOf("contactId"));
                    else
                        copy = textJSON;

                    // postal code get
                    if (copy.Contains("postalCode"))
                    {
                        copy = substringMethod(copy, "postalCode", 13);
                        copy = getTarget(copy);
                    }
                    else
                        copy = "";

                    if (postalCode.Replace(" ", string.Empty) == copy.Replace(" ", string.Empty))
                        return list[i];
                }

                return null;
            }

            /* a method that get the detail info about the customer */
            public BPvalues[] getCustomerDetail(string[] customerId)
            { 
                // local fields for storing data
                List<BPvalues> valueList = new List<BPvalues>();
                int already = 0;

                // start getting data
                for (int i = 1; i <= customerId.Length / 200 + 1; i++)
                {
                    // set uri and the tracking current number complete
                    string uri = "https://ws-use.brightpearl.com/public-api/ashlin/contact-service/contact/";
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
                    request.Headers.Add("brightpearl-app-ref", appRef);
                    request.Headers.Add("brightpearl-account-token", appToken);
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
                string uri = "https://ws-use.brightpearl.com/2.0.0/ashlin/product-service/product-search?SKU=" + sku;

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

        /* 
         * A class that Post request to brightpearl 
         */
        [Serializable()]
        private class PostRequest
        {
            // fields for web request
            private HttpWebRequest request;
            private HttpWebResponse response;

            // fields for credentials
            private string appRef;
            private string appToken;
            
            // field for price calculation
            Price price = new Price();

            /* constructor to initialize the web request of app reference and app token */
            public PostRequest(string appRef, string appToken)
            {
                this.appRef = appRef;
                this.appToken = appToken;
            }

            /* post new address to API */
            public string postAddressRequest(BPvalues address)
            {
                string uri = "https://ws-use.brightpearl.com/2.0.0/ashlin/contact-service/postal-address";

                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);

                // get country ISO code
                string country;
                if (address.Country.Contains("US"))
                    country = "USA";
                else
                    country = "CAN";

                // generate the JSON file for address post
                string textJSON = "{\"addressLine1\":\"" + address.Address1 + "\",\"addressLine2\":\"" + address.Address2 + "\",\"addressLine3\":\"" + address.City + "\",\"addressLine4\":\"" + address.Province + "\",\"postalCode\":\"" + address.PostalCode + "\",\"countryIsoCode\":\"" + country + "\"}";

                // turn request string into a byte stream
                byte[] postBytes = Encoding.UTF8.GetBytes(textJSON);

                // send request
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(postBytes, 0, postBytes.Length);
                }

                // get the response from the server
                response = (HttpWebResponse)request.GetResponse();
                string result;
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                result = substringMethod(result, ":", 1);

                return getTarget(result);  //return the addresss ID
            }

            /* post new customer to API */
            public string postContactRequest(string addressID, BPvalues value)
            {
                string uri = "https://ws-use.brightpearl.com/2.0.0/ashlin/contact-service/contact";

                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);

                // generate JSON file for contact post
                string textJSON = "{\"firstName\":\"" + value.FirstName + "\",\"lastName\":\"" + value.LastName + "\",\"postAddressIds\":{\"DEF\":" + addressID + ",\"BIL\":" + addressID + ",\"DEL\":" + addressID + "}," +
                                  "\"communication\":{\"telephones\":{\"PRI\":\"" + value.Phone + "\"}},\"relationshipToAccount\":{\"isSupplier\": false,\"isStaff\":false,\"isCustomer\":true},\"financialDetails\":{\"priceListId\": 5}}";

                // turn request string into a byte stream
                byte[] postBytes = Encoding.UTF8.GetBytes(textJSON);

                // send request
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(postBytes, 0, postBytes.Length);
                }

                // get the response from the server
                response = (HttpWebResponse)request.GetResponse();
                string result;
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                int index = result.IndexOf(":") + 1;
                int length = index;
                while (Char.IsNumber(result[length]))
                {
                    length++;
                }
                string contactID = result.Substring(index, length - index);

                return contactID;  //return the contact ID
            }

            /* post new order to API */
            public string postOrderRequest(string contactID, BPvalues value)
            {
                string uri = "https://ws-use.brightpearl.com/2.0.0/ashlin/order-service/order";

                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);

                // get channel id from the country of the customer
                string channelId;
                if (value.Country.Contains("US"))
                    channelId = "12";
                else
                    channelId = "13";

                // generate JSON file for order post
                string textJSON = "{\"orderTypeCode\":\"SO\",\"placeOn\":\"" + DateTime.Today.ToString("yyyy-MM-dd") + "T00:00:00+00:00\",\"orderStatus\":{\"orderStatusId\":3}," +
                                  "\"currency\":{\"orderCurrencyCode\":\"CAD\"},\"parties\":{\"customer\":{\"contactId\":" + contactID + "}},\"assignment\":{\"current\":{\"channelId\":" + channelId + "}}}";


                // turn request string into a byte stream
                byte[] postBytes = Encoding.UTF8.GetBytes(textJSON);

                // send request
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(postBytes, 0, postBytes.Length);
                }

                // get the response from the server
                try      // might have server internal error, so do it in try and catch
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch    // HTTP response 500
                {
                    return "Error";    // cannot post order, return error instead
                }

                string result;
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                result = substringMethod(result, ":", 1);

                return getTarget(result);  //return the order ID
            }

            /* post new order row to API */
            public string postOrderRowRequest(string orderID, BPvalues value, int skuIndex, int quantityIndex)
            {
                // get product id
                GetRequest get = new GetRequest(appRef, appToken);
                string productId = get.getProductId(value.SKU[skuIndex]);

                // fields for web request
                string uri = "https://ws-use.brightpearl.com/2.0.0/ashlin/order-service/order/" + orderID + "/row";
                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);

                // generate JSON file for order row post
                string textJSON;
                string sku = value.SKU[skuIndex];
                int quantity = value.Quantity[quantityIndex];
                bool imprint = value.Logo;
                bool rush = value.Rush;
                if (productId != null)
                {
                    textJSON = "{\"productId\":\"" + productId + "\",\"quantity\":{\"magnitude\":\"" + quantity + "\"},\"rowValue\":{\"taxCode\":\"T\",\"rowNet\":{\"value\":\"" + price.getPrice(sku, quantity, imprint, rush) + "\"},\"rowTax\":{\"value\":\"0\"}}}";
                }
                else
                {
                    textJSON = "{\"productName\":\"" + value.Description[skuIndex] + " "+ sku + "\",\"quantity\":{\"magnitude\":\"" + quantity + "\"},\"rowValue\":{\"taxCode\":\"T\",\"rowNet\":{\"value\":\"" + price.getPrice(sku, quantity, imprint, rush) + "\"},\"rowTax\":{\"value\":\"0\"}}}";
                }


                // turn request string into a byte stream
                byte[] postBytes = Encoding.UTF8.GetBytes(textJSON);

                // send request
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(postBytes, 0, postBytes.Length);
                }

                // get the response from server
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch
                {
                    return "Error";        // 503 Server Unabailable
                }
                string result;
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                result = substringMethod(result, ":", 1);

                return getTarget(result);  //return the order row ID
            }

            /* post reservation request to API and return the message*/
            public string postReservationRequest(string orderID, string orderRowID, BPvalues value, int skuIndex, int quantityIndex)
            {
                // get product id
                GetRequest get = new GetRequest(appRef, appToken);
                string productId = get.getProductId(value.SKU[skuIndex]);

                string uri = "https://ws-use.brightpearl.com/2.0.0/ashlin/warehouse-service/order/" + orderID + "/reservation/warehouse/2";
                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);

                // generate JSON file for order row post
                string textJSON;
                if (productId != null)
                {
                    textJSON = "{\"products\": [{\"productId\": \"" + productId + "\",\"salesOrderRowId\": \"" + orderRowID + "\",\"quantity\":\"" + value.Quantity[quantityIndex] + "\"}]}";
                }
                else
                {
                    return null;
                }

                // turn request string into a byte stream
                byte[] postBytes = Encoding.UTF8.GetBytes(textJSON);

                // send request
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(postBytes, 0, postBytes.Length);
                }

                // get response from the server to see if there has error or not
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        response = e.Response as HttpWebResponse;
                        if ((int)response.StatusCode == 503)
                        {
                            return "Error";    // web server 503 server unavailable
                        }
                    }
                }

                return null;
            }
        }
    }

    /*
     * A class that calculate the price 
     */
    [Serializable()]
    public class Price
    {
        // fields for storing discount matrix values
        private double[] list = new double[11];

        /* constructor that initialize discount matrix list field */
        public Price()
        {
            SqlConnection connection = new SqlConnection(Properties.Settings.Default.Designcs);

            //  [0] 1 net standard, [1] 6 net standard, [2] 24 net standard, [3] 50 net standard, [4] 100 net standard, [5] 250 net standard, [6] 500 net standard, [7] 1000 net standard, [8] 2500 net standard, [9] rush net
            SqlCommand command = new SqlCommand("SELECT [1_Net_Standard Delivery], [6_Net_Standard Delivery], [24_Net_Standard Delivery], [50_Net_Standard Delivery], [100_Net_Standard Delivery], [250_Net_Standard Delivery], [500_Net_Standard Delivery], [1000_Net_Standard Delivery], [2500_Net_Standard Delivery], [RUSH_Net_25_wks] "
                                              + "FROM ref_discount_matrix;", connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            for (int i = 0; i <= 8; i++)
            {
                list[i] = reader.GetDouble(i);
            }
            reader.Close();
            // [10] multiplier
            command = new SqlCommand("SELECT [MSRP Multiplier] FROM ref_msrp_multiplier;", connection);
            reader = command.ExecuteReader();
            reader.Read();
            list[9] = reader.GetDouble(0);
            connection.Close();
        }

        /* a method that return the price from the given information of the product */
        public double getPrice(string sku, int quantity, bool imprint, bool rush)
        {
            // first get the base price of the sku and calculate msrp -> msrp will also be the return value
            double msrp = list[10] * getPrice(sku);

            if (imprint)
            {
                // the case if it is imprinted
                // calculate run charge
                double runcharge = Math.Round(msrp * 0.05) / 0.6;
                if (runcharge > 8)
                    runcharge = 8;
                else if (runcharge < 1)
                    runcharge = 1;

                if (rush)
                {
                    // the case if it is rush
                    if (quantity < 6)
                        return (msrp + runcharge) * list[0] * list[9];
                    if (quantity >= 6 && quantity < 25)
                        return (msrp + runcharge) * list[1] * list[9];
                    if (quantity >= 25 && quantity < 50)
                        return (msrp + runcharge) * list[2] * list[9];
                    if (quantity >= 50 && quantity < 100)
                        return (msrp + runcharge) * list[3] * list[9];
                    if (quantity >= 100 && quantity < 250)
                        return (msrp + runcharge) * list[4] * list[9];
                    if (quantity >= 250 && quantity < 500)
                        return (msrp + runcharge) * list[5] * list[9];
                    if (quantity >= 500 && quantity < 1000)
                        return (msrp + runcharge) * list[6] * list[9];
                    if (quantity >= 1000 && quantity < 2500)
                        return (msrp + runcharge) * list[7] * list[9];
                    if (quantity >= 2500)
                        return (msrp + runcharge) * list[8] * list[9];
                }
                else
                {
                    // the case if it is not rush
                    if (quantity < 6)
                        return (msrp + runcharge) * list[0];
                    if (quantity >= 6 && quantity < 25)
                        return (msrp + runcharge) * list[1];
                    if (quantity >= 25 && quantity < 50)
                        return (msrp + runcharge) * list[2];
                    if (quantity >= 50 && quantity < 100)
                        return (msrp + runcharge) * list[3];
                    if (quantity >= 100 && quantity < 250)
                        return (msrp + runcharge) * list[4];
                    if (quantity >= 250 && quantity < 500)
                        return (msrp + runcharge) * list[5];
                    if (quantity >= 500 && quantity < 1000)
                        return (msrp + runcharge) * list[6];
                    if (quantity >= 1000 && quantity < 2500)
                        return (msrp + runcharge) * list[7];
                    if (quantity >= 2500)
                        return (msrp + runcharge) * list[8];
                }
            }
            else
            {
                // the case if it is blank
                if (rush)
                {
                    // the case if it is rush
                    if (quantity < 6)
                        msrp *= list[0] * list[9];
                    else if (quantity >= 6 && quantity < 25)
                        msrp *= list[1] * list[9];
                    else if (quantity >= 25 && quantity < 50)
                        msrp *= list[2] * list[9];
                    else if (quantity >= 50 && quantity < 100)
                        msrp *= list[3] * list[9];
                    else if (quantity >= 100 && quantity < 250)
                        msrp *= list[4] * list[9];
                    else if (quantity >= 250 && quantity < 500)
                        msrp *= list[5] * list[9];
                    else if (quantity >= 500 && quantity < 1000)
                        msrp *= list[6] * list[9];
                    else if (quantity >= 1000 && quantity < 2500)
                        msrp *= list[7] * list[9];
                    else 
                        msrp *= list[8] * list[9];
                }
                else
                {
                    // the case if it is not rush
                    if (quantity < 6)
                        msrp *= list[0];
                    else if (quantity >= 6 && quantity < 25)
                        msrp *= list[1];
                    else if (quantity >= 25 && quantity < 50)
                        msrp *= list[2];
                    else if (quantity >= 50 && quantity < 100)
                        msrp *= list[3];
                    else if (quantity >= 100 && quantity < 250)
                        msrp *= list[4];
                    else if (quantity >= 250 && quantity < 500)
                        msrp *= list[5];
                    else if (quantity >= 500 && quantity < 1000)
                        msrp *= list[6];
                    else if (quantity >= 1000 && quantity < 2500)
                        msrp *= list[7];
                    else
                        msrp *= list[8];
                }
            }

            return msrp;
        }

        /* a supporting method that return the base price of the given sku */
        private double getPrice(string sku)
        {
            double basePrice;

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.Designcs))
            {
                SqlCommand command = new SqlCommand("SELECT Base_Price FROM master_SKU_Attributes WHERE SKU_Ashlin = \'" + sku + "\';", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                basePrice = Convert.ToDouble(reader.GetValue(0));
            }

            return basePrice;
        }
    }
}