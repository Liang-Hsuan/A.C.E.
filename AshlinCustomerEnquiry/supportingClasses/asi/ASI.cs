using AshlinCustomerEnquiry.supportingClasses.brightpearl;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;

namespace AshlinCustomerEnquiry.supportingClasses.asi
{
    /*
     * A class that connect to ASI and retrieve company informaiton
     */
    [Serializable]
    public class ASI
    {
        // fields for web request
        private WebRequest request;
        private HttpWebResponse response;

        // field for storing auth token
        private readonly string token;

        /* constructor that retrieve the auth token in order to do the request after */
        public ASI()
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
            string textJSON = "{\"asi\":\"" + asi + "\",\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";

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

            // set token value
            result = substringMethod(result, "\"", 1);
            token = getTarget(result);
        }

        /* a method that return company info from the given asi number */
        public BPvalues getCompanyInfo(string asi)
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
            string textJSON;
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                textJSON = streamReader.ReadToEnd();

            #region Data Retrieve
            // start getting data
            // company
            textJSON = substringMethod(textJSON, "\"Name\"", 8);
            string company = getTarget(textJSON);

            // email
            textJSON = substringMethod(textJSON, "\"Address\"", 11);
            string email = getTarget(textJSON);

            // phone
            textJSON = substringMethod(textJSON, "\"PhoneNumber\"", 15);
            string phone = getTarget(textJSON);
            if (phone == "null" || phone == "ull")
                phone = "";

            // address1
            textJSON = substringMethod(textJSON, "\"AddressLine1\"", 16);
            string address1 = getTarget(textJSON);

            // address 2
            textJSON = substringMethod(textJSON, "\"AddressLine2\"", 16);
            string address2 = getTarget(textJSON);
            if (address2 == "null" || address2 == "ull")
                address2 = "";

            // city
            textJSON = substringMethod(textJSON, "\"City\"", 8);
            string city = getTarget(textJSON);

            // province
            textJSON = substringMethod(textJSON, "\"State\"", 9);
            string province = getTarget(textJSON);

            // postal code 
            textJSON = substringMethod(textJSON, "\"ZipCode\"", 11);
            string postalCode = getTarget(textJSON);

            // country
            textJSON = substringMethod(textJSON, "\"CountryCode\"", 15);
            string country = getTarget(textJSON);
            if (country == "null" || country == "ull")
                country = "";
            #endregion

            return new BPvalues("", "", company, phone, email, address1, address2, city, province, postalCode, country, new string[0], new string[0], new int[0], true, false, null);
        }

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
    }
}