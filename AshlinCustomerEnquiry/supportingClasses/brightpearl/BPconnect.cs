using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;

namespace AshlinCustomerEnquiry.supportingClasses.brightpearl
{
    /*
     * A class that do API integration with Brightpearl
     */
    [Serializable]
    public class BPconnect
    {
        // fields for brightpearl integration
        private readonly GetRequest get;
        private readonly PostRequest post;

        /* constructor that initialize authentication for request classes */
        public BPconnect()
        {
            // initialize API authentication
            SqlConnection authenticationConnection = new SqlConnection(Properties.Settings.Default.ASCMcs);
            SqlCommand getAuthetication = new SqlCommand("SELECT Field3_Value, Field1_Value FROM ASCM_Credentials WHERE Source = 'Brightpearl';", authenticationConnection);
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

            // the case if it is a vaild result
            if (idSet[0] != "-1") return get.getCustomerDetail(idSet);

            // the case if there are too many result found
            BPvalues[] invalid = new BPvalues[1];
            invalid[0] = new BPvalues {FirstName = "-1"};
            return invalid;
        }

        /* a method that accept email / company parameters to return the customer detail -> [1] email only, [2] company only, [3] both */
        public BPvalues[] getCustomerWithInfo(string email, string company, int choice)
        {
            // get all the id that correspond to the informaiton given
            string[] idSet = get.getCustomerIdWithInfo(email, company, choice);

            // the case if no result or there are too many result
            if (idSet == null)
                return null;

            // the case if it is valid result
            if (idSet[0] != "-1") return get.getCustomerDetail(idSet);

            // the case if there are too many result found
            BPvalues[] invalid = new BPvalues[1];
            invalid[0] = new BPvalues {FirstName = "-1"};
            return invalid;
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
                while (post.HasError)
                {
                    Thread.Sleep(5000);
                    orderId = post.postOrderRequest(contactId, value);
                }

                // post order row and reserve the item
                for (int i = 0; i < value.SKU.Length; i++)
                {
                    for (int j = 0; j < value.Quantity.Length; j++)
                    {
                        // post order row
                        string orderRowId = post.postOrderRowRequest(orderId, value, i, j);
                        if (!post.HasError) continue;
                        do
                        {
                            Thread.Sleep(5000);
                            orderRowId = post.postOrderRowRequest(orderId, value, i, j);
                        } while (post.HasError);
                    }
                }

                // post comment
                post.patchComment(orderId, value.Comment);
                while (post.HasError)
                {
                    Thread.Sleep(5000);
                    post.patchComment(orderId, value.Comment);
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
                string orderId = post.postOrderRequest(contactId, value);
                while (post.HasError)
                {
                    Thread.Sleep(5000);
                    orderId = post.postOrderRequest(contactId, value);
                }

                // post order row and reserve the item
                for (int i = 0; i < value.SKU.Length; i++)
                {
                    for (int j = 0; j < value.Quantity.Length; j++)
                    {
                        // post order row
                        string orderRowId = post.postOrderRowRequest(orderId, value, i, j);
                        if (!post.HasError) continue;
                        do
                        {
                            Thread.Sleep(5000);
                            orderRowId = post.postOrderRowRequest(orderId, value, i, j);
                        } while (post.HasError);
                    }
                }

                //post comment
                post.patchComment(orderId, value.Comment);
                while (post.HasError)
                {
                    Thread.Sleep(5000);
                    post.patchComment(orderId, value.Comment);
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
            return original.Substring(original.IndexOf(startingString) + additionIndex);
        }

        /* a method that get the next target token */
        private static string getTarget(string text)
        {
            int i = 0;
            while (text[i] != '"' && text[i] != ',' && text[i] != '}')
                i++;

            return text.Substring(0, i);
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
            private readonly string appRef;
            private readonly string appToken;

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
                string textJson;
                response = (HttpWebResponse)request.GetResponse();

                // read all the text from JSON response
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    textJson = streamReader.ReadToEnd();

                // deserialize json to key value
                var info = new JavaScriptSerializer().Deserialize<Dictionary<string, dynamic>>(textJson);

                // get the number of result
                int number = info["response"]["metaData"]["resultsAvailable"];

                // the case there is no customer exists or the result is too many
                if (number < 1)
                    return null;
                if (number > 500)
                {
                    string[] invalid = { "-1" };
                    return invalid;
                }

                // start getting id
                string[] list = new string[number];
                for (int i = 0; i < number; i++)
                    list[i] = info["response"]["results"][i][0].ToString();

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
                        uri = "https://ws-use.brightpearl.com/public-api/ashlin/contact-service/contact-search?companyName=" + company.Replace(" ", "%20");
                        break;
                    case 3:
                        uri = "https://ws-use.brightpearl.com/public-api/ashlin/contact-service/contact-search?primaryEmail=" + email + "&companyName=" + company.Replace(" ", "%20");
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
                string textJson;
                response = (HttpWebResponse)request.GetResponse();

                // read all the text from JSON response
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    textJson = streamReader.ReadToEnd();

                // deserialize json to key value
                var info = new JavaScriptSerializer().Deserialize<Dictionary<string, dynamic>>(textJson);

                // get the number of result
                int number = info["response"]["metaData"]["resultsAvailable"];

                // the case there is no customer exists or the result is too many
                if (number < 1)
                    return null;
                if (number > 500)
                {
                    string[] invalid = { "-1" };
                    return invalid;
                }

                // start getting id
                string[] list = new string[number];
                for (int i = 0; i < number; i++)
                    list[i] = info["response"]["results"][i][0].ToString();

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
                string textJson;
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    textJson = streamReader.ReadToEnd();

                // deserialize json to key value
                var info = new JavaScriptSerializer().Deserialize<Dictionary<string, dynamic>>(textJson);

                // looping through each customer's postal code to see if the cutomer exist in these IDs
                for (int i = 0; i < number; i++)
                {
                    // get address id to get postal code
                    string address = info["response"][i]["postAddressIds"]["DEF"].ToString();

                    // get postal code
                    address = info["response"][0]["postalAddresses"][address]["postalCode"];

                    if (string.Equals(postalCode.Replace(" ", string.Empty), address.Replace(" ", string.Empty), StringComparison.CurrentCultureIgnoreCase))
                        return list[i];
                }

                return null;
            }

            /* a method that get the detail info about the customer */
            public BPvalues[] getCustomerDetail(string[] customerId)
            {
                // input error check
                if (customerId == null) throw new ArgumentNullException(nameof(customerId));

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
                    string textJson;
                    using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                        textJson = streamReader.ReadToEnd();
                    #endregion

                    // deserialize json to key value
                    var info = new JavaScriptSerializer().Deserialize<Dictionary<string, dynamic>>(textJson);

                    // putting data to BPvalue for each id
                    for (int j = 0; j < current - 1; j++)
                    {
                        BPvalues value = new BPvalues
                        {
                            FirstName = info["response"][j]["firstName"],
                            LastName = info["response"][j]["lastName"]
                        };

                        #region Data Retrieve
                        // get address id to get postal code
                        string addressId = info["response"][j]["postAddressIds"]["DEF"].ToString();

                        // get postal code
                        try { value.Address1 = info["response"][j]["postalAddresses"][addressId]["addressLine1"]; } catch { /* ignore */ }
                        try { value.Address2 = info["response"][j]["postalAddresses"][addressId]["addressLine2"]; } catch { /* ignore */ }
                        try { value.City = info["response"][j]["postalAddresses"][addressId]["addressLine3"]; } catch { /* ignore */ }
                        try { value.Province = info["response"][j]["postalAddresses"][addressId]["addressLine4"]; } catch { /* ignore */ }
                        try { value.PostalCode = info["response"][j]["postalAddresses"][addressId]["postalCode"]; } catch { /* ignore */ }
                        try { value.Country = info["response"][j]["postalAddresses"][addressId]["countryIsoCode"]; } catch { /* ignore */ }
                        try { value.Email = info["response"][j]["communication"]["emails"]["PRI"]["email"]; } catch { /* ignore */ }
                        try { value.Phone = info["response"][j]["communication"]["telephones"]["PRI"]; } catch { /* ignore */ }
                        try { value.Company = info["response"][j]["organisation"]["name"]; } catch { /* ignore */ }
                        #endregion

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
                string textJson;
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    textJson = streamReader.ReadToEnd();

                // deserialize json to key value
                var info = new JavaScriptSerializer().Deserialize<Dictionary<string, dynamic>>(textJson);

                // the case there is no product exists
                return info["response"]["metaData"]["resultsAvailable"] < 1 ? null : info["response"]["results"][0][0].ToString();
            }
        }

        /* 
         * A class that Post request to brightpearl 
         */
        [Serializable()]
        private class PostRequest
        {
            #region Fields Declaration
            // fields for web request
            private HttpWebRequest request;
            private HttpWebResponse response;

            // fields for credentials
            private readonly string appRef;
            private readonly string appToken;

            // field for getting price information
            private readonly Price price = new Price();

            // field for error indication
            public bool HasError { get; private set; }
            #endregion

            /* constructor to initialize the web request of app reference and app token */
            public PostRequest(string appRef, string appToken)
            {
                this.appRef = appRef;
                this.appToken = appToken;
            }

            /* post new address to API */
            public string postAddressRequest(BPvalues address)
            {
                const string uri = "https://ws-use.brightpearl.com/2.0.0/ashlin/contact-service/postal-address";

                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);

                // get country ISO code
                string country = address.Country.Contains("US") ? "USA" : "CAN";

                // generate the JSON file for address post
                string textJSON = "{\"addressLine1\":\"" + address.Address1 + "\",\"addressLine2\":\"" + address.Address2 + "\",\"addressLine3\":\"" + address.City + "\",\"addressLine4\":\"" + address.Province + "\",\"postalCode\":\"" + address.PostalCode + "\",\"countryIsoCode\":\"" + country + "\"}";

                // turn request string into a byte stream
                byte[] postBytes = Encoding.UTF8.GetBytes(textJSON);

                // send request
                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(postBytes, 0, postBytes.Length);

                // get the response from the server
                response = (HttpWebResponse)request.GetResponse();
                string result;
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    result = streamReader.ReadToEnd();

                result = substringMethod(result, ":", 1);
                return getTarget(result);  //return the addresss ID
            }

            /* post new customer to API */
            public string postContactRequest(string addressID, BPvalues value)
            {
                const string uri = "https://ws-use.brightpearl.com/2.0.0/ashlin/contact-service/contact";

                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);

                // generate JSON file for contact post
                string textJSON = "{\"firstName\":\"" + value.FirstName + "\",\"lastName\":\"" + value.LastName + "\",\"postAddressIds\":{\"DEF\":" + addressID + ",\"BIL\":" + addressID + ",\"DEL\":" + addressID + "},\"organisation\":{\"name\":\"" + value.Company + "\"}," +
                                  "\"communication\":{\"emails\":{\"PRI\":{\"email\":\"" + value.Email + "\"}},\"telephones\":{\"PRI\":\"" + value.Phone + "\"}},\"contactStatus\":{\"current\":{\"contactStatusId\":9}},\"relationshipToAccount\":{\"isSupplier\": false,\"isStaff\":false,\"isCustomer\":true},\"financialDetails\":{\"priceListId\": 5}}";

                // turn request string into a byte stream
                byte[] postBytes = Encoding.UTF8.GetBytes(textJSON);

                // send request
                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(postBytes, 0, postBytes.Length);

                // get the response from the server
                response = (HttpWebResponse)request.GetResponse();
                string result;
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    result = streamReader.ReadToEnd();

                result = substringMethod(result, ":", 1);
                return getTarget(result);  //return the contact ID
            }

            /* post new order to API */
            public string postOrderRequest(string contactID, BPvalues value)
            {
                // set has error to false
                HasError = false;

                const string uri = "https://ws-use.brightpearl.com/2.0.0/ashlin/order-service/order";

                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);

                #region Flags
                // get channel id from the country of the customer
                string[] currencyAndchannelId = value.Country.Contains("US") ? new[]{ "USD", "13"} : new[]{ "CAD", "14"}; 

                // get price list id depending on rush and logo
                int priceListId;
                if (value.Logo && value.Rush)
                    priceListId = 8;
                else if (value.Logo && !value.Rush)
                    priceListId = 5;
                else if (!value.Logo && value.Rush)
                    priceListId = 7;
                else
                    priceListId = 6;
                #endregion

                // generate JSON file for order post
                string textJSON = "{\"orderTypeCode\":\"SO\",\"priceListId\":" + priceListId + ",\"placeOn\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(' ', 'T') + "+00:00\",\"orderStatus\":{\"orderStatusId\":3}," +
                                  "\"currency\":{\"orderCurrencyCode\":\"" + currencyAndchannelId[0] + "\"},\"parties\":{\"customer\":{\"contactId\":" + contactID + "}},\"assignment\":{\"current\":{\"channelId\":" + currencyAndchannelId[1] + "}}}";


                // turn request string into a byte stream
                byte[] postBytes = Encoding.UTF8.GetBytes(textJSON);

                // send request
                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(postBytes, 0, postBytes.Length);

                // get the response from the server
                try      // might have server internal error, so do it in try and catch
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch    // HTTP response 500 or 503
                {
                    HasError = true; // cannot post order, set has error to true
                    return null;
                }
            
                string result;
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    result = streamReader.ReadToEnd();

                result = substringMethod(result, ":", 1);
                return getTarget(result);  //return the order ID
            }

