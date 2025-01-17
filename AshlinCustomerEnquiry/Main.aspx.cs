﻿using AshlinCustomerEnquiry.supportingClasses.asi;
using AshlinCustomerEnquiry.supportingClasses.brightpearl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AshlinCustomerEnquiry
{
    public partial class Main : Page
    {
        // field for storing customer data
        private BPvalues[] value;

        // fields for ASI and Brightpearl
        private Asi asi;
        private BPconnect bp;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Initialization();
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
            calendar.Visible = false;
        }

        /* set the canlender can only select the day after today */
        protected void calendar_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.Date > DateTime.Today) return;
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
                BPvalues asiValue = asi.GetCompanyInfo(asiTextbox.Text);

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

                    firstNameTextbox.Text = string.Empty;
                    lastNameTextbox.Text = string.Empty;

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
                    ListItem item = new ListItem(result.FirstName + ' ' + result.LastName);
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
                Page.ClientScript.RegisterStartupScript(GetType(), "Scripts", script);
                searchPopup.Show();

                return;
            }
            #endregion

            #region Searching and Adding
            // supporting local field -> [0] name, [1] contact info
            BPvalues[][] list = new BPvalues[2][];

            // search with name
            if (firstName != "" && lastName != "")      // the case both firstname and lastname are supplied
                list[0] = bp.GetCustomerWithName(firstName, lastName, 3);
            else if (firstName != "" && lastName == "") // the case only firstname is supplied
                list[0] = bp.GetCustomerWithName(firstName, null, 1);
            else if (firstName == "" && lastName != "") // the case only lastname is supplied
                list[0] = bp.GetCustomerWithName(null, lastName, 2);
            else
                list[0] = null;

            // search with contact info
            if (company != "" && email != "")           // the case both company and email are supplied
                list[1] = bp.GetCustomerWithInfo(email, company, 3);
            else if (company != "" && email == "")      // the case only company is supplied
                list[1] = bp.GetCustomerWithInfo(null, company, 2);
            else if (company == "" && email != "")      // the case only email is supplied
                list[1] = bp.GetCustomerWithInfo(email, null, 1);
            else
                list[1] = null;

            // allocating data
            if (list[0] != null && list[1] != null)
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
                SqlCommand command = new SqlCommand("UPDATE ASCM_Credentials SET Username = \'" + username + "\', Password = \'" + password[0] + 
                                                   "\' WHERE Source = 'Ashlin Customer Enquiry' AND Username = \'" + Application["USERNAME"] + '\'', connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        #endregion

        /* the event for login button in the login board */
        protected void loginButton_Click(object sender, EventArgs e)
        {
            // get username 
            string username = usernameTextbox.Text.Trim();

            // get the number of user that correspond to the credential input
            SqlConnection connection = new SqlConnection(Properties.Settings.Default.ASCMcs);
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT COUNT(*) from ASCM_Credentials WHERE Source = 'Ashlin Customer Enquiry' AND Username = \'"
                                              + username + "\' AND [Password] = \'" + passwordTextbox.Text + '\'', connection);

            // check if the user put the right username and password
            if ((int)command.ExecuteScalar() == 1)
            {
                // correct credential case -> set login cookie to current logged in username
                Application["USERNAME"] = username;
                HttpCookie cookie = new HttpCookie("Login")
                {
                    Value = username,
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Add(cookie);

                // show welcome panel
                welcomePopup.Show();
            }
            else
            {
                // wrong credential case -> show the login borad again and signal them wrong
                loginPopup.Show();
                usernameTextbox.BackColor = Color.Red;
                passwordTextbox.BackColor = Color.Red;
            }

            connection.Close();
        }

        /* clear screen link button clicks that clear all the information on the page */
        protected void clearScreenLinkButton_Click(object sender, EventArgs e)
        {
            // clear customer information textboxes
            firstNameTextbox.Text = string.Empty;
            lastNameTextbox.Text = string.Empty;
            phoneTextbox.Text = string.Empty;
            emailTextbox.Text = string.Empty;
            companyTextbox.Text = string.Empty;
            address1Textbox.Text = string.Empty;
            address2Textbox.Text = string.Empty;
            cityTextbox.Text = string.Empty;
            provinceTextbox.Text = string.Empty;
            postalCodeTextbox.Text = string.Empty;
            countryTextbox.Text = string.Empty;

            // clear order information
            skuDropdownlist.SelectedIndex = 0;
            foreach (ListItem item in rushCheckboxList.Items) item.Selected = false;
            foreach (ListItem item in logoCheckboxList.Items) item.Selected = false;
            foreach (ListItem item in quantityCheckboxList.Items) item.Selected = false;

            // clear item in grid view
            DataTable table = (DataTable)ViewState["DataTable"];
            table.Rows.Clear();

            // bind the data source
            gridview.EditIndex = -1;
            gridview.DataSource = table;
            gridview.DataBind();
            ViewState["DataTable"] = table;

            // additional info set to default
            additionalInfoTextbox.Text = string.Empty;
            dateDeliveryTextbox.Text = DateTime.Today.AddDays(30).ToString("yyyy-MM-dd");
            staffDropdownlist.SelectedIndex = 0;

            // set new quote created label to invisible
            newQuoteCreatedLabel.Visible = false;
        }

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
                skuValue = skuValue.Substring(skuValue.IndexOf(';') + 1);
                row[4] = double.Parse(skuValue.Substring(0, skuValue.IndexOf(';')));    // base price
                row[5] = int.Parse(skuValue.Substring(skuValue.IndexOf(';') + 1));      // pricing tier

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
            // update the quantity to the table
            DataTable table = (DataTable)ViewState["DataTable"];
            table.Rows[e.RowIndex][3] = int.Parse(((TextBox)gridview.Rows[e.RowIndex].Cells[4].Controls[0]).Text);

            // bind the data source
            gridview.EditIndex = -1;
            gridview.DataSource = table;
            gridview.DataBind();
            ViewState["DataTable"] = table;
        }
        #endregion

        /* the event for quote button clicks that will call login panel if the user has not logged in and create quote */
        protected void quoteButton_Click(object sender, ImageClickEventArgs e)
        {         
            #region Error Checking
            // get table
            DataTable table = (DataTable)ViewState["DataTable"];

            if (firstNameTextbox.Text == "" || lastNameTextbox.Text == "" || address1Textbox.Text == "" || cityTextbox.Text == "" || dateDeliveryTextbox.Text == "" ||
                provinceTextbox.Text == "" || postalCodeTextbox.Text == "" || countryTextbox.Text == "" || table.Rows.Count < 1)
            {
                const string script = "alert(\"Please provide information on all the necessary fields (*)\");";
                ScriptManager.RegisterStartupScript(Page, GetType(), "ClientScript", script, true);
                return;
            }
            #endregion

            // set new quote created label to invisible
            newQuoteCreatedLabel.Visible = false;

            // local fields for BPvalue generation
            List<string> skuList = new List<string>();
            List<string> descriptionList = new List<string>();
            List<int> qtyList = new List<int>();
            List<double> basePriceList = new List<double>();
            List<int> pricingTierList = new List<int>();
            List<bool> giftBoxList = new List<bool>();
            int[] count = { rushCheckboxList.Items.Cast<ListItem>().Count(li => li.Selected), logoCheckboxList.Items.Cast<ListItem>().Count(li => li.Selected) };

            #region Email 
            // get the order detail
            string orderDetail = "Customer Information:\n\r" +
                                  "Name: " + firstNameTextbox.Text + " " + lastNameTextbox.Text + "\nPhone: " + phoneTextbox.Text + "\nEmail: " + emailTextbox.Text + "\nCompany: " + companyTextbox.Text
                                + "\nAddress1: " + address1Textbox.Text + "\nAddress2: " + address2Textbox.Text + "\nCity: " + cityTextbox.Text + "\nProvince / State: " + provinceTextbox.Text +
                                  "\nPostal Code: " + postalCodeTextbox.Text + "\nCountry: " + countryTextbox.Text + "\n\n\r" +
                                  "Order Detail:\n\r" +
                                  "Rush Order: ";
            if (count[0] > 1)
                orderDetail += "Yes, No\n";
            else if (rushCheckboxList.SelectedIndex == 0)
                orderDetail += "Yes\n";
            else
                orderDetail += "No\n";
            orderDetail += "With Logo: ";
            if (count[1] > 1)
                orderDetail += "Yes, No";
            else if (logoCheckboxList.SelectedIndex == 1)
                orderDetail += "No";
            else
                orderDetail += "Yes";
            for (int i = 0; i < table.Rows.Count; i++)
            {
                orderDetail += "\n\nItem " + (i + 1) + ": " + table.Rows[i][0] + "\nShort Description: " + table.Rows[i][1] + "\nGift Box: " + table.Rows[i][2] + "\nQuantity: " + table.Rows[i][3];

                // adding item to the lists
                skuList.Add(table.Rows[i][0].ToString());
                descriptionList.Add(table.Rows[i][1].ToString());
                giftBoxList.Add(Convert.ToBoolean(table.Rows[i][2]));
                qtyList.Add(Convert.ToInt32(table.Rows[i][3]));
                basePriceList.Add(Convert.ToDouble(table.Rows[i][4]));
                pricingTierList.Add(Convert.ToInt32(table.Rows[i][5]));
            }
            orderDetail += "\n\nDelivery Date: " + dateDeliveryTextbox.Text + "\n\nAdditional Info:\n" + additionalInfoTextbox.Text;

            // send message
            var mail = new System.Net.Mail.MailMessage();
            var client = new System.Net.Mail.SmtpClient("smtp.gmail.com");

            mail.From = new System.Net.Mail.MailAddress("intern1002@ashlinbpg.com");
            mail.To.Add("ashlin@ashlinbpg.com");
            mail.To.Add(staffDropdownlist.SelectedValue.Substring(staffDropdownlist.SelectedValue.IndexOf(';') + 1));
            mail.Subject = "NEW ORDER QUOTE";
            mail.Body = orderDetail;

            client.Port = 587;
            client.Credentials = new System.Net.NetworkCredential("intern1002@ashlinbpg.com", "AshlinIntern2");
            client.EnableSsl = true;
            client.Send(mail);
            #endregion

            #region Brightpearl
            // declare BPvalues object
            BPvalues bpValue = new BPvalues(firstNameTextbox.Text, lastNameTextbox.Text, companyTextbox.Text, phoneTextbox.Text, emailTextbox.Text, address1Textbox.Text, address2Textbox.Text, cityTextbox.Text, provinceTextbox.Text,
                                            postalCodeTextbox.Text, countryTextbox.Text, staffDropdownlist.SelectedValue.Remove(staffDropdownlist.SelectedValue.IndexOf(';')), skuList, descriptionList, qtyList, basePriceList, pricingTierList, giftBoxList, 
                                            true, false, additionalInfoTextbox.Text, DateTime.ParseExact(dateDeliveryTextbox.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));

            // price list determination
            if (count[0] > 1)
            {
                if (count[1] > 1)
                {
                    // 4 cases
                    // 1 st case
                    bpValue.Rush = true;
                    bpValue.Logo = true;
                    bp.PostOrder(bpValue);
                    // 2nd case
                    bpValue.Rush = true;
                    bpValue.Logo = false;
                    bp.PostOrder(bpValue);
                    // 3rd case
                    bpValue.Rush = false;
                    bpValue.Logo = true;
                    bp.PostOrder(bpValue);
                    // 4th case
                    bpValue.Rush = false;
                    bpValue.Logo = false;
                    bp.PostOrder(bpValue);
                }
                else if (logoCheckboxList.SelectedIndex == 1)
                {
                    // 2 cases
                    // 1st case 
                    bpValue.Rush = true;
                    bpValue.Logo = false;
                    bp.PostOrder(bpValue);
                    // 2nd case
                    bpValue.Rush = false;
                    bpValue.Logo = false;
                    bp.PostOrder(bpValue);
                }
                else
                {
                    // 2 cases
                    // 1st case 
                    bpValue.Rush = true;
                    bpValue.Logo = true;
                    bp.PostOrder(bpValue);
                    // 2nd case
                    bpValue.Rush = false;
                    bpValue.Logo = true;
                    bp.PostOrder(bpValue);
                }
            }
            else if (rushCheckboxList.SelectedIndex == 0)
            {
                if (count[1] > 1)
                {
                    // 2 cases
                    // 1 st case
                    bpValue.Rush = true;
                    bpValue.Logo = true;
                    bp.PostOrder(bpValue);
                    // 2nd case
                    bpValue.Rush = true;
                    bpValue.Logo = false;
                    bp.PostOrder(bpValue);
                }
                else if (logoCheckboxList.SelectedIndex == 1)
                {
                    // 1 case 
                    bpValue.Rush = true;
                    bpValue.Logo = false;
                    bp.PostOrder(bpValue);
                }
                else
                {
                    // 1 case
                    bpValue.Rush = true;
                    bpValue.Logo = true;
                    bp.PostOrder(bpValue);
                }
            }
            else
            {
                if (count[1] > 1)
                {
                    // 2 cases
                    // 1 st case
                    bpValue.Rush = false;
                    bpValue.Logo = true;
                    bp.PostOrder(bpValue);
                    // 2nd case
                    bpValue.Rush = false;
                    bpValue.Logo = false;
                    bp.PostOrder(bpValue);
                }
                else if (logoCheckboxList.SelectedIndex == 1)
                {
                    // 1 case 
                    bpValue.Rush = false;
                    bpValue.Logo = false;
                    bp.PostOrder(bpValue);
                }
                else
                {
                    // 1 case
                    bpValue.Rush = false;
                    bpValue.Logo = true;
                    bp.PostOrder(bpValue);
                }
            }
            #endregion

            // set new quote created label to visible
            newQuoteCreatedLabel.Visible = true;
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
            table.Columns.Add("Pricing Tier", typeof(int));
            gridview.DataSource = table;
            gridview.DataBind();
            ViewState["DataTable"] = table;

            #region Drop Down List
            // local field for storing data
            List<ListItem> list = new List<ListItem> {new ListItem("")};

            // adding SKUs to the dropdown list
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.Designcs))
            {
                SqlCommand command = new SqlCommand("SELECT SKU_Ashlin, Short_Description, GiftBox, Base_Price, Pricing_Tier FROM master_SKU_Attributes sku " +
                                                    "INNER JOIN master_Design_Attributes design ON design.Design_Service_Code = sku.Design_Service_Code " +
                                                    "WHERE sku.Active = 'True' and Design_Service_Flag = 'Design'", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    list.Add(new ListItem(reader.GetString(0), reader.GetString(1) + ';' + reader.GetBoolean(2) + ';' + reader.GetValue(3) + ';' + reader.GetInt32(4)));
            }

            skuDropdownlist.DataSource = list;
            skuDropdownlist.DataTextField = "Text";
            skuDropdownlist.DataValueField = "Value";
            skuDropdownlist.DataBind();

            // declare BPconnect object for staff search and reset list
            bp = new BPconnect();
            list.Clear();

            // get staff dictionary
            Dictionary<string, string> dic = bp.GetStaff();

            // put staff into dropboxlist
            list.AddRange(dic.Select(pair => new ListItem(pair.Value, pair.Key)));
            staffDropdownlist.DataSource = list;
            staffDropdownlist.DataTextField = "Text";
            staffDropdownlist.DataValueField = "Value";
            staffDropdownlist.DataBind();
            #endregion

            // set default delivery date
            dateDeliveryTextbox.Text = DateTime.Today.AddDays(30).ToString("yyyy-MM-dd");

            // add confirm attribute to buttons
            clearScreenLinkButton.Attributes.Add("onclick", "javascript:return confirm('Are you sure you want to clear all the information on the page?');");
            quoteButton.Attributes.Add("onclick", "javascript:return confirm('Are you sure you want to create the quote for this order?');this.disabled=true;"
                                                + ClientScript.GetPostBackEventReference(quoteButton, null) + ';');

            #region Session Declaration
            try
            {
                // initialize ASI object and store it
                if (Session["ASI"] == null)
                    Session["ASI"] = new Asi();
            }
            catch (System.Net.WebException ex)
            {
                // the case if there is error from asi -> disable asi function
                asiTextbox.Text = ((System.Net.HttpWebResponse)ex.Response).StatusDescription;
                asiTextbox.Enabled = false;
                asiNextButton.Enabled = false;
            }

            // initialize BPconnect object and store it
            if (Session["BPconnect"] == null)
                Session["BPconnect"] = bp;
            #endregion

            // check if the user has cookies login
            if (Request.Cookies["Login"] != null)
            {
                Application["USERNAME"] = Request.Cookies["Login"].Value;
                welcomePopup.Show();
            }
            else
                // the case has no cookies login -> show login panel for login
                loginPopup.Show();
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
