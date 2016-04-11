using AshlinCustomerEnquiry.supportingClasses.brightpearl;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace AshlinCustomerEnquiry.supportingClasses.asi
{
    /*
     * A class that connect to ASI and retrieve company informaiton
     */
    [Serializable]
    public class Asi
    {
        // fields for web request
        private WebRequest request;
        private HttpWebResponse response;

        // field for storing auth token
        private readonly string token;

        /* constructor that retrieve the auth token in order to do the request after */
        public Asi()
        {
            // field for login credentials
            string asi;
            string username;
            string password;

            // get credentials first from database
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.ASCMcs))
            {
                // [0] username, [1] password, [2] ASI number
                SqlCommand command = new SqlCommand("SELECT Username, Password, Field1_Value FROM ASCM_Credentials WHERE Source = 'ASI API';", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();

                username = reader.GetString(0);
                password = reader.GetString(1);
                asi = reader.GetString(2);
            }

            // create login uri
            const string loginUri = "http://asiservice.asicentral.com/credit/v1/login";

            // posting request to get token
            request = (HttpWebRequest)WebRequest.Create(loginUri);
            request.Method = "POST";
            request.ContentType = "application/json";

            // generate JSON file 
            string textJson = "{\"asi\":\"" + asi + "\",\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";

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

            // set token value
            token = result.Substring(1, result.Length - 2);
        }

        /* a method that return company info from the given asi number */
        public BPvalues GetCompanyInfo(string asi)
        {
            // uri for getting company information
            string uri = "http://asiservice.asicentral.com/credit/v1/creditsummary/?asiNumber=" + asi;

            // post request to uri
            request = WebRequest.Create(uri);
            request.Headers.Add("AuthToken", token);
            request.Method = "GET";

            try
            {
                // get the response from the server
                response = (HttpWebResponse)request.GetResponse();
            }
            catch
            {
                // the case if the company does not exist (422 unprocessable entity0 -> return nothing
                return null;
            }

            // read all the text from JSON response
            string textJson;
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                textJson = streamReader.ReadToEnd();

            // deserialize json to key value
            var info = new JavaScriptSerializer().Deserialize<Dictionary<string, dynamic>>(textJson);

            #region Data Retrieve
            // start getting data
            string name = info["CompanyDetails"]["Name"];
            string phone = info["CompanyDetails"]["Phones"][0]["PhoneNumber"];
            string email = info["CompanyDetails"]["Emails"][0]["Address"];
            string address1 = info["CompanyDetails"]["Addresses"][0]["AddressLine1"];
            string address2 = info["CompanyDetails"]["Addresses"][0]["AddressLine2"];
            string city = info["CompanyDetails"]["Addresses"][0]["City"];
            string province = info["CompanyDetails"]["Addresses"][0]["State"];
            string postalCode = info["CompanyDetails"]["Addresses"][0]["ZipCode"];
            string country = info["CompanyDetails"]["Addresses"][0]["CountryCode"];
            #endregion

            return new BPvalues("", "", name, phone, email, address1, address2, city, province, postalCode, country, null, null, null, null, null, true, false, null, DateTime.Today);
        }
    }
}