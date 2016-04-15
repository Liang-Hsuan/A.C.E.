<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="AshlinCustomerEnquiry.Main" %>
<%@ Register Assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" TagPrefix="ajax"%>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="image/ashlin.ico" type="image/x-icon" />
    <title>Ashlin Customer Enquiry</title>
    <style type="text/css">
        .auto-style34 {
            width: 121px;
        }
        .auto-style35 {
            width: 122px;
        }
        .auto-style81 {
            width: 148px;
        }
        .auto-style38 {
            width: 15px;
        }
        .auto-style18 {
            width: 149px;
        }
        .auto-style82 {
            width: 148px;
            text-align: right;
        }
        .auto-style91 {
            width: 149px;
            text-align: right;
        }
        .auto-style99 {
            height: 35px;
        }
        .auto-style5 {
            width: 100%;
        }
        .auto-style39 {
            width: 151px;
            overflow: auto;
        }
        .auto-style50 {
            width: 101px;
        }
        .auto-style74 {
            font-size: medium;
        }
        .auto-style40 {
            width: 121px;
            height: 23px;
        }
        .auto-style45 {
            width: 121px;
            text-align: right;
            height: 23px;
        }
        .auto-style41 {
            width: 15px;
            height: 23px;
        }
        .auto-style48 {
            width: 122px;
            text-align: right;
            height: 23px;
        }
        .auto-style63 {
            width: 101px;
            height: 23px;
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
        .auto-style96 {
            height: 23px;
            text-align: center;
        }
        .auto-style64 {
            text-align: center;
        }
        .wizard:hover {
            background-color: lightblue;
        }
        .auto-style103 {
            height: 23px;
        }
        .quote:hover {
            background-color: lightblue;
        }
        .auto-style106 {
            font-size: xx-large;
        }
        .auto-style111 {
            width: 500px;
            height: 280px;
            border-style: solid; 
            border-width: 3px;
        }
        .auto-style127 {
            width: 253px;
            height: 23px;
        }
        .auto-style128 {
            text-align: right;
            height: 33px;
            background-color: tan;
        }
        .modalBackground {
            background-color: white;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

         .auto-style132 {
            font-size: xx-large;
            text-align: center;
            height: 76px;
            background-color: tan;
        }
        .auto-style144 {
            width: 249px;
        }
        .auto-style147 {
            width: 118px;
            text-align: right;
            height: 31px;
        }
        .auto-style148 {
            width: 118px;
        }
        .auto-style149 {
            width: 249px;
            text-align: right;
        }
        .auto-style150 {
            width: 249px;
            height: 31px;
        }
        .auto-style151 {
            width: 118px;
            height: 40px;
        }
        .auto-style152 {
            width: 249px;
            height: 40px;
        }
        .auto-style153 {
            width: 118px;
            height: 30px;
        }
        .auto-style154 {
            width: 249px;
            height: 30px;
            text-align: center;
        }
        .auto-style160 {
            height: 86px;
            background-color: tan;
            font-size: xx-large;
            text-align: center;
        }
        .auto-style170 {
            height: 76px;
            text-align: center;
            font-size: xx-large;
            background-color: tan;
        }
        .auto-style174 {
            width: 550px;
            height: 325px;
            border-style: solid; 
            border-width: 3px;
        }
        .auto-style177 {
            width: 274px;
        }
        .auto-style179 {
            width: 242px;
            text-align: center;
        }
        .auto-style186 {
            width: 170px;
            text-align: right;
            height: 23px;
        }
        .auto-style187 {
            width: 224px;
            height: 23px;
        }
        .auto-style191 {
            width: 249px;
            height: 33px;
        }
        .auto-style192 {
            height: 33px;
            text-align: right;
            background-color: tan;
        }
        .auto-style194 {
            width: 274px;
            height: 23px;
        }
        .search:hover {
            background-color: lightblue;
        }
        .auto-style195 {
            width: 274px;
            height: 23px;
            text-align: right;
        }
        .auto-style199 {
            width: 249px;
            height: 23px;
        }
        .auto-style200 {
            width: 274px;
            height: 22px;
        }
        .auto-style218 {
            text-align: right;
        }
        .auto-style235 {
            height: 23px;
            width: 171px;
        }
        .auto-style236 {
            width: 125px;
            text-align: right;
            height: 23px;
        }
        .auto-style249 {
            width: 121px;
            height: 60px;
        }
        .auto-style250 {
            width: 15px;
            height: 60px;
        }
    </style>
</head>
<body style="height: 100%; background-color:tan">
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager"/>
    <div>  
        <table style="width: 100%">
            <tr>
                <td class="auto-style34"></td>
                <td class="auto-style34"></td>
                <td class="auto-style34"></td>
                <td class="auto-style34"></td>
                <td class="auto-style34" rowspan="3">
                    <asp:HyperLink ID="logoLink" runat="server" ImageUrl="~/image/AshlinIcon.png" ImageWidth="140px" NavigateUrl="http://www.ashlinbpg.com/retail/" ToolTip="go to Ashlin website" TabIndex="37">go to Ashlin website</asp:HyperLink>
                </td>
                <td class="auto-style34" rowspan="3"></td>
                <td class="auto-style34" rowspan="3"></td>
                <td class="auto-style34">
                    <h1 style="text-align: center; font-size: 40px; width: 480px">Ashlin BPG Marketing Inc</h1>
                </td>
                <td class="auto-style35"></td>
                <td class="auto-style35"></td>
                <td class="auto-style35">
                    <br />
                </td>
                <td class="auto-style35"></td>
                <td class="auto-style35"></td>
                <td class="auto-style35"></td>
                <td class="auto-style35"></td>
                <td class="auto-style35"></td>
            </tr>
            <tr>
                <td class="auto-style34"></td>
                <td class="auto-style34"></td>
                <td class="auto-style34"></td>
                <td class="auto-style34"></td>
                <td class="auto-style34">
                    <h2 style="text-align: center; width: 481px;">Customer Enquiry Form</h2>
                </td>
                <td></td>
                <td></td>
                <td colspan="4">Become a Fan:<br />
                    <asp:HyperLink ID="facebook" runat="server" Height="20px" ImageHeight="20px" ImageUrl="~/image/facebook.png" ImageWidth="20px" NavigateUrl="https://www.facebook.com/ashlinbpg" Width="20px" TabIndex="38">go to Facebook</asp:HyperLink>
                    &nbsp;&nbsp;<asp:HyperLink ID="twitter" runat="server" Height="20px" ImageHeight="20px" ImageUrl="~/image/twitter.png" ImageWidth="20px" NavigateUrl="https://twitter.com/ashlinbpg" Width="20px" TabIndex="39">go to Twitter</asp:HyperLink>
                    &nbsp;&nbsp;<asp:HyperLink ID="pinterest" runat="server" Height="20px" ImageHeight="20px" ImageUrl="~/image/pinterest.png" ImageWidth="20px" NavigateUrl="https://www.pinterest.com/ashlinbpg/" Width="20px" TabIndex="40">go to Pinterest</asp:HyperLink>
                    &nbsp;&nbsp;<asp:HyperLink ID="linkedin" runat="server" Height="20px" ImageHeight="20px" ImageUrl="~/image/linkedin.png" ImageWidth="20px" NavigateUrl="https://www.linkedin.com/company/ashlin-bpg-marketing" Width="20px" TabIndex="41">go to Linkedin</asp:HyperLink>
                </td>
                <td class="auto-style35"></td>
                <td class="auto-style35"></td>
            </tr>
            <tr>
                <td class="auto-style34"></td>
                <td class="auto-style34"></td>
                <td class="auto-style34"></td>
                <td class="auto-style34"></td>
                <td class="auto-style34"></td>
                <td class="auto-style35"></td>
                <td class="auto-style35"></td>
                <td class="auto-style35"></td>
                <td class="auto-style35"></td>
                <td class="auto-style35"></td>
                <td class="auto-style35"></td>
                <td class="auto-style35"></td>
                <td class="auto-style35"></td>
            </tr>
        </table>
    
    </div>
        <asp:Panel ID="customerPanel" runat="server" BackImageUrl="image/wallpaper.jpg">
            <table style="width: 100%">
                <tr>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style38"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style91" rowspan="2">
                        <asp:ImageButton ID="wizardButton" runat="server" CssClass="wizard" Height="40px" ImageUrl="~/image/wizard.png" ToolTip="use Wizard" Width="33px" TabIndex="42" />
                    </td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                </tr>
                <tr>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td colspan="2">
                        <h4><span style="color: #FFFFFF;">Customer Information Result :</span></h4>
                    </td>
                    <td class="auto-style38"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                </tr>
                <tr>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style82">
                        <span style="color: #FFFFFF;">First Name :</span></td>
                    <td>
                        <asp:TextBox ID="firstNameTextbox" runat="server" Enabled="False" TabIndex="43" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style38"><span style="color: #FFFFFF;">*</span></td>
                    <td class="auto-style91"><span style="color: #FFFFFF;">Address 1 :</span></td>
                    <td class="auto-style18" rowspan="2">
                        <asp:TextBox ID="address1Textbox" runat="server" Enabled="False" Height="40px" style="overflow: auto" TabIndex="48" TextMode="MultiLine" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style18"><span style="color: #FFFFFF;">*</span></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                </tr>
                <tr>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style82"><span style="color: #FFFFFF;">Last Name:</span></td>
                    <td class="auto-style81">
                        <asp:TextBox ID="lastNameTextbox" runat="server" Enabled="False" TabIndex="44" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style38"><span style="color: #FFFFFF;">*</span></td>
                    <td class="auto-style91"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                </tr>
                <tr>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style82"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style38"></td>
                    <td class="auto-style91"><span style="color: #FFFFFF;">Address 2 :</span></td>
                    <td class="auto-style18" rowspan="2">
                        <asp:TextBox ID="address2Textbox" runat="server" Enabled="False" Height="40px" style="overflow: auto" TabIndex="49" TextMode="MultiLine" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                </tr>
                <tr>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style82"><span style="color: #FFFFFF;">Phone Number :</span></td>
                    <td class="auto-style81">
                        <asp:TextBox ID="phoneTextbox" runat="server" Enabled="False" TabIndex="45" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style38"></td>
                    <td class="auto-style91"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                </tr>
                <tr>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style82"><span style="color: #FFFFFF;">Email Address :</span></td>
                    <td class="auto-style81">
                        <asp:TextBox ID="emailTextbox" runat="server" Enabled="False" TabIndex="46" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style38"></td>
                    <td class="auto-style91"><span style="color: #FFFFFF;">City :</span></td>
                    <td class="auto-style18">
                        <asp:TextBox ID="cityTextbox" runat="server" Enabled="False" TabIndex="50" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style18"><span style="color: #FFFFFF;">*</span></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                </tr>
                <tr>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style38"></td>
                    <td class="auto-style91"><span style="color: #FFFFFF;">Province / State :</span></td>
                    <td class="auto-style18">
                        <asp:TextBox ID="provinceTextbox" runat="server" Enabled="False" Width="151px" TabIndex="51"></asp:TextBox>
                    </td>
                    <td class="auto-style18"><span style="color: #FFFFFF;">*</span></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                </tr>
                <tr>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style82"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style38"></td>
                    <td class="auto-style91"><span style="color: #FFFFFF;">Postal Code :</span></td>
                    <td class="auto-style18">
                        <asp:TextBox ID="postalCodeTextbox" runat="server" Enabled="False" Width="151px" TabIndex="52"></asp:TextBox>
                    </td>
                    <td class="auto-style18"><span style="color: #FFFFFF;">*</span></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                </tr>
                <tr>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style82"><span style="color: #FFFFFF;">Company Name :</span></td>
                    <td class="auto-style81">
                        <asp:TextBox ID="companyTextbox" runat="server" Enabled="False" TabIndex="47" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style38"></td>
                    <td class="auto-style91"><span style="color: #FFFFFF;">Country :</span></td>
                    <td class="auto-style18">
                        <asp:TextBox ID="countryTextbox" runat="server" Enabled="False" Width="151px" TabIndex="53"></asp:TextBox>
                    </td>
                    <td class="auto-style18"><span style="color: #FFFFFF;">*</span></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                </tr>
                <tr>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style82"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style38"></td>
                    <td class="auto-style91"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                </tr>
                <tr>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style38"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                </tr>
                <tr>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style81"></td>
                    <td class="auto-style38"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                    <td class="auto-style18"></td>
                </tr>
            </table>
            
        </asp:Panel>
        <div class="auto-style99">
            <table style="width: 100%; height: 37px;">
                <tr>
                    <td class="auto-style99"></td>
                    <td style="width: 231px;height: 35px;">
                        <asp:RadioButton ID="disableRadioButton" runat="server" AutoPostBack="True" Checked="True" Font-Overline="False" Font-Size="Small" 
                            OnCheckedChanged="disableRadioButton_CheckedChanged" GroupName="enableDisableRadioButtonGroup" TabIndex="54" Text="Disable Editing" />
&nbsp;
                        <asp:RadioButton ID="enableRadioButton" runat="server" AutoPostBack="True" Font-Size="Small" 
                            OnCheckedChanged="enableRadioButton_CheckedChanged" GroupName="enableDisableRadioButtonGroup" TabIndex="55" Text="Enable Editing" />
                    </td>
                    <td class="auto-style99"></td>
                </tr>
            </table>
            <asp:Panel ID="orderPanel" runat="server" BackImageUrl="image/wallpaper.jpg" style="background-position: center bottom">
                <table class="auto-style5">
                    <tr>
                        <td class="auto-style34"></td>
                        <td class="auto-style34"></td>
                        <td class="auto-style74" colspan="2"></td>
                        <td class="auto-style38"></td>
                        <td></td>
                        <td></td>
                        <td class="auto-style38"></td>
                        <td></td>
                        <td></td>
                        <td class="auto-style50"></td>
                        <td class="auto-style50"></td>
                    </tr>
                    <tr>
                        <td class="auto-style34"></td>
                        <td class="auto-style34"></td>
                        <td class="auto-style74" colspan="2">
                            <h4><span style="color: #ffffff"><strong>Order Information:</strong></span></h4>
                        </td>
                        <td class="auto-style38"></td>
                        <td></td>
                        <td></td>
                        <td class="auto-style38"></td>
                        <td></td>
                        <td></td>
                        <td class="auto-style50"></td>
                        <td class="auto-style50"></td>
                    </tr>
                    <tr>
                        <td class="auto-style40"></td>
                        <td class="auto-style40"></td>
                        <td style="height: 23px; text-align: right;font-size: medium;color: #FFFFFF">Item # :</td>
                        <td style="font-size: small;height: 23px;width: 121px">
                            <asp:DropDownList ID="skuDropdownlist" runat="server" AutoPostBack="True" TabIndex="56" Width="151px">
                            </asp:DropDownList>
                        </td>
                        <td class="auto-style41"></td>
                        <td style="height: 23px;text-align: right"><span style="color: #ffffff">Rush Order :</span></td>
                        <td class="auto-style103">
                            <asp:CheckBoxList ID="rushCheckboxList" runat="server" CellPadding="0" CellSpacing="0" Font-Size="Small" ForeColor="White" Height="20px" RepeatDirection="Horizontal" TabIndex="57" Width="151px">
                                <asp:ListItem Value="true">Yes</asp:ListItem>
                                <asp:ListItem Value="false">No</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                        <td class="auto-style41"></td>
                        <td style="height: 23px;text-align: right">
                            <span style="color: #ffffff">With Logo :</span></td>
                        <td class="auto-style103">
                            <asp:CheckBoxList ID="logoCheckboxList" runat="server" CellPadding="0" CellSpacing="0" Font-Size="Small" Height="20px" RepeatDirection="Horizontal" Width="151px" TabIndex="58" ForeColor="White">
                                <asp:ListItem Value="true">Yes</asp:ListItem>
                                <asp:ListItem Value="false">No</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                        <td class="auto-style63"></td>
                        <td class="auto-style63"></td>
                    </tr>
                    <tr>
                        <td class="auto-style40"></td>
                        <td class="auto-style40"></td>
                        <td class="auto-style45"></td>
                        <td class="auto-style40">
                            <asp:Button ID="addButton" runat="server" BackColor="Green" Font-Bold="True" ForeColor="White" TabIndex="60" Text="Add" Width="75px" OnClick="addButton_Click" />
                        </td>
                        <td class="auto-style41"></td>
                        <td class="auto-style48"><span style="color: #ffffff">Quantity :</span></td>
                        <td class="auto-style103" colspan="4">
                            <asp:CheckBoxList ID="quantityCheckboxList" runat="server" CellPadding="0" CellSpacing="0" Font-Size="Small" ForeColor="White" Height="19px" RepeatDirection="Horizontal" RepeatLayout="Flow" TabIndex="59" Width="350px">
                                <asp:ListItem Selected="True">1</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>24</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>
                                <asp:ListItem>250</asp:ListItem>
                                <asp:ListItem>500</asp:ListItem>
                                <asp:ListItem>1000</asp:ListItem>
                                <asp:ListItem>2500</asp:ListItem>
                            </asp:CheckBoxList>
                            </td>
                        <td class="auto-style63"></td>
                        <td class="auto-style63"></td>
                    </tr>
                    <tr>
                        <td class="auto-style40"></td>
                        <td class="auto-style40"></td>
                        <td style=" width: 121px;text-align: left;height: 23px"><span style="color: #ffffff">*</span></td>
                        <td class="auto-style40"></td>
                        <td class="auto-style41"></td>
                        <td class="auto-style48"></td>
                        <td class="auto-style103" colspan="4"></td>
                        <td class="auto-style63"></td>
                        <td class="auto-style63"></td>
                    </tr>
                    <tr>
                        <td class="auto-style40"></td>
                        <td class="auto-style40"></td>
                        <td class="auto-style64" colspan="8">
                            <asp:GridView ID="gridview" runat="server" BackColor="LightGoldenrodYellow" ForeColor="Black" BorderWidth="1px" CellPadding="2" BorderColor="Tan" GridLines="None" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" TabIndex="61" OnRowEditing="gridview_RowEditing" OnRowCancelingEdit="gridview_RowCancelingEdit" OnRowDeleting="gridview_RowDeleting" OnRowUpdating="gridview_RowUpdating">
                                <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                <Columns>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="editLinkButton" runat="server" CausesValidation="false" CommandName="Edit" Text="Edit" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="updateLinkButton" runat="server" CommandName="Update" Text="Update"  />
                                            <asp:LinkButton ID="cancelLinkButton" runat="server" CommandName="Cancel" Text="Cancel"  />
                                         </EditItemTemplate>
                                        <ControlStyle ForeColor="Green" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SKU" HeaderText="SKU" ReadOnly="True" ItemStyle-Width="200px" >
                                    <ItemStyle Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Short Description" HeaderText="Short Description" ReadOnly="True" />
                                    <asp:CheckBoxField DataField="Gift Box" HeaderText="Gift Box" ReadOnly="True" ItemStyle-Width="100px" >
                                    <ItemStyle Width="100px" />
                                    </asp:CheckBoxField>
                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" ItemStyle-Width="100px" >
                                    <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Base Price" HeaderText="Base Price" ReadOnly="True" ItemStyle-Width="100px" >
                                    <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Pricing Tier" HeaderText="Pricing Tier" ReadOnly="True" SortExpression="Pricing Tier" ItemStyle-Width="100px"/>
                                    <asp:ButtonField Text="Remove" CommandName="Delete" ControlStyle-ForeColor="Red" >
                                    <ControlStyle ForeColor="Red" />
                                    </asp:ButtonField>
                                </Columns>
                                <FooterStyle BackColor="Tan" />
                                <HeaderStyle BackColor="Tan" Font-Bold="True" />
                                <PagerStyle ForeColor="DarkSlateBlue" HorizontalAlign="Center" BackColor="PaleGoldenrod" />
                                <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                <SortedAscendingCellStyle BackColor="#FAFAE7" />
                                <SortedAscendingHeaderStyle BackColor="#DAC09E" />
                                <SortedDescendingCellStyle BackColor="#E1DB9C" />
                                <SortedDescendingHeaderStyle BackColor="#C2A47B" />
                            </asp:GridView>
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td class="auto-style40"></td>
                        <td class="auto-style40"></td>
                        <td class="auto-style64"></td>
                        <td class="auto-style64"></td>
                        <td class="auto-style64"></td>
                        <td class="auto-style64"></td>
                        <td class="auto-style64"></td>
                        <td class="auto-style64"></td>
                        <td class="auto-style64"></td>
                        <td class="auto-style64" colspan="3" rowspan="7">
                            <asp:Calendar ID="calendar" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" OnDayRender="calendar_DayRender" OnSelectionChanged="calendar_SelectionChanged" TabIndex="65" Visible="False" Width="200px">
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
                        <td class="auto-style65"></td>
                        <td class="auto-style65"></td>
                        <td class="auto-style65"></td>
                        <td class="auto-style65"></td>
                        <td class="auto-style66"></td>
                        <td class="auto-style67"></td>
                        <td style="width: 122px;height: 24px"></td>
                        <td class="auto-style66"></td>
                        <td class="auto-style67"></td>
                    </tr>
                    <tr>
                        <td class="auto-style40"></td>
                        <td class="auto-style40"></td>
                        <td class="auto-style45"><span style="color: #ffffff">Additional Info :</span></td>
                        <td colspan="4" rowspan="3">
                            <asp:TextBox ID="additionalInfoTextbox" runat="server" Height="66px" TextMode="MultiLine" Width="445px" TabIndex="61" style="overflow: auto"></asp:TextBox>
                        </td>
                        <td class="auto-style103" colspan="2"><span style="color: #ffffff">Date of&nbsp; Delivery :</span></td>
                    </tr>
                    <tr>
                        <td class="auto-style40"></td>
                        <td class="auto-style40"></td>
                        <td class="auto-style45"></td>
                        <td class="auto-style103" colspan="2">
                            <asp:TextBox ID="dateDeliveryTextbox" runat="server" TabIndex="63" Width="106px"></asp:TextBox>
                            <asp:Button ID="dateDeliveryButton" runat="server" OnClick="dateEventButton_Click" TabIndex="64" Text="..." Width="37px" />
                            <span style="color: #ffffff">&nbsp;*</span></td>
                    </tr>
                    <tr>
                        <td class="auto-style34"></td>
                        <td class="auto-style34"></td>
                        <td class="auto-style34"></td>
                        <td class="auto-style38"></td>
                        <td class="auto-style35"></td>
                    </tr>
                    <tr>
                        <td class="auto-style40"></td>
                        <td class="auto-style40"></td>
                        <td class="auto-style40"></td>
                        <td class="auto-style40"></td>
                        <td class="auto-style41"></td>
                        <td class="auto-style96"></td>
                        <td class="auto-style96"></td>
                        <td class="auto-style41"></td>
                        <td style="width: 122px;height: 23px"></td>
                    </tr>
                    <tr>
                        <td class="auto-style34"></td>
                        <td class="auto-style34"></td>
                        <td class="auto-style34"></td>
                        <td class="auto-style34"></td>
                        <td class="auto-style38"></td>
                        <td class="auto-style64" colspan="2"></td>
                        <td class="auto-style38"></td>
                        <td class="auto-style35"></td>
                    </tr>
                    <tr>
                        <td class="auto-style249"></td>
                        <td class="auto-style249"></td>
                        <td class="auto-style249"></td>
                        <td class="auto-style249"></td>
                        <td class="auto-style250"></td>
                        <td style="height: 60px;text-align: center" colspan="2"></td>
                        <td class="auto-style250"></td>
                        <td style="width: 122px;height: 60px"></td>
                        <td style="width: 151px;overflow: auto;height: 60px"></td>
                        <td style="height: 60px" colspan="2"></td>
                    </tr>
                    <tr>
                        <td class="auto-style34"></td>
                        <td class="auto-style34"></td>
                        <td class="auto-style34"></td>
                        <td class="auto-style34"></td>
                        <td class="auto-style38"></td>
                        <td class="auto-style64" colspan="2"><asp:Label ID="newQuoteLabel" runat="server" Font-Size="Small" ForeColor="White" TabIndex="67" Text="New Quote Created !" Visible="False"></asp:Label>
                        </td>
                        <td class="auto-style38"></td>
                        <td class="auto-style35"></td>
                        <td class="auto-style39"></td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td class="auto-style34"></td>
                        <td class="auto-style34"></td>
                        <td class="auto-style34"></td>
                        <td class="auto-style34"></td>
                        <td class="auto-style38"></td>
                        <td class="auto-style64" colspan="2" rowspan="3">&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="quoteButton" runat="server" CssClass="quote" ImageUrl="~/image/quote.png" TabIndex="68" Width="50px" OnClick="quoteButton_Click" />
                            <em><span style="color: #ffffff"><strong>Quote</strong></span></em></td>
                        <td class="auto-style38"></td>
                        <td class="auto-style35"></td>
                        <td class="auto-style39"></td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td class="auto-style34">
                            <asp:Label ID="uselessLabel" runat="server" style="display: none"></asp:Label>
                        </td>
                        <td class="auto-style34"></td>
                        <td class="auto-style218" colspan="2">
                            &nbsp;</td>
                        <td class="auto-style38"></td>
                        <td class="auto-style38"></td>
                        <td class="auto-style35"></td>
                        <td class="auto-style39"></td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td class="auto-style34"></td>
                        <td class="auto-style34"></td>
                        <td class="auto-style218" colspan="2">
                            <asp:LinkButton ID="updateLinkButton" runat="server" Font-Size="Small" ForeColor="White" OnClick="updateLinkButton_Click" TabIndex="66">update Username and Password</asp:LinkButton>
                        </td>
                        <td class="auto-style38"></td>
                        <td class="auto-style38"></td>
                        <td class="auto-style35"></td>
                        <td class="auto-style39"></td>
                        <td colspan="2"></td>
                    </tr>
                </table>
                
            </asp:Panel>
            <asp:Panel ID="welcomePanel" runat="server" Height="280px" Width="500px" BackColor="White">
                <table class="auto-style111">
                    <tr>
                        <td class="auto-style64" colspan="2" style="background-color: tan"><strong><span class="auto-style106">Welcome To</span><br class="auto-style106"/> <span class="auto-style106">Ashlin Customer Enquiry Form</span></strong></td>
                    </tr>
                    <tr>
                        <td style="width: 293px;height: 23px"></td>
                        <td class="auto-style127"></td>
                    </tr>
                    <tr>
                        <td style="width: 293px;height: 23px;text-align: right">Do you want to use <strong>Look Up Wizard</strong> ?</td>
                        <td class="auto-style127">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
                            <asp:Button ID="yesButton" runat="server" BackColor="Green" Font-Bold="True" ForeColor="White" Height="23px" Text="Yes" Width="50px" OnClick="yesButton_Click" TabIndex="1" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="welcomeNoButton" runat="server" BackColor="Red" Font-Bold="True" ForeColor="White" Height="23px" Text="No" Width="50px" TabIndex="2" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;height: 105px" colspan="2">
                            <asp:Image ID="welcomeWizardImage" runat="server" Height="70px" ImageUrl="~/image/wizard-black.png" Width="60px" TabIndex="3" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style128" colspan="2">
                            <asp:Button ID="welcomeSkipButton" runat="server" Text="Skip" Width="70px" OnClick="welcomeSkipButton_Click" TabIndex="34" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="asiPanel" runat="server" DefaultButton="asiNextButton" BackColor="White" Height="280px" Width="500px" TabIndex="5">
                <table class="auto-style111">
                    <tr>
                        <td colspan="2" class="auto-style132"><strong>Please Enter Your ASI Number</strong></td>
                    </tr>
                    <tr>
                        <td class="auto-style148"></td>
                        <td class="auto-style144"></td>
                    </tr>
                    <tr>
                        <td class="auto-style147"><strong>ASI # :</strong></td>
                        <td class="auto-style150">
                            <asp:TextBox ID="asiTextbox" runat="server" Width="178px" TabIndex="6"></asp:TextBox>
                            <asp:Button ID="asiNextButton" runat="server" BackColor="Green" Font-Bold="True" ForeColor="White" Height="23px" Text="Next" Width="60px" TabIndex="7" OnClick="asiNextButton_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style148"></td>
                        <td class="auto-style149"></td>
                    </tr>
                    <tr>
                        <td class="auto-style153"></td>
                        <td class="auto-style154">
                            <em>System powered by ASI<span> ®</span></em></td>
                    </tr>
                    <tr>
                        <td class="auto-style151"></td>
                        <td class="auto-style152"></td>
                    </tr>
                    <tr>
                        <td class="auto-style128" colspan="2">
                            <asp:Button ID="asiCancelButton" runat="server" BackColor="Red" Font-Bold="True" ForeColor="White" Height="23px" Text="Cancel" Width="60px" TabIndex="8" />
                            &nbsp;&nbsp;
                            <asp:Button ID="asiSkipButton" runat="server" Text="Skip" Width="70px" OnClick="asiSkipButton_Click" TabIndex="9" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="searchPanel" runat="server" DefaultButton="searchButton" BackColor="White" Height="280px" Width="500px">
                <table class="auto-style111">
                    <tr>
                        <td class="auto-style160" colspan="3"><strong>Please Enter Information
                            <br />
                            About the Customer</strong></td>
                    </tr>
                    <tr>
                        <td style="height: 33px" colspan="2"><strong>Customer Information :</strong></td>
                        <td class="auto-style191">
                            <asp:Label ID="tooManyResultLabel" runat="server" Font-Size="11pt" ForeColor="Red" Text="Too many results found" Visible="False" Font-Bold="True" TabIndex="10"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style186">First Name :</td>
                        <td class="auto-style187">
                            <asp:TextBox ID="searchFirstNameTextbox" runat="server" Width="155px" TabIndex="11"></asp:TextBox>
                        </td>
                        <td class="auto-style199"></td>
                    </tr>
                    <tr>
                        <td class="auto-style186">Last Name :</td>
                        <td class="auto-style187">
                            <asp:TextBox ID="searchLastNameTextbox" runat="server" Width="155px" TabIndex="12"></asp:TextBox>
                        </td>
                        <td class="auto-style144" rowspan="2">
                            <asp:ImageButton ID="searchButton" runat="server" ImageUrl="~/image/search.png" CssClass="search" Width="50px" OnClick="searchButton_Click" TabIndex="15"/>
                            <em><strong>Search</strong></em></td>
                    </tr>
                    <tr>
                        <td class="auto-style186">Company Name :</td>
                        <td class="auto-style187">
                            <asp:TextBox ID="searchCompanyTextbox" runat="server" Width="155px" TabIndex="13"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style186">Email :</td>
                        <td class="auto-style187">
                            <asp:TextBox ID="searchEmailTextbox" runat="server" Width="155px" TabIndex="14"></asp:TextBox>
                        </td>
                        <td class="auto-style199"></td>
                    </tr>
                    <tr>
                        <td class="auto-style128" colspan="3">
                            <asp:Button ID="searchCancelButton" runat="server" BackColor="Red" Font-Bold="True" ForeColor="White" Height="23px" Text="Cancel" Width="60px" TabIndex="16" />
                            &nbsp;&nbsp;
                            <asp:Button ID="searchBackButton" runat="server" OnClick="searchBackButton_Click" Text="Back" Width="70px" TabIndex="17" />
                        </td>
                    </tr>
                </table>
                
            </asp:Panel>
            <asp:Panel ID="resultPanel" runat="server" Height="325px" Width="550px" BackColor="White" TabIndex="18">
                <table class="auto-style174">
                    <tr>
                        <td class="auto-style170" colspan="2"><strong>Customer Search Result</strong></td>
                    </tr>
                    <tr>
                        <td class="auto-style179" rowspan="8">
                            <asp:ListBox ID="listbox" runat="server" Height="185px" Width="210px" AutoPostBack="True" OnSelectedIndexChanged="listbox_SelectedIndexChanged" TabIndex="19"></asp:ListBox>
                        </td>
                        <td class="auto-style195">
                            <asp:Button ID="resultSelectButton" runat="server" BackColor="Green" Font-Bold="True" ForeColor="White" Height="23px" OnClick="resultSelectButton_Click" Text="Select" Width="70px" TabIndex="20" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style177">Company Name :</td>
                    </tr>
                    <tr>
                        <td class="auto-style177">
                            <asp:TextBox ID="resultCompanyTextbox" runat="server" Width="250px" TabIndex="21"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style200">Email :</td>
                    </tr>
                    <tr>
                        <td class="auto-style177">
                            <asp:TextBox ID="resultEmailTextbox" runat="server" Width="250px" TabIndex="22"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style200">Phone :</td>
                    </tr>
                    <tr>
                        <td class="auto-style177">
                            <asp:TextBox ID="resultPhoneTextbox" runat="server" Width="250px" TabIndex="23"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style194"></td>
                    </tr>
                    <tr>
                        <td class="auto-style192" colspan="2">
                            <asp:Button ID="enterManuallyButton" runat="server" BackColor="Red" Font-Bold="True" ForeColor="White" Text="Enter Info Manually" TabIndex="24" />
                            &nbsp;&nbsp;
                            <asp:Button ID="resultBackButton" runat="server" Text="Back" Width="70px" OnClick="resultBackButton_Click" TabIndex="25" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="loginPanel" runat="server" DefaultButton="loginButton" Height="170px" Width="350px" BackColor="White" TabIndex="26">
                <table style="width: 350px;height: 170px;border-style: solid;border-width: 3px">
                    <tr>
                        <td colspan="2" style="background-color: tan"><b>Please Login To Continue</b></td>
                    </tr>
                    <tr>
                        <td colspan="2" style="height: 20px"></td>
                    </tr>
                    <tr>
                        <td style="width: 103px;text-align: right;height: 23px">Username :</td>
                        <td style="width: 174px;height: 23px">
                            <asp:TextBox ID="usernameTextbox" runat="server" Width="170px" TabIndex="27"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style=" width: 103px;text-align: right">Password :</td>
                        <td style="width: 174px">
                            <asp:TextBox ID="passwordTextbox" TextMode="Password" runat="server" Width="170px" TabIndex="28"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 43px;text-align: center" colspan="2">
                            <asp:Button ID="loginCancelButton" runat="server" BackColor="Red" Font-Bold="True" ForeColor="White" Height="23px" Text="Cancel" Width="60px" TabIndex="29" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="loginButton" runat="server" BackColor="Green" Font-Bold="True" ForeColor="White" Height="23px" Text="Login" Width="60px" OnClick="loginButton_Click" TabIndex="30" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"  style="background-color: tan"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="updatePanel" runat="server" DefaultButton="updateButton" BackColor="White" Height="170px" Width="350px" TabIndex="31">
                <table style="width: 350px;height: 170px;border-style: solid;border-width: 3px">
                    <tr>
                        <td colspan="2" style="background-color: tan"><b>Enter New Credentials</b></td>
                    </tr>
                    <tr>
                        <td class="auto-style236">New Username :</td>
                        <td style="height: 23px;width: 171px;text-align: left">
                            <asp:TextBox ID="newUsernameTextbox" runat="server" Width="170px" TabIndex="32"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style236">New Password :</td>
                        <td class="auto-style235">
                            <asp:TextBox ID="newPasswordTextbox" runat="server" TextMode="Password" Width="170px" TabIndex="33"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style236">Enter Again :</td>
                        <td class="auto-style235">
                            <asp:TextBox ID="enterAgainTextbox" runat="server" TextMode="Password" Width="170px" TabIndex="34"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 40px;text-align: center" colspan="2">
                            <asp:Button ID="updateCancelButton" runat="server" BackColor="Red" Font-Bold="True" ForeColor="White" Height="23px" Text="Cancel" Width="60px" TabIndex="35" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="updateButton" runat="server" BackColor="Green" Font-Bold="True" ForeColor="White" Height="23px" Text="Update" Width="60px" OnClick="updateButton_Click" TabIndex="36" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="background-color: tan"></td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
        </div>      
        <ajax:ModalPopupExtender ID="welcomePopup" runat="server" TargetControlID="uselessLabel" PopupControlID="welcomePanel" CancelControlID="welcomeNoButton"
            BackgroundCssClass="modalBackground"/>
        <ajax:ModalPopupExtender ID="asiPopup" runat="server" TargetControlID="wizardButton" PopupControlID="asiPanel" CancelControlID="asiCancelButton"
            BackgroundCssClass="modalBackground"/>
        <ajax:ModalPopupExtender ID="searchPopup" runat="server" TargetControlID="uselessLabel" PopupControlID="searchPanel" CancelControlID="searchCancelButton"
            BackgroundCssClass="modalBackground"/>
        <ajax:ModalPopupExtender ID="resultPopup" runat="server" TargetControlID="uselessLabel" PopupControlID="resultPanel" CancelControlID="enterManuallyButton"
            BackgroundCssClass="modalBackground"/>
        <ajax:ModalPopupExtender ID="loginPopup" runat="server" TargetControlID="uselessLabel" PopupControlID="loginPanel" CancelControlID="loginCancelButton"
            BackgroundCssClass="modalBackground"/>
        <ajax:ModalPopupExtender ID="updatePopup" runat="server" TargetControlID="uselessLabel" PopupControlID="updatePanel" CancelControlID="updateCancelButton"
            BackgroundCssClass="modalBackground"/>
    </form>
</body>
</html>