            /* post new order row to API */
            public string postOrderRowRequest(string orderID, BPvalues value, int skuIndex, int quantityIndex)
            {
                // set has error to false
                HasError = false;

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

                // flags creation
                string sku = value.SKU[skuIndex];
                int quantity = value.Quantity[quantityIndex];
                bool imprint = value.Logo;
                bool rush = value.Rush;
                double netPrice = price.getPrice(sku, quantity, imprint, rush) * quantity;

                string taxCode;
                double taxRate;
                if (value.Country.Contains("US"))
                {
                    // the case if it's US order, no need tax
                    taxCode = "Z";
                    taxRate = 0;
                }
                else
                {
                    // the case if it's CA order, get tax code depend on region
                    switch (value.Province)
                    {
                        case "NB":
                            taxCode = "NB";
                            taxRate = 0.13;
                            break;
                        case "NF":
                            taxCode = "NF";
                            taxRate = 0.13;
                            break;
                        case "NL":
                            taxCode = "NL";
                            taxRate = 0.13;
                            break;
                        case "NS":
                            taxCode = "NS";
                            taxRate = 0.13;
                            break;
                        case "ON":
                            taxCode = "ON";
                            taxRate = 0.13;
                            break;
                        case "PEI":
                            taxCode = "PEI";
                            taxRate = 0.14;
                            break;
                        case "BC":
                            taxCode = "BC";
                            taxRate = 0.05;
                            break;
                        case "MAN":
                            taxCode = "MAN";
                            taxRate = 0.05;
                            break;
                        case "PQ":
                            taxCode = "PQ";
                            taxRate = 0.05;
                            break;
                        case "SK":
                            taxCode = "SK";
                            taxRate = 0.05;
                            break;
                        case "AB":
                            taxCode = "AB";
                            taxRate = 0.05;
                            break;
                        case "NV":
                            taxCode = "NV";
                            taxRate = 0.05;
                            break;
                        case "YK":
                            taxCode = "YK";
                            taxRate = 0.05;
                            break;
                        default:
                            taxCode = "N";
                            taxRate = 0;
                            break;
                    }
                }

                // generate JSON file for order row post
                string textJSON;
                if (productId != null)
                    textJSON = "{\"productId\":\"" + productId + "\",\"quantity\":{\"magnitude\":\"" + quantity + "\"},\"rowValue\":{\"taxCode\":\"" + taxCode + "\",\"rowNet\":{\"value\":\"" + Math.Round(netPrice, 4) + "\"},\"rowTax\":{\"value\":\"" + Math.Round(netPrice * taxRate, 4) + "\"}}}";
                else
                    textJSON = "{\"productName\":\"" + value.Description[skuIndex] + " " + sku + "\",\"quantity\":{\"magnitude\":\"" + quantity + "\"},\"rowValue\":{\"taxCode\":\"" + taxCode + "\",\"rowNet\":{\"value\":\"" + Math.Round(netPrice, 4) + "\"},\"rowTax\":{\"value\":\"" + Math.Round(netPrice * taxRate, 4) + "\"}}}";

                // turn request string into a byte stream
                byte[] postBytes = Encoding.UTF8.GetBytes(textJSON);

                // send request
                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(postBytes, 0, postBytes.Length);

                // get the response from server
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch
                {
                    HasError = true;
                    return null;        // 500 server internal error or 503 Server Unavailable
                }
                string result;
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    result = streamReader.ReadToEnd();

                result = substringMethod(result, ":", 1);
                return getTarget(result);   //return the order row ID
            }

