using AshlinCustomerEnquiry.supportingClasses.brightpearl;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace AshlinCustomerEnquiry
{
    public partial class Main : System.Web.UI.Page
    {
        // field for storing customer data
        private BPvalues[] value;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                generateDropDownList();

                // add function to checkbox list -> only allow one selection
                rushCheckboxList.Attributes.Add("onclick", "return HandleOnCheckRush()");
                logoCheckboxList.Attributes.Add("onclick", "return HandleOnCheckLogo()");

                welcomePopup.Show();
            }
            else
            {
                // after a post back return to the position as before
                Page.MaintainScrollPositionOnPostBack = true;

                // restore value
                if (ViewState["Value"] != null)
                    value = (BPvalues[]) ViewState["Value"];
            }
        }

        #region Comboboxes Event
        /* selection change that show the corresponding value on the textboxes */
        protected void skuDropdownlist1_SelectedIndexChanged(object sender, EventArgs e)
        {
            shortDescriptionTextbox1.Text = skuDropdownlist1.SelectedValue;
        }
        protected void skuDropdownlist2_SelectedIndexChanged(object sender, EventArgs e)
        {
            shortDescriptionTextbox2.Text = skuDropdownlist2.SelectedValue;
        }
        protected void skuDropdownlist3_SelectedIndexChanged(object sender, EventArgs e)
        {
            shortDescriptionTextbox3.Text = skuDropdownlist3.SelectedValue;
        }
        #endregion

        #region Calendar Event
        /* the event for date event button click that change the visibility of the calendar */
        protected void dateEventButton_Click(object sender, EventArgs e)
        {
            if (!calendar.Visible)
                calendar.Visible = true;
            else
                calendar.Visible = false;
        }

        /* the event for calendar date selection that show the date time on the textbox */
        protected void calendar_SelectionChanged(object sender, EventArgs e)
        {
            dateEventTextbox.Text = calendar.SelectedDate.ToString("yyyy-MM-dd");
        }
        #endregion

        #region Radio Buttons
        /* the event for auto and manual radio button clicks */
        protected void disableRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (disableRadioButton.Checked)
            {
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
        }
        protected void enableRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (enableRadioButton.Checked)
            {
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

        protected void asiSkipButton_Click(object sender, EventArgs e)
        {
            asiPopup.Hide();
            searchPopup.Show();
        }

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
                string script = "<script>alert(\"Please provide some information about the customer\");</script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", script);

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
                list[0] = search.getCustomerWithName(firstName, lastName, 3);
            else if (firstName != "" && lastName == "") // the case only firstname is supplied
                list[0] = search.getCustomerWithName(firstName, null, 1);
            else if (firstName == "" && lastName != "") // the case only lastname is supplied
                list[0] = search.getCustomerWithName(null, lastName, 2);
            else
                list[0] = null;

            // search with contact info
            if (company != "" && email != "")           // the case both company and email are supplied
                list[1] = search.getCustomerWithInfo(email, company, 3);
            else if (company != "" && email == "")      // the case only company is supplied
                list[1] = search.getCustomerWithInfo(null, company, 2);
            else if (company == "" && email != "")      // the case only email is supplied
                list[1] = search.getCustomerWithInfo(email, null, 1);
            else
                list[1] = null;

            // allocating data
            if (list[0] != null & list[1] != null)
            {
                if (list[0].Length < list[1].Length)
                    value = list[0];
                else
                    value = list[1];
            }
            else if (list[0] == null)
                value = list[1];
            else if (list[1] == null)
                value = list[0];
            #endregion

            // the case if there is no result or there are too many result
            if (value == null)
            {
                string script = "<script>alert(\"There is no result of this customer. Please enter information manually\");</script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", script);
                searchPopup.Show();

                return;
            }
            else if (value[0].FirstName == "-1")
            {
                tooManyResultLabel.Visible = true;
                searchPopup.Show();

                return;
            }

            // show all the result 
            listbox.Items.Clear();
            foreach (BPvalues result in value)
            {
                ListItem item = new ListItem(result.FirstName + " " + result.LastName);
                listbox.Items.Add(item);
            }
            showResult(value[0]);

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
            showResult(value[listbox.SelectedIndex]);
            resultPopup.Show();
        }

        /* the event for select button click that show the result of the selected customer to the form */
        protected void resultSelectButton_Click(object sender, EventArgs e)
        {
            int i = listbox.SelectedIndex;

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

        /* the event for back button click that goes back to search panel */
        protected void resultBackButton_Click(object sender, EventArgs e)
        {
            resultPopup.Hide();
            searchPopup.Show();
        }
        #endregion

        #region Supporting Methods
        /* a method that add text and value to the drop down list */
        private void generateDropDownList()
        {
            // local field for storing data
            List<ListItem> skuList = new List<ListItem>();

            // adding SKUs to the dropdown list
            skuList.Add(new ListItem(""));
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.Designcs))
            {
                SqlCommand command =
                    new SqlCommand("SELECT SKU_Ashlin, Short_Description FROM master_SKU_Attributes sku " +
                                   "INNER JOIN master_Design_Attributes design ON design.Design_Service_Code = sku.Design_Service_Code " +
                                   "WHERE sku.Active = \'True\';", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    skuList.Add(new ListItem(reader.GetString(0), reader.GetString(1)));
                }
            }

            skuDropdownlist1.DataSource = skuList;
            skuDropdownlist1.DataTextField = "Text";
            skuDropdownlist1.DataValueField = "Value";
            skuDropdownlist1.DataBind();

            skuDropdownlist2.DataSource = skuList;
            skuDropdownlist2.DataTextField = "Text";
            skuDropdownlist2.DataValueField = "Value";
            skuDropdownlist2.DataBind();

            skuDropdownlist3.DataSource = skuList;
            skuDropdownlist3.DataTextField = "Text";
            skuDropdownlist3.DataValueField = "Value";
            skuDropdownlist3.DataBind();
        }

        /* a supporting method that take a BPvalues object and display the value on the controls */
        private void showResult(BPvalues display)
        {
            resultPhoneTextbox.Text = display.Phone;
            resultEmailTextbox.Text = display.Email;
            resultCompanyTextbox.Text = display.Company;
        }
        #endregion
    }
}