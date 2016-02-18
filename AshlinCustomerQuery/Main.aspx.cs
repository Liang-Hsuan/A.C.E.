using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI.WebControls;
using AshlinCustomerQuery.SearchingClasses.Brightpearl;
using AshlinCustomerQuery.SearchingClasses.Sage;

namespace AshlinCustomerQuery
{
    public partial class Main : System.Web.UI.Page
    {
        // fields for order and person / company information
        private string firstName;
        private string lastName;
        private string sage;
        private string asi;
        private string company;
        private string phone;
        private string email;
        private string sku;
        private int quantity;
        private string theme;
        private DateTime date;
        private double budget;

        // fields for customer searching
        private BPsearch search = new BPsearch();
        private BPvalues[] value;

        // supportin flag
        private int current;            // default set to 1

        // field for dropdown list items
        private List<ListItem> skuList = new List<ListItem>();

        /* page load that initialize the dropdown list items */
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                generateDropDownList();

                current = 1;
            }
            else
            {
                // after a post back return to the position as before
                Page.MaintainScrollPositionOnPostBack = true;

                // restore value
                if (ViewState["Value"] != null)
                    value = (BPvalues[])ViewState["Value"];
            }
        }

        /* the event for sku dropdown list that show the description of the selected item */
        protected void skuDropdownlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            shortDescriptionTextbox.Text = skuDropdownlist.SelectedValue;
        }

        /* the event for date event button click that change the visibility of the calendar */
        protected void dateEventButton_Click(object sender, EventArgs e)
        {
            if (!Calendar.Visible)
                Calendar.Visible = true;
            else
                Calendar.Visible = false;
        }

        /* a method that add text and value to the drop down list */
        private void generateDropDownList()
        {
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

            skuDropdownlist.DataSource = skuList;
            skuDropdownlist.DataTextField = "Text";
            skuDropdownlist.DataValueField = "Value";
            skuDropdownlist.DataBind();
        }

        /* the event for calendar date selection that show the date time on the textbox */
        protected void Calendar_SelectionChanged(object sender, EventArgs e)
        {
            dateEventTextbox.Text = Calendar.SelectedDate.ToString();
        }

        #region Radio Buttons
        /* the event for auto and manual radio button clicks */
        protected void autoFillRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (autoFillRadioButton.Checked)
            {
                firstNameResultTextbox.Enabled = false;
                lastNameResultTextbox.Enabled = false;
                phoneResultTextbox.Enabled = false;
                emailResultTextbox.Enabled = false;
                companyResultTextbox.Enabled = false;
                address1ResultTextbox.Enabled = false;
                address2ResultTextbox.Enabled = false;
                cityTextbox.Enabled = false;
                provinceTextbox.Enabled = false;
                countryTextbox.Enabled = false;
            }
        }
        protected void manualFillRadioButton_CheckedChanged1(object sender, EventArgs e)
        {
            if (manualFillRadioButton.Checked)
            {
                firstNameResultTextbox.Enabled = true;
                lastNameResultTextbox.Enabled = true;
                phoneResultTextbox.Enabled = true;
                emailResultTextbox.Enabled = true;
                companyResultTextbox.Enabled = true;
                address1ResultTextbox.Enabled = true;
                address2ResultTextbox.Enabled = true;
                cityTextbox.Enabled = true;
                provinceTextbox.Enabled = true;
                countryTextbox.Enabled = true;
            }
        }
        #endregion

        /* the event for search button click that search the information about the customer and show them in the result section */
        protected void searchButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            // clear text first
            firstNameResultTextbox.Text = "";
            lastNameResultTextbox.Text = "";
            emailResultTextbox.Text = "";
            phoneResultTextbox.Text = "";
            companyResultTextbox.Text = "";
            address1ResultTextbox.Text = "";
            address2ResultTextbox.Text = "";
            cityTextbox.Text = "";
            provinceTextbox.Text = "";
            countryTextbox.Text = "";
            numberLabel.Text = "";
            verificationLabel.Visible = false;

            // get data from textboxes
            firstName = firstNameTextbox.Text;
            lastName = lastNameTextbox.Text;
            sage = sageTextbox.Text;
            asi = asiTextbox.Text;
            company = companyNameTextbox.Text;
            phone = phoneTextbox.Text;
            email = emailTextbox.Text;
            sage = sageTextbox.Text;

            #region Error Checking
            // the case if user haven't provide any thing about customer
            if (firstName.Equals("") && lastName.Equals("") && sage.Equals("") && 
                asi.Equals("") && company.Equals("")  && email.Equals(""))
            {
                string script = "<script>alert(\"Please provide some information about the customer\");</script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", script);

                return;
            }
            #endregion

            #region Searching and Adding
            // down to business
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

            #region Verfication
            // the case if the user provide sage number -> check if the information is matched
            if (!sage.Equals(""))
            {
                SageSearch sageSearch = new SageSearch();
                SageValues sageValue = new SageValues(firstName, lastName, company, phone, email, sage);

                int confidenceLevel = sageSearch.getConfidenceLevel(sageValue);

                if (confidenceLevel < 30)
                {
                    verificationLabel.Text = "Sage# not matched the information provided";
                    verificationLabel.ForeColor = Color.Red;
                }
                else if (confidenceLevel >= 30 && confidenceLevel < 50)
                {
                    verificationLabel.Text = "Sage# not quite matched the information provided";
                    verificationLabel.ForeColor = Color.Orange;
                }
                else if (confidenceLevel >= 50 && confidenceLevel < 75)
                {
                    verificationLabel.Text = "Sage# quite matched the information provided";
                    verificationLabel.ForeColor = Color.LightGreen;
                }
                else
                {
                    verificationLabel.Text = "Sage# matched the information provided very well";
                    verificationLabel.ForeColor = Color.Green;
                }

                verificationLabel.Visible = true;
            }
            #endregion

            // the case if there is no result
            if (value == null)
            {
                string script = "<script>alert(\"There is no result of this customer. Please enter information manually\");</script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", script);

                return;
            }

            current = 1;
            numberLabel.Text = current + " / " + value.Length;
            showResult(value[0]);

            // save viewstate
            ViewState["Value"] = value;
            ViewState["Current"] = current;
            ViewState["HasSearched"] = true;
        }

        /* a supporting method that take a BPvalues object and display the value on the controls */
        private void showResult(BPvalues display)
        {
            firstNameResultTextbox.Text = display.FirstName;
            lastNameResultTextbox.Text = display.LastName;
            phoneResultTextbox.Text = display.Phone;
            emailResultTextbox.Text = display.Email;
            companyResultTextbox.Text = display.Company;
            address1ResultTextbox.Text = display.Address1;
            address2ResultTextbox.Text = display.Address2;
            cityTextbox.Text = display.City;
            provinceTextbox.Text = display.Province;
            countryTextbox.Text = display.Country;
        }

        #region Next and Prev Buttons
        /* next and prev button event that change the page */
        protected void prevButton_Click(object sender, EventArgs e)
        {
            // check if the user has searched the result or not, if has restore data
            if (ViewState["HasSearched"] == null)
                return;
            current = Convert.ToInt32(ViewState["Current"]);

            if (current > 1)
            {
                current--;
            }

            showResult(value[current - 1]);
            numberLabel.Text = current + " / " + value.Length;

            // save current data
            ViewState["Current"] = current;
        }
        protected void nextButton_Click(object sender, EventArgs e)
        {
            // check if the user has already searched the result or not, if has restore data
            if (ViewState["HasSearched"] == null)
                return;
            current = Convert.ToInt32(ViewState["Current"]);

            if (current < value.Length)
            {
                current++;
            }

            showResult(value[current - 1]);
            numberLabel.Text = current + " / " + value.Length;

            // save current data
            ViewState["Current"] = current;
        }
        #endregion
    }
}