            /* post reservation request to API and return the message*/
            public void postReservationRequest(string orderID, string orderRowID, BPvalues value, int skuIndex, int quantityIndex)
            {
                // set has error to false
                HasError = false;

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
                    textJSON = "{\"products\": [{\"productId\": \"" + productId + "\",\"salesOrderRowId\": \"" + orderRowID + "\",\"quantity\":\"" + value.Quantity[quantityIndex] + "\"}]}";
                else
                    return;

                // turn request string into a byte stream
                byte[] postBytes = Encoding.UTF8.GetBytes(textJSON);

                // send request
                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(postBytes, 0, postBytes.Length);

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
                        if ((int) response.StatusCode == 503)
                            HasError = true; // web server 503 server unavailable
                    }
                }
            }

            /* patch comment custom field request */
            public void patchComment(string orderId, string comment)
            {
                // set has error to false
                HasError = false;

                string uri = "https://ws-use.brightpearl.com/2.0.0/ashlin/order-service/order/" + orderId + "/custom-field";

                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "PATCH";
                request.ContentType = "application/json";
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);

                // format comment
                comment = comment.Replace('\n', ' ');
                comment = comment.Replace('\r', ' ');
                comment = comment.Replace('\t', ' ');

                // generate JSON field
                string textJSON = "[{\"op\":\"add\",\"path\":\"/PCF_ORDERCOM\",\"value\":\"" + comment.Trim() + "\"}]";

                // turn request string into a byte stream
                byte[] postBytes = Encoding.UTF8.GetBytes(textJSON);

                // send request
                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(postBytes, 0, postBytes.Length);

                // get the response from the server
                try      // might have server internal error, so do it in try and catch
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch    // HTTP response 500 or 503
                {
                    HasError = true; // cannot post comment, set has error to true
                }
            }
        }
    }

    /*
     * A class that calculate the price 
     */
    [Serializable]
    public class Price
    {
        // fields for storing discount matrix values
        private readonly double[] list = new double[11];

        /* constructor that initialize discount matrix list field */
        public Price()
        {
            SqlConnection connection = new SqlConnection(Properties.Settings.Default.Designcs);

            // [0] 1 net standard, [1] 6 net standard, [2] 24 net standard, [3] 50 net standard, [4] 100 net standard, [5] 250 net standard, [6] 500 net standard, [7] 1000 net standard, [8] 2500 net standard, [9] rush net
            SqlCommand command = new SqlCommand("SELECT [1_Net_Standard Delivery], [6_Net_Standard Delivery], [24_Net_Standard Delivery], [50_Net_Standard Delivery], [100_Net_Standard Delivery], [250_Net_Standard Delivery], [500_Net_Standard Delivery], [1000_Net_Standard Delivery], [2500_Net_Standard Delivery], [RUSH_Net_25_wks] "
                                              + "FROM ref_discount_matrix", connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            for (int i = 0; i <= 9; i++)
                list[i] = reader.GetDouble(i);
            reader.Close();

            // [10] multiplier
            command.CommandText = "SELECT [MSRP Multiplier] FROM ref_msrp_multiplier";
            reader = command.ExecuteReader();
            reader.Read();
            list[10] = reader.GetDouble(0);
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
                double runcharge = (msrp * 0.05) / 0.6;
                if (runcharge > 8)
                    runcharge = 8;
                else if (runcharge < 1)
                    runcharge = 1;

                if (rush)
                {
                    // the case if it is rush
                    if (quantity < 6)
                        return (msrp + runcharge) * list[0] * list[9];
                    if (quantity >= 6 && quantity < 24)
                        return (msrp + runcharge) * list[1] * list[9];
                    if (quantity >= 24 && quantity < 50)
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
                    if (quantity >= 6 && quantity < 24)
                        return (msrp + runcharge) * list[1];
                    if (quantity >= 24 && quantity < 50)
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
                    else if (quantity >= 6 && quantity < 24)
                        msrp *= list[1] * list[9];
                    else if (quantity >= 24 && quantity < 50)
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
                    else if (quantity >= 6 && quantity < 24)
                        msrp *= list[1];
                    else if (quantity >= 24 && quantity < 50)
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
        private static double getPrice(string sku)
        {
            double basePrice;

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.Designcs))
            {
                SqlCommand command = new SqlCommand("SELECT Base_Price FROM master_SKU_Attributes WHERE SKU_Ashlin = \'" + sku + "\';", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();

                basePrice = Convert.ToDouble(reader.GetValue(0));
            }

            return basePrice;
        }
    }
}