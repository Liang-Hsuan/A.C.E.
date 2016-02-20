using System;
using System.Data.SqlClient;
using System.Net;
using AshlinCustomerQuery.SearchingClasses.Brightpearl;
using System.Text;
using System.IO;

namespace AshlinCustomerQuery.PostingClasses
{
    /* 
     * A class that post the new order and invoice to brightpearl
     */
    public class BPpost
    {
        // fields for web posting request
        private HttpWebRequest request;
        private HttpWebResponse response;
        private string appRef;
        private string appToken;

        // field for getting customer result
        private BPsearch search = new BPsearch();
        private BPvalues value;

        /* second constructor to initialize the web request of app reference and app token */
        public BPpost(BPvalues value)
        {
            // initialize credential
            using (SqlConnection authenticationConnection = new SqlConnection(Properties.Settings.Default.ASCMcs))
            {
                SqlCommand getAuthetication = new SqlCommand("SELECT Field3_Value, Field1_Value FROM ASCM_Credentials WHERE Source = \'Brightpearl Testing\';", authenticationConnection);
                authenticationConnection.Open();
                SqlDataReader reader = getAuthetication.ExecuteReader();
                reader.Read();
                appRef = reader.GetString(0);
                appToken = reader.GetString(1);
            }

            // initialize BPvalues class
            this.value = value;
        }

        /* post new address to API */
        public string postAddressRequest()
        {
            string uri = "https://ws-use.brightpearl.com/2.0.0/ashlintest/contact-service/postal-address";

            request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("brightpearl-app-ref", appRef);
            request.Headers.Add("brightpearl-account-token", appToken);

            // generate the JSON file for address post
            string textJSON = "{\"addressLine1\":\"" + value.Address1 + "\",\"addressLine2\":\"" + value.Address2 + "\",\"addressLine3\":\"" + value.City + "\",\"addressLine4\":\"" + value.Province + "\",\"postalCode\":\"" + value.PostalCode + "\",\"countryIsoCode\":\"" + value.Country + "\"}";

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
            string addressID = result.Substring(index, length - index);

            return addressID;  //return the addresss ID
        }

        /* post new customer to API */
        public string postContactRequest(string addressID)
        {
            string uri = "https://ws-use.brightpearl.com/2.0.0/ashlintest/contact-service/contact";

            request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("brightpearl-app-ref", appRef);
            request.Headers.Add("brightpearl-account-token", appToken);

            // generate JSON file for contact post
            string textJSON = "{\"firstName\":\"" + value.FirstName + "\",\"lastName\":\"" + value.LastName + "\",\"postAddressIds\":{\"DEF\":" + addressID + ",\"BIL\":" + addressID + ",\"DEL\":" + addressID + "},\"communication\":{\"telephones\":{\"PRI\":\"" + value.Phone + "\"}},\"relationshipToAccount\":{\"isSupplier\": false,\"isStaff\":false,\"isCustomer\":true},\"financialDetails\":{\"priceListId\": 5}}";

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
        public string postOrderRequest(string contactID)
        {
            string uri = "https://ws-use.brightpearl.com/2.0.0/ashlintest/order-service/order";

            request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("brightpearl-app-ref", appRef);
            request.Headers.Add("brightpearl-account-token", appToken);

            // generate JSON file for order post
            int shippingMethodId = 1;
            /*if (table.Rows[0][28].ToString() != "canada_post")   // check the carrier
            {
                shippingMethodId = 2;
            } */
            string textJSON = "{\"orderTypeCode\":\"SO\",\"placeOn\":\"" + DateTime.Now.ToString("yyyy-MM-dd h:mm tt").Replace(' ', 'T') + "+00:00\",\"orderStatus\":{\"orderStatusId\":2},\"delivery\":{\"deliveryDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd h:mm tt").Replace(' ', 'T') + "+00:00\",\"shippingMethodId\":" + shippingMethodId + "},\"currency\":{\"orderCurrencyCode\":\"CAD\"},\"parties\":{\"customer\":{\"contactId\":" + contactID + "}},\"assignment\":{\"current\":{\"channelId\":1}}}";


            // turn request string into a byte stream
            byte[] postBytes = Encoding.UTF8.GetBytes(textJSON);

            // send request
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(postBytes, 0, postBytes.Length);
            }

            // get the response from the server
            try    // might have server internal error, so do it in try and catch
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

            int index = result.IndexOf(":") + 1;
            int length = index;
            while (Char.IsNumber(result[length]))
            {
                length++;
            }
            string orderID = result.Substring(index, length - index);

            return orderID;  //return the order ID
        }

        /* post new order row to API */
        /*public string postOrderRowRequest(string orderID)
        {
            // get product id
            string productId = search.getProductId(value.SKU);

            string uri = "https://ws-use.brightpearl.com/2.0.0/ashlintest/order-service/order/" + orderID + "/row";
            request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("brightpearl-app-ref", appRef);
            request.Headers.Add("brightpearl-account-token", appToken);

            // generate JSON file for order row post
            string textJSON;
            if (productId != null)
            {
                textJSON = "{\"productId\":\"" + productId + "\",\"quantity\":{\"magnitude\":\"" + value.Quantity + "\"},\"rowValue\":{\"taxCode\":\"T\",\"rowNet\":{\"value\":\"" + table.Rows[0][7] + "\"},\"rowTax\":{\"value\":\"" + table.Rows[0][9] + "\"}}}";
            }
            else
            {
                textJSON = "{\"productName\":\"" + table.Rows[0][3] + "\",\"quantity\":{\"magnitude\":\"" + table.Rows[0][6] + "\"},\"rowValue\":{\"taxCode\":\"T\",\"rowNet\":{\"value\":\"" + table.Rows[0][7] + "\"},\"rowTax\":{\"value\":\"" + table.Rows[0][9] + "\"}}}";
            }


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
            string orderRowID = result.Substring(index, length - index);

            return orderRowID;  //return the order row ID
        } */

        /* post reservation to API */
      /*  public void postReservationRequest(string orderID, string orderRowID)
        {
            // get product id
            GetRequest get = new GetRequest(appRef, appToken);
            string productId = get.getProductId(table.Rows[0][2].ToString());

            string uri = "https://ws-use.brightpearl.com/2.0.0/ashlintest/warehouse-service/order/" + orderID + "/reservation/warehouse/2";
            request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("brightpearl-app-ref", appRef);
            request.Headers.Add("brightpearl-account-token", appToken);

            // generate JSON file for order row post
            string textJSON;
            if (productId != null)
            {
                textJSON = "{\"products\": [{\"productId\": \"" + productId + "\",\"salesOrderRowId\": \"" + orderRowID + "\",\"quantity\":\"" + table.Rows[0][6] + "\"}]}";
            }
            else
            {
                return;
            }

            // turn request string into a byte stream
            byte[] postBytes = Encoding.UTF8.GetBytes(textJSON);

            // send request
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(postBytes, 0, postBytes.Length);
            }

            // no need for response, but for some reason we need to get response so that it can uploaded successfully
            response = (HttpWebResponse)request.GetResponse();
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                streamReader.ReadToEnd();
            }
        } */
    }

    /* 
     * A class that retrieve the data from Design database
     */
    class DataRetrieve
    {
        public DataRetrieve()
        {
            
        }
    }
}