using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;

namespace AshlinCustomerQuery.SearchingClasses.Sage
{
    /* 
     * A class that verify the customer's information with sage number
     */
    public class SageSearch
    {
        // field for getting request from Sage
        private GetRequest get;

        /* constructor that initialize GetRequest class */
        public SageSearch()
        {
            using (SqlConnection authenticationConnection = new SqlConnection(Properties.Settings.Default.ASCMcs))
            {
                SqlCommand getAuthetication = new SqlCommand("SELECT Username, Field3_Value, Password FROM ASCM_Credentials WHERE Source = \'SageAPI\';", authenticationConnection);
                authenticationConnection.Open();
                SqlDataReader reader = getAuthetication.ExecuteReader();
                reader.Read();
                get = new GetRequest(reader.GetString(0), reader.GetString(1), reader.GetString(2));
            }
        }

        /* return the confidence level of the given customer's informaiton */
        public int getConfidenceLevel(SageValues value)
        {
            return get.getVerfication(value);
        }

        /*
         * A class that get JSON response
         */
        private class GetRequest
        {
            private HttpWebRequest request;
            private HttpWebResponse response;
            private string acctId;
            private string loginId;
            private string password;

            /* constructor that initialize api credential fields */
            public GetRequest(string acctId, string loginId, string password)
            {
                this.acctId = acctId;
                this.loginId = loginId;
                this.password = password;
            }

            /* post request to Sage and get the confidence level of the given information */
            public int getVerfication(SageValues value)
            {
                // generating json file string
                string textJSON = "{\"Request\":\"DistVerify\",\"APIVer\":\"0103\",\"Auth\":{\"AcctID\":" + acctId +
                                  ", \"LoginID\":\"" + loginId + "\", \"Password\":\"" + password + "\",}," +
                                  "\"CompanyInfo\":{";

                // adding information about the customer 
                if (value.FirstName != "")
                    textJSON += "\"ContactFirstName\":\"" + value.FirstName + "\",";
                if (value.LastName != "")
                    textJSON += "\"ContactLastName\":\"" + value.LastName + "\",";
                if (value.Company != "")
                    textJSON += "\"Company\":\"" + value.Company + "\",";
                if (value.Phone != "")
                    textJSON += "\"Phone\":\"" + value.Phone + "\",";
                if (value.Email != "")
                    textJSON += "\"Email\":\"" + value.Email + "\",";
                if (value.Sage != "")
                    textJSON += "\"SAGENum\":\"" + value.Sage + "\"";
                if (textJSON[textJSON.Length - 1] == ',')
                    textJSON.Remove(textJSON.Length - 1);
                textJSON += "}}";


                string uri = "https://www.promoplace.com/ws/ws.dll/SITK";

                // post request to uri
                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";

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

                int index = result.IndexOf("VerificationConfidenceLevel") + 29;
                int length = index;
                while (Char.IsNumber(result[length]))
                {
                    length++;
                }

                return Convert.ToInt32(result.Substring(index, length - index));
            }
        }
    }
}