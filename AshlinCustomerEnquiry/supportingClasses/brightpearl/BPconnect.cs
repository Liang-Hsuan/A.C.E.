using System;
using System.Collections.Generic;
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
            using (var authCon = new System.Data.SqlClient.SqlConnection(Properties.Settings.Default.ASCMcs))
            {
                var getAuthetication = new System.Data.SqlClient.SqlCommand("SELECT Field3_Value, Field1_Value FROM ASCM_Credentials WHERE Source = 'Brightpearl'", authCon);
                authCon.Open();
                var reader = getAuthetication.ExecuteReader();
                reader.Read();
                string appRef = reader.GetString(0);
                string appToken = reader.GetString(1);

                // initializes request fields
                get = new GetRequest(appRef, appToken);
                post = new PostRequest(appRef, appToken);
            }
        }

        #region Get
        /* a method that accept name as parameters to return the customer detail */
        public BPvalues[] GetCustomerWithName(string firstName, string lastName, int choice)
        {
            // get all the id that correspond to the name given
            string[] idSet = get.GetCustomerIdWithName(firstName, lastName, choice);

            // the case if there is no result or there are too many result
            if (idSet == null)
                return null;

            // check if there is too many result
            return idSet[0] != "-1" ? get.GetCustomerDetail(idSet) : new[] { new BPvalues { FirstName = "-1" } };
        }

        /* a method that accept email / company parameters to return the customer detail -> [1] email only, [2] company only, [3] both */
        public BPvalues[] GetCustomerWithInfo(string email, string company, int choice)
        {
            // get all the id that correspond to the informaiton given
            string[] idSet = get.GetCustomerIdWithInfo(email, company, choice);

            // the case if no result
            if (idSet == null)
                return null;

            // check if there is too many result
            return idSet[0] != "-1" ? get.GetCustomerDetail(idSet) : new[] { new BPvalues {FirstName = "-1"} };
        }
        #endregion

        /* a method that post new order / quote to brightpearl */
        public void PostOrder(BPvalues value)
        {
            // get the contact id first
            string contactId = get.GetCustomerId(value.FirstName, value.LastName, value.PostalCode);

            // if customer does not exist -> create new contact
            if (contactId == null)
            {
                // post address with new customer
                string addressId = post.PostAddressRequest(value);
                contactId = post.PostContactRequest(addressId, value);
            }

            // post order
            string orderId = post.PostOrderRequest(contactId, value);
            while (post.HasError)
            {
                Thread.Sleep(5000);
                orderId = post.PostOrderRequest(contactId, value);
            }

            // the case if the order has logo -> add service item
            if (value.Logo)
            {
                value.Sku.Add("DIESETUP-SVC-SVC");
                value.Description.Add("Imprint Fee");
                value.Quantity.Add(1);
                value.BasePrice.Add(Price.GetPrice("DIESETUP-SVC-SVC"));
                value.PricingTier.Add(0);
                value.GiftBox.Add(false);
            }

            // post order row
            for (int i = 0; i < value.Sku.Count; i++)
            {
                // post order row
                post.PostOrderRowRequest(orderId, value, i);
                if (!post.HasError) continue;
                do
                {
                    Thread.Sleep(5000);
                    post.PostOrderRowRequest(orderId, value, i);
                } while (post.HasError);
            }

            // the case if the order has logo -> remove service item
            if (value.Logo)
            {
                int index = value.Sku.Count - 1;
                value.Sku.RemoveAt(index);
                value.Description.RemoveAt(index);
                value.Quantity.RemoveAt(index);
                value.BasePrice.RemoveAt(index);
                value.PricingTier.RemoveAt(index);
                value.GiftBox.RemoveAt(index);
            }

            // post comment
            if (value.Comment == "") return;
            post.PatchComment(orderId, value.Comment);
            while (post.HasError)
            {
                Thread.Sleep(5000);
                post.PatchComment(orderId, value.Comment);
            }
        }

        #region ID Return
        /* return the list of customer id */
        public string[] GetContactId(string firstName, string lastName)
        {
            return get.GetCustomerIdWithName(firstName, lastName, 3);
        }

        /* return the product id of the given sku */
        public string GetProductId(string sku)
        {
            return get.GetProductId(sku);
        }

        /* return all the staff members' id and name */
        public Dictionary<string, string> GetStaff()
        {
            return get.GetStaff();
        }
        #endregion

        #region Supporting Methods
        /* a method that substring the given string */
        private static string SubstringMethod(string original, string startingString, int additionIndex)
        {
            return original.Substring(original.IndexOf(startingString, StringComparison.Ordinal) + additionIndex);
        }

        /* a method that get the next target token */
        private static string GetTarget(string text)
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
        [Serializable]
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
            public string[] GetCustomerIdWithName(string firstName, string lastName, int choice)
            {
                // generate uri from user's choice
                string uri;
                switch (choice)
                {
                    case 1:
                        uri = "https://ws-use.brightpearl.com/public-api/ashlin/contact-service/contact-search?columns=contactId&firstName=" 
                             + firstName;
                        break;
                    case 2:
                        uri = "https://ws-use.brightpearl.com/public-api/ashlin/contact-service/contact-search?columns=contactId&lastName=" 
                             + lastName;
                        break;
                    case 3:
                        uri = "https://ws-use.brightpearl.com/public-api/ashlin/contact-service/contact-search?columns=contactId&firstName=" 
                             + firstName + "&lastName=" + lastName;
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
                    return new[] {"-1"};

                // start getting id
                string[] list = new string[number];
                for (int i = 0; i < number; i++)
                    list[i] = info["response"]["results"][i][0].ToString();

                return list;
            }

            /* get the customer id from the given email address and company name -> [1] email only, [2] company only, [3] both */
            public string[] GetCustomerIdWithInfo(string email, string company, int choice)
            {
                // generate uri from user's choice
                string uri;
                switch (choice)
                {
                    case 1:
                        uri = "https://ws-use.brightpearl.com/public-api/ashlin/contact-service/contact-search?columns=contactId&primaryEmail=" 
                             + email;
                        break;
                    case 2:
                        uri = "https://ws-use.brightpearl.com/public-api/ashlin/contact-service/contact-search?columns=contactId&companyName=" 
                             + company.Replace(" ", "%20");
                        break;
                    case 3:
                        uri = "https://ws-use.brightpearl.com/public-api/ashlin/contact-service/contact-search?columns=contactId&primaryEmail="
                             + email + "&companyName=" + company.Replace(" ", "%20");
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
                    return new[] { "-1" };

                // start getting id
                string[] list = new string[number];
                for (int i = 0; i < number; i++)
                    list[i] = info["response"]["results"][i][0].ToString();

                return list;
            }

            /* a method that return the only corresponding customer */
            public string GetCustomerId(string firstName, string lastName, string postalCode)
            {
                // get all the customer id that exist with the given first name and last name
                string[] list = GetCustomerIdWithName(firstName, lastName, 3);

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

                    try
                    {
                        // get postal code
                        address = info["response"][0]["postalAddresses"][address]["postalCode"];
                    }
                    catch
                    {
                        return null;
                    }

                    if (string.Equals(postalCode.Replace(" ", string.Empty), address.Replace(" ", string.Empty), StringComparison.CurrentCultureIgnoreCase))
                        return list[i];
                }

                return null;
            }

            /* a method that get the detail info about the customer */
            public BPvalues[] GetCustomerDetail(string[] customerId)
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

                        // the case contact is not a person
                        if (value.FirstName == "") continue;

                        #region Data Retrieve
                        // get address id to get postal code
                        string addressId = info["response"][j]["postAddressIds"]["DEF"].ToString();

                        // get postal code
                        try { value.Address1 = info["response"][j]["postalAddresses"][addressId]["addressLine1"]; } catch { /* ignore -> null case */ }
                        try { value.Address2 = info["response"][j]["postalAddresses"][addressId]["addressLine2"]; } catch { /* ignore -> null case */ }
                        try { value.City = info["response"][j]["postalAddresses"][addressId]["addressLine3"]; } catch { /* ignore -> null case */ }
                        try { value.Province = info["response"][j]["postalAddresses"][addressId]["addressLine4"]; } catch { /* ignore -> null case */ }
                        try { value.PostalCode = info["response"][j]["postalAddresses"][addressId]["postalCode"]; } catch { /* ignore -> null case */ }
                        try { value.Country = info["response"][j]["postalAddresses"][addressId]["countryIsoCode"]; } catch { /* ignore -> null case */ }
                        try { value.Email = info["response"][j]["communication"]["emails"]["PRI"]["email"]; } catch { /* ignore -> null case */ }
                        try { value.Phone = info["response"][j]["communication"]["telephones"]["PRI"]; } catch { /* ignore -> null case */ }
                        try { value.Company = info["response"][j]["organisation"]["name"]; } catch { /* ignore -> null case */ }
                        #endregion

                        valueList.Add(value);
                    }
                }

                return valueList.ToArray();
            }
            #endregion

            /* a method that return <contactId;email, firstName, lastName> for all the staff memebers */
            public Dictionary<string, string> GetStaff()
            {
                // uri for searching all staff members
                const string uri = "https://ws-use.brightpearl.com/2.0.0/ashlin/contact-service/contact-search?columns=contactId,primaryEmail,firstName,lastName&isStaff=true";

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

                // addint item to the dictionary
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (var item in info["response"]["results"])
                    dic.Add(item[0].ToString() + ';' + item[1], item[2] + ' ' + item[3]);

                return dic;
            }

            /* get the product id from the given sku */
            public string GetProductId(string sku)
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
        [Serializable]
        private class PostRequest
        {
            // fields for web request
            private HttpWebRequest request;
            private HttpWebResponse response;

            // fields for credentials
            private readonly string appRef;
            private readonly string appToken;

            // field for error indication
            public bool HasError { get; private set; }

            /* constructor to initialize the web request of app reference and app token */
            public PostRequest(string appRef, string appToken)
            {
                this.appRef = appRef;
                this.appToken = appToken;
            }

            /* post new address to API */
            public string PostAddressRequest(BPvalues address)
            {
                const string uri = "https://ws-use.brightpearl.com/2.0.0/ashlin/contact-service/postal-address";

                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);

                // get country ISO code
                string country = address.Country.ToUpper().Contains("CA") ? "CAN" : "USA";

                // generate the JSON file for address post
                string textJson = "{\"addressLine1\":\"" + address.Address1 + "\",\"addressLine2\":\"" + address.Address2 + "\",\"addressLine3\":\"" + address.City + "\",\"addressLine4\":\"" + address.Province + "\",\"postalCode\":\"" + address.PostalCode + "\",\"countryIsoCode\":\"" + country + "\"}";

                // turn request string into a byte stream
                byte[] postBytes = Encoding.UTF8.GetBytes(textJson);

                // send request
                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(postBytes, 0, postBytes.Length);

                // get the response from the server
                response = (HttpWebResponse)request.GetResponse();
                string result;
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    result = streamReader.ReadToEnd();

                result = SubstringMethod(result, ":", 1);
                return GetTarget(result);  //return the addresss ID
            }

            /* post new customer to API */
            public string PostContactRequest(string addressId, BPvalues value)
            {
                const string uri = "https://ws-use.brightpearl.com/2.0.0/ashlin/contact-service/contact";

                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);

                // generate JSON file for contact post
                string textJson = "{\"firstName\":\"" + value.FirstName + "\",\"lastName\":\"" + value.LastName + "\",\"postAddressIds\":{\"DEF\":" + addressId + ",\"BIL\":" + addressId + ",\"DEL\":" + addressId + "},\"organisation\":{\"name\":\"" + value.Company + "\"}," +
                                  "\"communication\":{\"emails\":{\"PRI\":{\"email\":\"" + value.Email + "\"}},\"telephones\":{\"PRI\":\"" + value.Phone + "\"}},\"contactStatus\":{\"current\":{\"contactStatusId\":9}},\"relationshipToAccount\":{\"isSupplier\": false,\"isStaff\":false,\"isCustomer\":true},\"financialDetails\":{\"priceListId\": 5}}";

                // turn request string into a byte stream
                byte[] postBytes = Encoding.UTF8.GetBytes(textJson);

                // send request
                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(postBytes, 0, postBytes.Length);

                // get the response from the server
                response = (HttpWebResponse)request.GetResponse();
                string result;
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    result = streamReader.ReadToEnd();

                result = SubstringMethod(result, ":", 1);
                return GetTarget(result);  //return the contact ID
            }

            /* post new order to API */
            public string PostOrderRequest(string contactId, BPvalues value)
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
                string[] currencyAndchannelId = value.Country.ToUpper().Contains("CA") ? new[]{ "CAD", "14" } : new[]{ "USD", "13" };

                // get price list id and reference depending on rush and logo
                int priceListId;
                string reference;
                if (value.Logo && value.Rush)
                {
                    priceListId = value.Country.ToUpper().Contains("CA") ? 8 : 16;
                    reference = "Rush Delivery with Logo";
                }
                else if (value.Logo && !value.Rush)
                {
                    priceListId = value.Country.ToUpper().Contains("CA") ? 5 : 13;
                    reference = "Standard Delivery with Logo";
                }
                else if (!value.Logo && value.Rush)
                {
                    priceListId = value.Country.ToUpper().Contains("CA") ? 7 : 14;
                    reference = "Rush Delivery with no Logo";
                }
                else
                {
                    priceListId = value.Country.ToUpper().Contains("CA") ? 6 : 15;
                    reference = "Standard Delivery with no Logo";
                }
                #endregion

                // generate JSON file for order post
                string textJson = "{\"orderTypeCode\":\"SO\",\"reference\":\"" + reference + "\",\"priceListId\":" + priceListId + ",\"placeOn\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(' ', 'T') + "+00:00\",\"orderStatus\":{\"orderStatusId\":1},\"delivery\":{\"deliveryDate\":\"" + value.DeliveryDate.ToString("yyyy-MM-dd") + "T00:00:00+00:00\"}" + 
                                  ",\"currency\":{\"orderCurrencyCode\":\"" + currencyAndchannelId[0] + "\"},\"parties\":{\"customer\":{\"contactId\":" + contactId + "}},\"assignment\":{\"current\":{\"channelId\":" + currencyAndchannelId[1] + ",\"staffOwnerContactId\":" + value.StaffId + "}}}";


                // turn request string into a byte stream
                byte[] postBytes = Encoding.UTF8.GetBytes(textJson);

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

                result = SubstringMethod(result, ":", 1);
                return GetTarget(result);  //return the order ID
            }

            /* post new order row to API */
            public string PostOrderRowRequest(string orderId, BPvalues value, int index)
            {
                // set has error to false
                HasError = false;

                // get product id
                GetRequest get = new GetRequest(appRef, appToken);
                string productId = get.GetProductId(value.Sku[index]);

                // fields for web request
                string uri = "https://ws-use.brightpearl.com/2.0.0/ashlin/order-service/order/" + orderId + "/row";
                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);

                // flags creation
                string sku = value.Sku[index];
                int quantity = value.Quantity[index];
                bool imprint = value.Logo;
                bool rush = value.Rush;
                double netPrice = Price.GetPrice(value.BasePrice[index], value.PricingTier[index], quantity, imprint, rush, !value.Country.ToUpper().Contains("CA")) * quantity;

                string taxCode;
                double taxRate;
                if (!value.Country.ToUpper().Contains("CA"))
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
                string textJson;
                if (productId != null)
                    textJson = "{\"productId\":\"" + productId + "\",\"productName\":\"" + value.Description[index] + " - " + (value.GiftBox[index] ? "Includes Gift Boxes" : "Basic packaging included : {gift boxes extra}") + 
                               "\",\"quantity\":{\"magnitude\":\"" + quantity + "\"},\"rowValue\":{\"taxCode\":\"" + taxCode + "\",\"rowNet\":{\"value\":\"" + Math.Round(netPrice, 4) + "\"},\"rowTax\":{\"value\":\"" + Math.Round(netPrice * taxRate, 4) + "\"}}}";
                else
                    textJson = "{\"productName\":\"" + sku + ' ' + value.Description[index] + " - " + (value.GiftBox[index] ? "Includes Gift Boxes" : "Basic packaging included : {gift boxes extra}") + "\",\"quantity\":{\"magnitude\":\"" + quantity + 
                               "\"},\"rowValue\":{\"taxCode\":\"" + taxCode + "\",\"rowNet\":{\"value\":\"" + Math.Round(netPrice, 4) + "\"},\"rowTax\":{\"value\":\"" + Math.Round(netPrice * taxRate, 4) + "\"}}}";

                // turn request string into a byte stream
                byte[] postBytes = Encoding.UTF8.GetBytes(textJson);

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

                result = SubstringMethod(result, ":", 1);
                return GetTarget(result);   //return the order row ID
            }

            /* post reservation request to API (deprecated) */
            public void PostReservationRequest(string orderId, string orderRowId, BPvalues value, int index)
            {
                // set has error to false
                HasError = false;

                // get product id
                GetRequest get = new GetRequest(appRef, appToken);
                string productId = get.GetProductId(value.Sku[index]);

                string uri = "https://ws-use.brightpearl.com/2.0.0/ashlin/warehouse-service/order/" + orderId + "/reservation/warehouse/2";
                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("brightpearl-app-ref", appRef);
                request.Headers.Add("brightpearl-account-token", appToken);

                // generate JSON file for order row post
                string textJson;
                if (productId != null)
                    textJson = "{\"products\": [{\"productId\": \"" + productId + "\",\"salesOrderRowId\": \"" + orderRowId + "\",\"quantity\":\"" + value.Quantity[index] + "\"}]}";
                else
                    return;

                // turn request string into a byte stream
                byte[] postBytes = Encoding.UTF8.GetBytes(textJson);

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
            public void PatchComment(string orderId, string comment)
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
                string textJson = "[{\"op\":\"add\",\"path\":\"/PCF_ORDERCOM\",\"value\":\"" + comment.Trim() + "\"}]";

                // turn request string into a byte stream
                byte[] postBytes = Encoding.UTF8.GetBytes(textJson);

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
}