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
        private static BPvalues value;

        // field for getting product information
        private DataRetrieve retriever = new DataRetrieve();

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
            BPpost.value = value;
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
                textJSON = "{\"productId\":\"" + productId + "\",\"quantity\":{\"magnitude\":\"" + value.Quantity + "\"},\"rowValue\":{\"taxCode\":\"T\",\"rowNet\":{\"value\":\"" + retriever.getPrice(value.SKU, value.Quantity) + "\"},\"rowTax\":{\"value\":\"" + table.Rows[0][9] + "\"}}}";
            }
            else
            {
                textJSON = "{\"productName\":\"" + retriever.getProductName(value.SKU) + "\",\"quantity\":{\"magnitude\":\"" + value.Quantity + "\"},\"rowValue\":{\"taxCode\":\"T\",\"rowNet\":{\"value\":\"" + retriever.getPrice(value.SKU, value.Quantity) + "\"},\"rowTax\":{\"value\":\"" + table.Rows[0][9] + "\"}}}";
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

        /* 
         * A inner class that retrieve the data from Design database
         */
        class DataRetrieve
        {
            // field that connection to database to retrieve price of the product
            private SqlConnection connection;

            /* constructor that initialize connection field */
            public DataRetrieve()
            {
                connection = new SqlConnection(Properties.Settings.Default.Designcs);
            }

            /* a method that return the price of given sku and the value field quantity for calculating discount */
            public double getPrice(string sku, int quantity)
            {
                // retrieve the price, imprint flag and number of component of the product
                SqlCommand command = new SqlCommand("SELECT Base_Price, Imprintable, Components FROM master_SKU_Attributes sku " +  
                                                    "INNER JOIN master_Design_Attributes design ON design.Design_Service_Code = sku.Design_Service_Code " +
                                                    "WHERE SKU_Ashlin = \'" + sku + "\';", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                double price = Convert.ToDouble(reader.GetValue(0));
                bool imprint = reader.GetBoolean(1);
                int components = Convert.ToInt32(reader.GetValue(2));
                connection.Close();

                // generate all necessary fields for price calculation
                double[] discount = getDiscount();
                double msrp = price * discount[9];
                if (imprint)
                {
                    double runCharge = Math.Round(msrp * 0.05) / 0.6 + components - 1;
                    if (runCharge > 8)
                        runCharge = 8;
                    else if (runCharge < 1)
                        runCharge = 1;
                    msrp += runCharge;
                }

                // price calculation
                if (quantity >= 1 && quantity < 6)
                    return msrp*discount[0];
                if (quantity > 6 && quantity <= 24)
                    return msrp*discount[1];
                if (quantity > 24 && quantity <= 50)
                    return msrp*discount[2];
                if (quantity > 50 && quantity <= 100)
                    return msrp*discount[3];
                if (quantity > 100 && quantity <= 250)
                    return msrp*discount[4];
                if (quantity > 250 && quantity <= 500)
                    return msrp*discount[5];
                if (quantity > 500 && quantity <= 1000)
                    return msrp*discount[6];
                if (quantity > 1000 && quantity <= 2500)
                    return msrp*discount[7];

                return msrp * discount[8];
            }

            /* a method that get the product name of the given sku */
            public string getProductName(string sku)
            {
                // get the design code of the sku | and the field will be also the product name for return 
                string design = sku.Substring(0, sku.IndexOf('-'));


                using (connection)
                {
                    SqlCommand command = new SqlCommand("SELECT Short_Description FROM master_Design_Attributes WHERE Design_Service_Code = \'" + design + "\';", connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();

                    design = reader.GetString(0);
                }

                return design;
            }

            /* a private supporting method that return the discount matrix */
            private double[] getDiscount()
            {
                double[] list = new double[10];

                //  [0] 1 c net standard, [1] 6 c net standard, [2] 24 c net standard, [3] 50 c net standard, [4] 100 net standard, [5] 250 net standard, [6] 500 net standard, [7] 1000 net standard, [8] 2500 net standard
                SqlCommand command = new SqlCommand("SELECT [1_Net_Standard Delivery], [6_Net_Standard Delivery], [24_Net_Standard Delivery], [50_Net_Standard Delivery], [100_Net_Standard Delivery], [250_Net_Standard Delivery], [500_Net_Standard Delivery], [1000_Net_Standard Delivery], [2500_Net_Standard Delivery] "
                                                  + "FROM ref_discount_matrix;", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                for (int i = 0; i <= 8; i++)
                {
                    list[i] = reader.GetDouble(i);
                }
                reader.Close();
                // [9] multiplier
                command = new SqlCommand("SELECT [MSRP Multiplier] FROM ref_msrp_multiplier;", connection);
                reader = command.ExecuteReader();
                reader.Read();
                list[9] = reader.GetDouble(0);
                connection.Close();

                return list;
            }
        }
    }
}