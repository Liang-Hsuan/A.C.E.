using AshlinCustomerEnquiry.supportingClasses.asi;
using AshlinCustomerEnquiry.supportingClasses.brightpearl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI.WebControls;

namespace AshlinCustomerEnquiry
{
    public partial class Main : System.Web.UI.Page
    {
        // field for storing customer data
        private BPvalues[] value;

        // fields for ASI and Brightpearl
        private Asi asi;
        private BPconnect bp;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialization();

                // initialize ASI object and store it
                if (Session["ASI"] == null)
                    Session["ASI"] = new Asi();

                // initialize BPconnect object and store it
                if (Session["BPconnect"] == null)
                    Session["BPconnect"] = new BPconnect();     

                welcomePopup.Show();
            }
            else
            {
                // after a post back return to the position as before
                Page.MaintainScrollPositionOnPostBack = true;

                // restore value
                if (ViewState["Value"] != null)
                    value = (BPvalues[]) ViewState["Value"];
                asi = (Asi) Session["ASI"];
                bp = (BPconnect) Session["BPconnect"];
            }
        }

        #region Calendar Event
        /* the event for date event button click that change the visibility of the calendar */
        protected void dateEventButton_Click(object sender, EventArgs e)
        {
            calendar.Visible = !calendar.Visible;
        }

        /* the event for calendar date selection that show the date time on the textbox */
        protected void calendar_SelectionChanged(object sender, EventArgs e)
        {
            dateDeliveryTextbox.Text = calendar.SelectedDate.ToString("yyyy-MM-dd");
        }

        /* set the canlender can only select the day after today */
        protected void calendar_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime date = DateTime.Today;
            if (e.Day.Date > date) return;
            e.Day.IsSelectable = false;
            e.Cell.ForeColor = Color.Gray;
        }
        #endregion

        #region Radio Buttons
        /* the event for auto and manual radio button clicks */
        protected void disableRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!disableRadioButton.Checked) return;
            firstNameTextbox.Enabled = false;
            lastNameTextbox.Enabled = false;
            phoneTextbox.Enabled = false;
            emailTextbox.Enabled = false;
            companyTextbox.Enabled = false;
            address1Textbox.Enabled = false;
            address2Textbox.Enabled = false;
            cityTextbox.Enabled = false;
            provinceTextbox.Enabled = false;
            postalCodeTextbox.Enabled = false;
            countryTextbox.Enabled = false;
        }
        protected void enableRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!enableRadioButton.Checked) return;
            firstNameTextbox.Enabled = true;
            lastNameTextbox.Enabled = true;
            phoneTextbox.Enabled = true;
            emailTextbox.Enabled = true;
            companyTextbox.Enabled = true;
            address1Textbox.Enabled = true;
            address2Textbox.Enabled = true;
            cityTextbox.Enabled = true;
            provinceTextbox.Enabled = true;
            postalCodeTextbox.Enabled = true;
            countryTextbox.Enabled = true;
        }
        #endregion

        #region Welcome Panel
        /* the event for yes button clicks that show the look up wizard -> asi panel */
        protected void yesButton_Click(object sender, EventArgs e)
        {
            welcomePopup.Hide();
            asiPopup.Show();
        }

        /* the skip button event that jump to search panel directly */
        protected void welcomeSkipButton_Click(object sender, EventArgs e)
        {
            welcomePopup.Hide();
            searchPopup.Show();
        }
        #endregion

        #region ASI Panel
        /* asi next button click that search the company info from the user input asi number */
        protected void asiNextButton_Click(object sender, EventArgs e)
        {
            // reset textbox color
            asiTextbox.BackColor = Color.White;

            if (asiTextbox.Text != "")
            {
                // get company information from the asi number that user entered
                BPvalues asiValue = asi.getCompanyInfo(asiTextbox.Text);

                // the case if the asi number does not exist -> return
                if (asiValue == null)
                {
                    asiTextbox.BackColor = Color.Red;
                    asiPopup.Show();

                    return;
                }

                // search result by company name
                value = bp.GetCustomerWithInfo(null, asiValue.Company, 2);

                #region Error Check
                // the case if there is no result or there are too many result
                if (value == null)
                {
                    // inform user there is not existing cusomter on the database from the company
                    const string script = "<script>alert(\"There is no customer exist from this company. Please enter information manually\");</script>";
                    Page.ClientScript.RegisterStartupScript(GetType(), "Scripts", script);

                    // show company info on the form
                    phoneTextbox.Text = asiValue.Phone;
                    emailTextbox.Text = asiValue.Email;
                    companyTextbox.Text = asiValue.Company;
                    address1Textbox.Text = asiValue.Address1;
                    address2Textbox.Text = asiValue.Address2;
                    cityTextbox.Text = asiValue.City;
                    provinceTextbox.Text = asiValue.Province;
                    postalCodeTextbox.Text = asiValue.PostalCode;
                    countryTextbox.Text = asiValue.Country;

                    firstNameTextbox.Text = "";
                    lastNameTextbox.Text = "";

                    return;
                }

                if (value[0].FirstName == "-1")
                {
                    tooManyResultLabel.Visible = true;
                    searchPopup.Show();

                    return;
                }
                #endregion

                // sort array by first name
                value = value.OrderBy(s => s.FirstName).ToArray();

                // show all the result 
                listbox.Items.Clear();
                foreach (BPvalues result in value)
                {
                    ListItem item = new ListItem(result.FirstName + " " + result.LastName);
                    listbox.Items.Add(item);
                }
                ShowResult(value[0]);

                // show result panel
                asiPopup.Hide();
                resultPopup.Show();

                // save viewstate
                ViewState["Value"] = value;
            }
            else
            {
                // the case if user did not put anything on the textbox -> signal them
                asiTextbox.BackColor = Color.Red;
                asiPopup.Show();
            }
        }

        /* the event for asi skip button click that jump to search panel */
        protected void asiSkipButton_Click(object sender, EventArgs e)
        {
            asiPopup.Hide();
            searchPopup.Show();
        }
        #endregion

        #region Search Panel
        /* search button click that search the customer from brightpearl database from the given information */
        protected void searchButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            // set warning invisible
            tooManyResultLabel.Visible = false;

            // get data from textboxes
            string firstName = searchFirstNameTextbox.Text;
            string lastName = searchLastNameTextbox.Text;
            string company = searchCompanyTextbox.Text;
            string email = searchEmailTextbox.Text;

            #region Error Checking
            // the case if user haven't provide any thing about customer
            if (firstName.Equals("") && lastName.Equals("") && company.Equals("") && email.Equals(""))
            {
                const string script = "<script>alert(\"Please provide some information about the customer\");</script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", script);
                searchPopup.Show();

                return;
            }
            #endregion

            #region Searching and Adding
            // down to business
            BPconnect search = new BPconnect();

            // supporting local field -> [0] name, [1] contact info
            BPvalues[][] list = new BPvalues[2][];

            // search with name
            if (firstName != "" && lastName != "")      // the case both firstname and lastname are supplied
                list[0] = search.GetCustomerWithName(firstName, lastName, 3);
            else if (firstName != "" && lastName == "") // the case only firstname is supplied
                list[0] = search.GetCustomerWithName(firstName, null, 1);
            else if (firstName == "" && lastName != "") // the case only lastname is supplied
                list[0] = search.GetCustomerWithName(null, lastName, 2);
            else
                list[0] = null;

            // search with contact info
            if (company != "" && email != "")           // the case both company and email are supplied
                list[1] = search.GetCustomerWithInfo(email, company, 3);
            else if (company != "" && email == "")      // the case only company is supplied
                list[1] = search.GetCustomerWithInfo(null, company, 2);
            else if (company == "" && email != "")      // the case only email is supplied
                list[1] = search.GetCustomerWithInfo(email, null, 1);
            else
                list[1] = null;

            // allocating data
            if (list[0] != null & list[1] != null)
                value = list[0].Length < list[1].Length ? list[0] : list[1];
            else if (list[0] == null)
                value = list[1];
            else if (list[1] == null)
                value = list[0];
            #endregion

            // the case if there is no result or there are too many result
            if (value == null)
            {
                const string script = "<script>alert(\"There is no result of this customer. Please enter information manually\");</script>";
                Page.ClientScript.RegisterStartupScript(GetType(), "Scripts", script);
                searchPopup.Show();

                return;
            }
            if (value[0].FirstName == "-1")
            {
                tooManyResultLabel.Visible = true;
                searchPopup.Show();

                return;
            }

            // sort array by first name
            value = value.OrderBy(s => s.FirstName).ToArray();

            // show all the result 
            listbox.Items.Clear();
            foreach (BPvalues result in value)
            {
                ListItem item = new ListItem(result.FirstName + ' ' + result.LastName);
                listbox.Items.Add(item);
            }
            ShowResult(value[0]);

            // show result panel
            searchPopup.Hide();
            resultPopup.Show();

            // save viewstate
            ViewState["Value"] = value;
        }

        /* back button event that goes back to asi panel */
        protected void searchBackButton_Click(object sender, EventArgs e)
        {
            searchPopup.Hide();
            asiPopup.Show();
        }
        #endregion

        #region Result Panel
        /* when selected index is changed show the corresponding info of the selected name */
        protected void listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowResult(value[listbox.SelectedIndex]);
            resultPopup.Show();
        }

        /* the event for select button click that show the result of the selected customer to the form */
        protected void resultSelectButton_Click(object sender, EventArgs e)
        {
            int i = listbox.SelectedIndex;

            if (i >= 0)
            {
                firstNameTextbox.Text = value[i].FirstName;
                lastNameTextbox.Text = value[i].LastName;
                phoneTextbox.Text = value[i].Phone;
                emailTextbox.Text = value[i].Email;
                companyTextbox.Text = value[i].Company;
                address1Textbox.Text = value[i].Address1;
                address2Textbox.Text = value[i].Address2;
                cityTextbox.Text = value[i].City;
                provinceTextbox.Text = value[i].Province;
                postalCodeTextbox.Text = value[i].PostalCode;
                countryTextbox.Text = value[i].Country;
            }
            else
                resultPopup.Show();
        }

        /* the event for back button click that goes back to search panel */
        protected void resultBackButton_Click(object sender, EventArgs e)
        {
            resultPopup.Hide();
            searchPopup.Show();
        }
        #endregion

        #region Update Panel
        /* update link button clicks that show the update panel */
        protected void updateLinkButton_Click(object sender, EventArgs e)
        {
            // the case if the user first time click quote button -> they need to log in
            if (Session["HasLogged"] == null)
            {
                usernameTextbox.BackColor = Color.White;
                passwordTextbox.BackColor = Color.White;

                loginPopup.Show();

                return;
            }

            // set textboxes back color to normal
            newUsernameTextbox.BackColor = Color.White;
            newPasswordTextbox.BackColor = Color.White;
            enterAgainTextbox.BackColor = Color.White;

            // show udpate panel for updating credentials
            updatePopup.Show();
        }

        /* update button clicks that update the credentials login for the webstie */
        protected void updateButton_Click(object sender, EventArgs e)
        {
            // get user input
            string username = newUsernameTextbox.Text.Trim();
            string[] password = { newPasswordTextbox.Text, enterAgainTextbox.Text };

            #region Error Check
            // username check
            if (username == "")
            {
                newUsernameTextbox.BackColor = Color.Red;
                updatePopup.Show();
                return;
            }
            newUsernameTextbox.BackColor = Color.White;

            // password check
            if (password[0] == "")
            {
                newPasswordTextbox.BackColor = Color.Red;
                updatePopup.Show();
                return;
            }
            newPasswordTextbox.BackColor = Color.White;

            // password confirm check
            if (password[1] == "")
            {
                enterAgainTextbox.BackColor = Color.Red;
                updatePopup.Show();
                return;
            }
            enterAgainTextbox.BackColor = Color.White;

            // overall password check
            if (password[0] != password[1])
            {
                newPasswordTextbox.BackColor = Color.Red;
                enterAgainTextbox.BackColor = Color.Red;
                updatePopup.Show();
                return;
            }
            #endregion

            // remove login cookies
            Response.Cookies["Login"].Expires = DateTime.Now.AddDays(-1);

            // start updating username and password
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.ASCMcs))
            {
                SqlCommand command = new SqlCommand("UPDATE ASCM_Credentials SET Username = \'" + username + "\', Password = \'" + password[0] + "\' WHERE Source = 'Ashlin Customer Enquiry';", connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        #endregion

        /* add button clicks that add new item to the grid view */
        protected void addButton_Click(object sender, EventArgs e)
        {
            // get table first
            DataTable table = (DataTable)ViewState["DataTable"];

            // get the list of quantity
            List<int> quantityList = (from ListItem item in quantityCheckboxList.Items where item.Selected select Convert.ToInt32(item.Value)).ToList();

            // error check
            if (skuDropdownlist.SelectedIndex < 1 || quantityList.Count < 1)
            {
                const string script = "<script>alert(\"Please select both SKU and the quantity in order to add the item\");</script>";
                Page.ClientScript.RegisterStartupScript(GetType(), "Scripts", script);
                return;
            }

            // adding each quantity to the table for the sku
            foreach (int quantity in quantityList)
            {
                DataRow row = table.NewRow();
                row[0] = skuDropdownlist.SelectedItem.ToString();                       // sku

                // get other values
                string skuValue = skuDropdownlist.SelectedValue;

                row[1] = skuValue.Substring(0, skuValue.IndexOf(';'));                  // short description
                skuValue = skuValue.Substring(skuValue.IndexOf(';') + 1);
                row[2] = bool.Parse(skuValue.Substring(0, skuValue.IndexOf(';')));      // gift box
                row[3] = quantity;
                row[4] = double.Parse(skuValue.Substring(skuValue.IndexOf(';') + 1));   // base price

                table.Rows.Add(row);
            }

            // bind the data source
            gridview.DataSource = table;
            gridview.DataBind();
            ViewState["DataTable"] = table;
        }

        #region Grid View
        /* event for row editing in grid view control */
        protected void gridview_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridview.EditIndex = e.NewEditIndex;
            gridview.DataSource = (DataTable)ViewState["DataTable"];
            gridview.DataBind();
        }

        /* evnet for row canceling in grid view control */
        protected void gridview_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gridview.EditIndex = -1;
            gridview.DataSource = (DataTable)ViewState["DataTable"];
            gridview.DataBind();
        }

        /* delete a row in grid view control */
        protected void gridview_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // remove the row from the table
            DataTable table = (DataTable)ViewState["DataTable"];
            table.Rows.Remove(table.Rows[e.RowIndex]);

            // bind the data source
            gridview.EditIndex = -1;
            gridview.DataSource = table;
            gridview.DataBind();
            ViewState["DataTable"] = table;
        }

        /* update quantity in a row */
        protected void gridview_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // get the new quantity of the item
            int qty = int.Parse(((TextBox)gridview.Rows[e.RowIndex].Cells[4].Controls[0]).Text);

            // update the quantity to the table
            DataTable table = (DataTable)ViewState["DataTable"];
            table.Rows[e.RowIndex][3] = qty;

            // bind the data source
            gridview.EditIndex = -1;
            gridview.DataSource = table;
            gridview.DataBind();
            ViewState["DataTable"] = table;
        }
        #endregion

        /* the event for login button in the login board */
        protected void loginButton_Click(object sender, EventArgs e)
        {
             // check if the user put the right username and password
            if (usernameTextbox.Text.Trim().Equals((string)Application["USERNAME"]) && passwordTextbox.Text.Equals((string)Application["PASSWORD"]))
            {
                // correct credential case -> set HasLogged to true and login cookie
                Session["HasLogged"] = true;
                HttpCookie cookie = new HttpCookie("Login")
                {
                    Value = "Success",
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Add(cookie);
            }
            else
            {
                // wrong credential case -> show the login borad again and signal them wrong
                loginPopup.Show();
                usernameTextbox.BackColor = Color.Red;
                passwordTextbox.BackColor = Color.Red;
            }
        }

        /* the event for quote button clicks that will call login panel if the user has not logged in and create quote */
        protected void quoteButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            // set label to visible
            newQuoteLabel.Visible = false;

            // the case if the user first time click quote button -> they need to log in
            if (Session["HasLogged"] == null)
            {
                usernameTextbox.BackColor = Color.White;
                passwordTextbox.BackColor = Color.White;

                loginPopup.Show();

                return;
            }

            #region Error Checking
            // get table
            DataTable table = (DataTable)ViewState["DataTable"];

            if (firstNameTextbox.Text == "" || lastNameTextbox.Text == "" || address1Textbox.Text == "" || cityTextbox.Text == "" || dateDeliveryTextbox.Text == "" ||
                provinceTextbox.Text == "" || postalCodeTextbox.Text == "" || countryTextbox.Text == "" || table.Rows.Count < 1)
            {
                const string script = "<script>alert(\"Please provide information on all the necessary fields (*)\");</script>";
                Page.ClientScript.RegisterStartupScript(GetType(), "Scripts", script);
                return;
            }
            #endregion

            // local fields for BPvalue generation
            List<string> skuList = new List<string>();
            List<string> descriptionList = new List<string>();
            List<int> qtyList = new List<int>();
            List<double> basePriceList = new List<double>();

            #region Email 
            // get the order detail
            string orderDetail = "Customer Information:\n\r" +
                                  "Name: " + firstNameTextbox.Text + " " + lastNameTextbox.Text + "\nPhone: " + phoneTextbox.Text + "\nEmail: " + emailTextbox.Text + "\nCompany: " + companyTextbox.Text
                                + "\nAddress1: " + address1Textbox.Text + "\nAddress2: " + address2Textbox.Text + "\nCity: " + cityTextbox.Text + "\nProvince / State: " + provinceTextbox.Text +
                                  "\nPostal Code: " + postalCodeTextbox.Text + "\nCountry: " + countryTextbox.Text + "\n\n\r" +
                                  "Order Detail:\n\r" +
                                  "Rush Order: ";
            if (rushCheckboxList.SelectedIndex == 0)
                orderDetail += "Yes\n";
            else
                orderDetail += "No\n";
            orderDetail += "With Logo: ";
            if (logoCheckboxList.SelectedIndex == 0)
                orderDetail += "Yes";
            else
                orderDetail += "No";
            for (int i = 0; i < table.Rows.Count; i++)
            {
                orderDetail += "\n\nItem " + (i + 1) + ": " + table.Rows[i][0] + "\nShort Description: " + table.Rows[i][1] + "\nGift Box: " + table.Rows[i][2] + "\nQuantity: " + table.Rows[i][3];

                // adding item to the lists
                skuList.Add(table.Rows[i][0].ToString());
                descriptionList.Add(table.Rows[i][1].ToString());
                qtyList.Add(Convert.ToInt32(table.Rows[i][3]));
                basePriceList.Add(Convert.ToDouble(table.Rows[i][4]));
            }
            orderDetail += "\n\nDelivery Date: " + dateDeliveryTextbox.Text + "\n\nAdditional Info:\n" + additionalInfoTextbox.Text;

            // send message
            MailMessage mail = new MailMessage();
            SmtpClient client = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("intern1002@ashlinbpg.com");
            mail.To.Add("intern1002@ashlinbpg.com");
            mail.Subject = "NEW ORDER QUOTE";
            mail.Body = orderDetail;

            client.Port = 587;
            client.Credentials = new NetworkCredential("intern1002@ashlinbpg.com", "AshlinIntern2");
            client.EnableSsl = true;
            client.Send(mail);
            #endregion

            #region Brightpearl
            // asssign boolean values
            bool rush; bool logo;
            if (rushCheckboxList.SelectedIndex != 0 && rushCheckboxList.SelectedIndex != 1)
                rush = false;
            else
                rush = Convert.ToBoolean(rushCheckboxList.SelectedValue);
            if (logoCheckboxList.SelectedIndex != 0 && logoCheckboxList.SelectedIndex != 1)
                logo = true;
            else
                logo = Convert.ToBoolean(logoCheckboxList.SelectedValue);

            // declare BPvalues object
            BPvalues bpValue = new BPvalues(firstNameTextbox.Text, lastNameTextbox.Text, companyTextbox.Text, phoneTextbox.Text, emailTextbox.Text, address1Textbox.Text, address2Textbox.Text, cityTextbox.Text, provinceTextbox.Text,
                                            postalCodeTextbox.Text, countryTextbox.Text, skuList.ToArray(), descriptionList.ToArray(), qtyList.ToArray(), basePriceList.ToArray(), logo, rush, additionalInfoTextbox.Text, 
                                            DateTime.ParseExact(dateDeliveryTextbox.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));

            // post order
            bp.PostOrder(bpValue);
            #endregion

            // set label to visible to inidcate success
            newQuoteLabel.Visible = true;
        }

        #region Supporting Methods
        /* a method that add text and value to the drop down list */
        private void Initialization()
        {
            // initialize grid view
            DataTable table = new DataTable();
            table.Columns.Add("SKU", typeof(string));
            table.Columns.Add("Short Description", typeof(string));
            table.Columns.Add("Gift Box", typeof(bool));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("Base Price", typeof(double));
            gridview.DataSource = table;
            gridview.DataBind();
            ViewState["DataTable"] = table;

            #region Drop Down List
            // local field for storing data
            List<ListItem> skuList = new List<ListItem> {new ListItem("")};

            // adding SKUs to the dropdown list
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.Designcs))
            {
                SqlCommand command = new SqlCommand("SELECT SKU_Ashlin, Short_Description, GiftBox, Base_Price FROM master_SKU_Attributes sku " +
                                                    "INNER JOIN master_Design_Attributes design ON design.Design_Service_Code = sku.Design_Service_Code " +
                                                    "WHERE sku.Active = 'True'", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    skuList.Add(new ListItem(reader.GetString(0), reader.GetString(1) + ';' + reader.GetBoolean(2) + ';' + reader.GetValue(3)));
            }

            skuDropdownlist.DataSource = skuList;
            skuDropdownlist.DataTextField = "Text";
            skuDropdownlist.DataValueField = "Value";
            skuDropdownlist.DataBind();
            #endregion

            // check if the user has cookies login
            if (Request.Cookies["Login"] != null)
                Session["HasLogged"] = true;
            else
            {
                // the case has no cookies login -> retrieve username and password
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.ASCMcs))
                {
                    SqlCommand command = new SqlCommand("SELECT [Username], [Password] FROM ASCM_Credentials WHERE Source = 'Ashlin Customer Enquiry'", connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();

                    Application["USERNAME"] = reader.GetString(0);
                    Application["PASSWORD"] = reader.GetString(1);
                }
            }

            // add function to checkbox list -> only allow one selection
            rushCheckboxList.Attributes.Add("onclick", "return HandleOnCheckRush()");
            logoCheckboxList.Attributes.Add("onclick", "return HandleOnCheckLogo()");
        }

        /* a supporting method that take a BPvalues object and display the value on the controls */
        private void ShowResult(BPvalues display)
        {
            resultPhoneTextbox.Text = display.Phone;
            resultEmailTextbox.Text = display.Email;
            resultCompanyTextbox.Text = display.Company;
        }
        #endregion
    }
}