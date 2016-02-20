<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="AshlinCustomerQuery.Main" %>
<%@ Register Assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" TagPrefix="ajax"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ashlin Customer Query Form</title>
    <style type="text/css">
        .auto-style2 {
            width: 481px;
        }
        .auto-style5 {
            width: 100%;
        }
        .auto-style18 {
            width: 149px;
        }
        .auto-style34 {
            width: 121px;
        }
        .auto-style35 {
            width: 122px;
        }
        .auto-style38 {
            width: 15px;
        }
        .auto-style39 {
            width: 151px;
            overflow: auto;
        }
        .auto-style40 {
            width: 121px;
            height: 23px;
        }
        .auto-style41 {
            width: 15px;
            height: 23px;
        }
        .auto-style42 {
            width: 122px;
            height: 23px;
        }
        .auto-style43 {
            width: 151px;
            overflow: auto;
            height: 23px;
        }
        .auto-style45 {
            width: 121px;
            text-align: right;
            height: 23px;
        }
        .auto-style48 {
            width: 122px;
            text-align: right;
            height: 23px;
        }
        .auto-style50 {
            width: 101px;
        }
        .auto-style63 {
            width: 101px;
            height: 23px;
        }
        .auto-style64 {
            text-align: center;
        }
        .Search:hover {
            background-color: lightblue;
        }
        .auto-style65 {
            width: 121px;
            height: 24px;
        }
        .auto-style66 {
            width: 15px;
            height: 24px;
        }
        .auto-style67 {
            width: 122px;
            text-align: right;
            height: 24px;
        }
        .auto-style68 {
            width: 122px;
            height: 24px;
        }
        .auto-style69 {
            width: 151px;
            overflow: auto;
            height: 24px;
        }
        .auto-style70 {
            width: 101px;
            height: 24px;
        }
        .auto-style71 {
            width: 100%;
        }
        .auto-style72 {
            width: 261px;
        }
        .auto-style74 {
            font-size: medium;
        }
        .auto-style81 {
            width: 148px;
        }
        .auto-style82 {
            width: 148px;
            text-align: right;
        }
        .auto-style91 {
            width: 149px;
            text-align: right;
        }
        .auto-style92 {
            width: 100%;
        }
        .auto-style93 {
            width: 100%;
        }
        .auto-style94 {
            width: 148px;
            height: 23px;
        }
        .auto-style95 {
            width: 149px;
            height: 23px;
        }
        .Quote:hover {
            background-color: lightblue;
        }
        .auto-style96 {
            height: 23px;
            text-align: center;
        }
        .auto-style97 {
            width: 480px;
        }
        .auto-style107 {
            width: 369px;
        }
        .auto-style112 {
            width: 141px;
            height: 23px;
        }
        .auto-style113 {
            width: 218px;
            height: 23px;
        }
        .auto-style116 {
            width: 141px;
        }
        .auto-style117 {
            width: 218px;
        }
        .auto-style119 {
            width: 141px;
            text-align: center;
        }
        .modalBackground {
            background-color: white;
            filter: alpha(opacity=85);
            opacity: 0.85;
            z-index: 10000;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div>
        <table class="auto-style93">
            <tr>
                <td class="auto-style34">&nbsp;</td>
                <td class="auto-style34">&nbsp;</td>
                <td class="auto-style34">&nbsp;</td>
                <td class="auto-style34">&nbsp;</td>
                <td class="auto-style34" rowspan="3">
                    <asp:HyperLink ID="logoLink" runat="server" ImageUrl="~/image/AshlinIcon.png" ImageWidth="140px" NavigateUrl="http://www.ashlinbpg.com/retail/">go to Ashlin website</asp:HyperLink>
                </td>
                <td class="auto-style34" rowspan="3">
                    &nbsp;</td>
                <td class="auto-style34" rowspan="3">
                    &nbsp;</td>
                <td class="auto-style34"><h1 style="text-align: center; font-size: 40px" class="auto-style97">Ashlin BPG Marketing Inc</h1></td>
                <td class="auto-style35">
                    &nbsp;</td>
                <td class="auto-style35">
                    &nbsp;</td>
                <td class="auto-style35">
                    <br />
                </td>
                <td class="auto-style35">&nbsp;</td>
                <td class="auto-style35">&nbsp;</td>
                <td class="auto-style35">&nbsp;</td>
                <td class="auto-style35">&nbsp;</td>
                <td class="auto-style35">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style34">&nbsp;</td>
                <td class="auto-style34">&nbsp;</td>
                <td class="auto-style34">&nbsp;</td>
                <td class="auto-style34">&nbsp;</td>
                <td class="auto-style34"><h2 style="text-align: center" class="auto-style2">Customer Query Form</h2></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td colspan="4">Become a Fan:<br />
                    <asp:HyperLink ID="facebook" runat="server" Height="20px" ImageHeight="20px" ImageUrl="~/image/facebook.png" ImageWidth="20px" Width="20px" NavigateUrl="https://www.facebook.com/ashlinbpg">go to Facebook</asp:HyperLink>
&nbsp;&nbsp;<asp:HyperLink ID="twitter" runat="server" Height="20px" ImageHeight="20px" ImageUrl="~/image/twitter.png" ImageWidth="20px" Width="20px" NavigateUrl="https://twitter.com/ashlinbpg">go to Twitter</asp:HyperLink>
&nbsp;&nbsp;<asp:HyperLink ID="pinterest" runat="server" Height="20px" ImageHeight="20px" ImageUrl="~/image/pinterest.png" ImageWidth="20px" Width="20px" NavigateUrl="https://www.pinterest.com/ashlinbpg/">go to Pinterest</asp:HyperLink>
&nbsp;&nbsp;<asp:HyperLink ID="linkedin" runat="server" Height="20px" ImageHeight="20px" ImageUrl="~/image/linkedin.png" ImageWidth="20px" Width="20px" NavigateUrl="https://www.linkedin.com/company/ashlin-bpg-marketing">go to Linkedin</asp:HyperLink>
                </td>
                <td class="auto-style35">&nbsp;</td>
                <td class="auto-style35">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style34">&nbsp;</td>
                <td class="auto-style34">&nbsp;</td>
                <td class="auto-style34">&nbsp;</td>
                <td class="auto-style34">&nbsp;</td>
                <td class="auto-style34">&nbsp;</td>
                <td class="auto-style35">&nbsp;</td>
                <td class="auto-style35">&nbsp;</td>
                <td class="auto-style35">&nbsp;</td>
                <td class="auto-style35">&nbsp;</td>
                <td class="auto-style35">&nbsp;</td>
                <td class="auto-style35">&nbsp;</td>
                <td class="auto-style35">&nbsp;</td>
                <td class="auto-style35">&nbsp;</td>
            </tr>
            </table>
    
    </div>
        <asp:Panel ID="Panel1" runat="server" BackColor="#F0F0F0">
            <table class="auto-style5">
                <tr>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style35">&nbsp;</td>
                    <td class="auto-style35">&nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style35">&nbsp;</td>
                    <td class="auto-style39">&nbsp;</td>
                    <td class="auto-style50">&nbsp;</td>
                    <td class="auto-style50">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style35">&nbsp;</td>
                    <td class="auto-style35">&nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style35">&nbsp;</td>
                    <td class="auto-style39">&nbsp;</td>
                    <td class="auto-style50">&nbsp;</td>
                    <td class="auto-style50">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td colspan="2" class="auto-style74">
                        <h4><strong>Personal Information:</strong></h4>
                    </td>
                    <td class="auto-style38">&nbsp;</td>
                    <td colspan="2">
                        <h4><strong>Contact Infomation:</strong></h4>
                    </td>
                    <td class="auto-style38">&nbsp;</td>
                    <td colspan="2">
                        <h4><strong>Order Details:</strong></h4>
                    </td>
                    <td class="auto-style50">&nbsp;</td>
                    <td class="auto-style50">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style40"></td>
                    <td class="auto-style40"></td>
                    <td class="auto-style45">
                        First Name :</td>
                    <td class="auto-style40">
                        <asp:TextBox ID="firstNameTextbox" runat="server" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style41"></td>
                    <td class="auto-style48">Company Name :</td>
                    <td class="auto-style42">
                        <asp:TextBox ID="companyNameTextbox" runat="server" Width="151px" TabIndex="4"></asp:TextBox>
                    </td>
                    <td class="auto-style41"></td>
                    <td class="auto-style48">Item # :</td>
                    <td class="auto-style43">
                        <asp:DropDownList ID="skuDropdownlist" runat="server" Width="151px" AutoPostBack="True" OnSelectedIndexChanged="skuDropdownlist_SelectedIndexChanged" TabIndex="7">
                        </asp:DropDownList>
                    </td>
                    <td class="auto-style63">*</td>
                    <td class="auto-style63"></td>
                </tr>
                <tr>
                    <td class="auto-style40"></td>
                    <td class="auto-style40"></td>
                    <td class="auto-style45">
                        Last Name :</td>
                    <td class="auto-style40">
                        <asp:TextBox ID="lastNameTextbox" runat="server" Width="151px" TabIndex="1"></asp:TextBox>
                    </td>
                    <td class="auto-style41"></td>
                    <td class="auto-style48"></td>
                    <td class="auto-style42"></td>
                    <td class="auto-style41"></td>
                    <td class="auto-style48">&nbsp;</td>
                    <td class="auto-style43">
                        <asp:TextBox ID="shortDescriptionTextbox" runat="server" Enabled="False" Height="20px" TextMode="MultiLine" Width="151px" style="overflow: auto" TabIndex="8"></asp:TextBox>
                    </td>
                    <td class="auto-style63"></td>
                    <td class="auto-style63"></td>
                </tr>
                <tr>
                    <td class="auto-style65"></td>
                    <td class="auto-style65"></td>
                    <td class="auto-style65"></td>
                    <td class="auto-style65"></td>
                    <td class="auto-style66"></td>
                    <td class="auto-style67">Phone Number :</td>
                    <td class="auto-style68">
                        <asp:TextBox ID="phoneTextbox" runat="server" Width="151px" TabIndex="5"></asp:TextBox>
                    </td>
                    <td class="auto-style66"></td>
                    <td class="auto-style67">Quantity :</td>
                    <td class="auto-style69">
                        <asp:TextBox ID="quantityTextbox" runat="server" Width="151px" TabIndex="9"></asp:TextBox>
                    </td>
                    <td class="auto-style70">*</td>
                    <td class="auto-style70"></td>
                </tr>
                <tr>
                    <td class="auto-style40"></td>
                    <td class="auto-style40"></td>
                    <td class="auto-style45">SAGE# :</td>
                    <td class="auto-style40">
                        <asp:TextBox ID="sageTextbox" runat="server" Width="151px" TabIndex="2"></asp:TextBox>
                    </td>
                    <td class="auto-style41"></td>
                    <td class="auto-style48">Email Address : </td>
                    <td class="auto-style42">
                        <asp:TextBox ID="emailTextbox" runat="server" Width="151px" TabIndex="6"></asp:TextBox>
                    </td>
                    <td class="auto-style41"></td>
                    <td class="auto-style48">Date for Event :</td>
                    <td class="auto-style43">
                        <asp:TextBox ID="dateEventTextbox" runat="server" Width="106px" TabIndex="10"></asp:TextBox>
                        <asp:Button ID="dateEventButton" runat="server" OnClick="dateEventButton_Click" Text="..." Width="37px" />
                    </td>
                    <td colspan="2" rowspan="8">
                        <asp:Calendar ID="Calendar" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" Visible="False" Width="200px" OnSelectionChanged="Calendar_SelectionChanged">
                            <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                            <NextPrevStyle VerticalAlign="Bottom" />
                            <OtherMonthDayStyle ForeColor="#808080" />
                            <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                            <SelectorStyle BackColor="#CCCCCC" />
                            <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                            <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                            <WeekendDayStyle BackColor="#FFFFCC" />
                        </asp:Calendar>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style40"></td>
                    <td class="auto-style40"></td>
                    <td class="auto-style45">ASI# :</td>
                    <td class="auto-style40">
                        <asp:TextBox ID="asiTextbox" runat="server" Width="151px" TabIndex="3"></asp:TextBox>
                    </td>
                    <td class="auto-style41"></td>
                    <td class="auto-style42"></td>
                    <td class="auto-style42"></td>
                    <td class="auto-style41"></td>
                    <td class="auto-style48">Budget :</td>
                    <td class="auto-style43">
                        <asp:TextBox ID="budgetTextbox" runat="server" Width="106px" TabIndex="11"></asp:TextBox>
                        &nbsp;CAD</td>
                </tr>
                <tr>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style35">&nbsp;</td>
                    <td class="auto-style35">&nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style35">&nbsp;</td>
                    <td class="auto-style39">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style40"></td>
                    <td class="auto-style40"></td>
                    <td class="auto-style40"></td>
                    <td class="auto-style40"></td>
                    <td class="auto-style41"></td>
                    <td class="auto-style96" colspan="2">
                        <asp:Label ID="verificationLabel" runat="server" Visible="False" Font-Size="Smaller"></asp:Label>
                    </td>
                    <td class="auto-style41"></td>
                    <td class="auto-style42"></td>
                    <td class="auto-style43"></td>
                </tr>
                <tr>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style64" colspan="2" rowspan="4">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="searchButton" runat="server" Height="50px" ImageUrl="~/image/search.png" OnClick="searchButton_Click" CssClass="Search" TabIndex="12"/>
                        &nbsp;<strong><em>Search</em></strong></td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style35">&nbsp;</td>
                    <td class="auto-style39">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style35">&nbsp;</td>
                    <td class="auto-style39">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style35">&nbsp;</td>
                    <td class="auto-style39">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style34">&nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style35">&nbsp;</td>
                    <td class="auto-style39">&nbsp;</td>
                </tr>
            </table>
            <div style="background-color: white">
                <table class="auto-style71">
                    <tr>
                        <td>&nbsp;</td>
                        <td class="auto-style72">
                            <asp:RadioButton ID="autoFillRadioButton" runat="server" AutoPostBack="True" Font-Overline="False" Font-Size="Small" Text="Auto Fill In Mode" GroupName="AutoManualRadioButtonGroup" Checked="True" OnCheckedChanged="autoFillRadioButton_CheckedChanged" TabIndex="13" />
                            <asp:RadioButton ID="manualFillRadioButton" runat="server" AutoPostBack="True" Font-Size="Small" Text="Manual Fill In Mode" GroupName="AutoManualRadioButtonGroup" OnCheckedChanged="manualFillRadioButton_CheckedChanged1" TabIndex="14" />
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <asp:Panel ID="Panel2" runat="server" BackColor="#F0F0F0">
            <table class="auto-style92">
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td colspan="2">
                        <h4>Customer Information Result :</h4>
                    </td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style82">First Name :</td>
                    <td class="auto-style81">
                        <asp:TextBox ID="firstNameResultTextbox" runat="server" Width="151px" Enabled="False" TabIndex="15"></asp:TextBox>
                    </td>
                    <td class="auto-style38">*</td>
                    <td class="auto-style91">Address 1 :</td>
                    <td class="auto-style18" rowspan="2">
                        <asp:TextBox ID="address1ResultTextbox" runat="server" Height="40px" TextMode="MultiLine" Width="151px" Enabled="False" style="overflow: auto" TabIndex="20"></asp:TextBox>
                    </td>
                    <td class="auto-style18">*</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style82">Last Name:</td>
                    <td class="auto-style81">
                        <asp:TextBox ID="lastNameResultTextbox" runat="server" Width="151px" Enabled="False" TabIndex="16"></asp:TextBox>
                    </td>
                    <td class="auto-style38">*</td>
                    <td class="auto-style91">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style82">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style91">Address 2 :</td>
                    <td class="auto-style18" rowspan="2">
                        <asp:TextBox ID="address2ResultTextbox" runat="server" Height="40px" TextMode="MultiLine" Width="151px" Enabled="False" style="overflow: auto" TabIndex="21"></asp:TextBox>
                    </td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style82">Phone Number :</td>
                    <td class="auto-style81">
                        <asp:TextBox ID="phoneResultTextbox" runat="server" Width="151px" Enabled="False" TabIndex="17"></asp:TextBox>
                    </td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style91">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style82">Email Address :</td>
                    <td class="auto-style81">
                        <asp:TextBox ID="emailResultTextbox" runat="server" Width="151px" Enabled="False" TabIndex="18"></asp:TextBox>
                    </td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style91">City :</td>
                    <td class="auto-style18">
                        <asp:TextBox ID="cityTextbox" runat="server" Enabled="False" Width="151px" TabIndex="22"></asp:TextBox>
                    </td>
                    <td class="auto-style18">*</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style91">Province / State : </td>
                    <td class="auto-style18">
                        <asp:TextBox ID="provinceTextbox" runat="server" Enabled="False" Width="151px" TabIndex="23"></asp:TextBox>
                    </td>
                    <td class="auto-style18">*</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style82">&nbsp;</td>
                    <td class="auto-style81">
                        &nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style91">Postal Code :</td>
                    <td class="auto-style18">
                        <asp:TextBox ID="postalCodeTextbox" runat="server" Enabled="False" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style18">*</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style82">Company Name :</td>
                    <td class="auto-style81">
                        <asp:TextBox ID="companyResultTextbox" runat="server" Enabled="False" TabIndex="19" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style91">Country :</td>
                    <td class="auto-style18">
                        <asp:TextBox ID="countryTextbox" runat="server" Enabled="False" TabIndex="24" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style18">*</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">
                        <asp:Label ID="uselessLabel" runat="server" style="display: none"></asp:Label>
                    </td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style64" colspan="3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="numberLabel" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style82">
                        <asp:Button ID="prevButton" runat="server" Height="23px" OnClick="prevButton_Click" Text="Prev" Width="50px" TabIndex="25" />
                    </td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style18">
                        <asp:Button ID="nextButton" runat="server" Height="23px" OnClick="nextButton_Click" Text="Next" Width="50px" TabIndex="26" />
                    </td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style94"></td>
                    <td class="auto-style94"></td>
                    <td class="auto-style94"></td>
                    <td class="auto-style94"></td>
                    <td colspan="3" rowspan="3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="quoteButton" runat="server" ImageUrl="~/image/quote.png" Width="50px" CssClass="Quote" TabIndex="27" OnClick="quoteButton_Click"/>
                        <strong><em>Quote</em></strong></td>
                    <td class="auto-style95"></td>
                    <td class="auto-style95"></td>
                    <td class="auto-style95"></td>
                    <td class="auto-style95"></td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="Panel3" runat="server" Height="210px" Width="369px" style="display: none">
            <table class="auto-style107">
                <tr>
                    <td class="auto-style112"></td>
                    <td class="auto-style113"></td>
                </tr>
                <tr>
                    <td colspan="2"><strong>Please login to continue</strong></td>
                </tr>
                <tr>
                    <td class="auto-style116">&nbsp;</td>
                    <td class="auto-style117">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style119">Username :</td>
                    <td class="auto-style117">
                        <asp:TextBox ID="usernameTextbox" runat="server" Height="16px" Width="180px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style119">Password :</td>
                    <td class="auto-style117">
                        <asp:TextBox ID="passwordTextbox" TextMode="Password" runat="server" Height="16px" Width="180px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style116">&nbsp;</td>
                    <td class="auto-style117">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style64" colspan="2">
                        <asp:Button ID="cancelButton" runat="server" BackColor="Red" Font-Bold="True" ForeColor="White" Text="Cancel" Width="75px" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="loginButton" runat="server" BackColor="#00CC66" Font-Bold="True" OnClick="loginButton_Click" ForeColor="White" Text="Login" Width="75px" />
                    </td>
                </tr>
            </table>
            
        </asp:Panel>
        
       <ajax:ModalPopupExtender ID="popup" runat="server" TargetControlID="uselessLabel" PopupControlID="Panel3" CancelControlID="cancelButton"
            BackgroundCssClass="modalBackground"/>
    </form>
</body>
</html>
