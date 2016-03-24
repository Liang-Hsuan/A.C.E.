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
        .auto-style42 {
            width: 122px;
            height: 23px;
        }
        .auto-style43 {
            width: 151px;
            overflow: auto;
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
        .auto-style68 {
            width: 122px;
            height: 24px;
        }
        .auto-style69 {
            width: 151px;
            overflow: auto;
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
        .auto-style101 {
            font-size: medium;
            height: 23px;
            text-align: right;
        }
        .auto-style103 {
            height: 23px;
        }
        .auto-style104 {
            font-size: small;
            height: 23px;
            width: 121px;
        }
        .auto-style105 {
            height: 23px;
            text-align: right;
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
        .auto-style125 {
            width: 293px;
            height: 23px;
        }
        .auto-style126 {
            width: 293px;
            height: 23px;
            text-align: right;
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
        .auto-style131 {
            text-align: center;
            height: 105px;
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
        .auto-style196 {
            height: 33px;
        }
        .auto-style199 {
            width: 249px;
            height: 23px;
        }
        .auto-style200 {
            width: 274px;
            height: 22px;
        }
        .auto-style205 {
            width: 174px;
            height: 23px;
        }
        .auto-style208 {
            width: 350px;
            height: 170px;
            border-style: solid; 
            border-width: 3px;
        }
        .auto-style211 {
            width: 174px;
        }
        .auto-style213 {
            width: 103px;
            text-align: right;
        }
        .auto-style214 {
            width: 103px;
            text-align: right;
            height: 23px;
        }
        .auto-style215 {
            height: 43px;
            text-align: center;
        }
        .auto-style216 {
            height: 20px;
        }
        .auto-style218 {
            text-align: right;
        }
        .auto-style227 {
            height: 40px;
            text-align: center;
        }
        .auto-style229 {
            height: 23px;
            width: 171px;
            text-align: left;
        }
        .auto-style231 {
            width: 350px;
            height: 170px;
            border-style: solid;
            border-width: 3px
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
        .auto-style237 {
            width: 121px;
            height: 40px;
        }
        .auto-style238 {
            width: 15px;
            height: 40px;
        }
        .auto-style240 {
            width: 122px;
            height: 51px;
        }
        .auto-style241 {
            width: 151px;
            overflow: auto;
            height: 51px;
        }
        .auto-style242 {
            height: 51px;
        }
        .auto-style243 {
            width: 121px;
            height: 51px;
        }
        .auto-style244 {
            width: 15px;
            height: 51px;
        }
        .auto-style245 {
            height: 51px;
            text-align: center;
        }
    </style>
</head>
<body style="height: 1012px; background-color:tan">
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager"/>
    <div>  
        <table style="width: 100%">
            <tr>
                <td class="auto-style34">&nbsp;</td>
                <td class="auto-style34">&nbsp;</td>
                <td class="auto-style34">&nbsp;</td>
                <td class="auto-style34">&nbsp;</td>
                <td class="auto-style34" rowspan="3">
                    <asp:HyperLink ID="logoLink" runat="server" ImageUrl="~/image/AshlinIcon.png" ImageWidth="140px" NavigateUrl="http://www.ashlinbpg.com/retail/" ToolTip="go to Ashlin website" TabIndex="31">go to Ashlin website</asp:HyperLink>
                </td>
                <td class="auto-style34" rowspan="3">&nbsp;</td>
                <td class="auto-style34" rowspan="3">&nbsp;</td>
                <td class="auto-style34">
                    <h1 style="text-align: center; font-size: 40px; width: 480px">Ashlin BPG Marketing Inc</h1>
                </td>
                <td class="auto-style35">&nbsp;</td>
                <td class="auto-style35">&nbsp;</td>
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
                <td class="auto-style34">
                    <h2 style="text-align: center; width: 481px;">Customer Enquiry Form</h2>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td colspan="4">Become a Fan:<br />
                    <asp:HyperLink ID="facebook" runat="server" Height="20px" ImageHeight="20px" ImageUrl="~/image/facebook.png" ImageWidth="20px" NavigateUrl="https://www.facebook.com/ashlinbpg" Width="20px" TabIndex="32">go to Facebook</asp:HyperLink>
                    &nbsp;&nbsp;<asp:HyperLink ID="twitter" runat="server" Height="20px" ImageHeight="20px" ImageUrl="~/image/twitter.png" ImageWidth="20px" NavigateUrl="https://twitter.com/ashlinbpg" Width="20px" TabIndex="33">go to Twitter</asp:HyperLink>
                    &nbsp;&nbsp;<asp:HyperLink ID="pinterest" runat="server" Height="20px" ImageHeight="20px" ImageUrl="~/image/pinterest.png" ImageWidth="20px" NavigateUrl="https://www.pinterest.com/ashlinbpg/" Width="20px" TabIndex="34">go to Pinterest</asp:HyperLink>
                    &nbsp;&nbsp;<asp:HyperLink ID="linkedin" runat="server" Height="20px" ImageHeight="20px" ImageUrl="~/image/linkedin.png" ImageWidth="20px" NavigateUrl="https://www.linkedin.com/company/ashlin-bpg-marketing" Width="20px" TabIndex="35">go to Linkedin</asp:HyperLink>
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
        <asp:Panel ID="customerPanel" runat="server" BackImageUrl="image/wallpaper.jpg">
            <table style="width: 100%">
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">
                        &nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style91" rowspan="2">
                        <asp:ImageButton ID="wizardButton" runat="server" CssClass="wizard" Height="40px" ImageUrl="~/image/wizard.png" ToolTip="use Wizard" Width="33px" TabIndex="36" />
                    </td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td colspan="2">
                        <h4><span style="color: #FFFFFF;">Customer Information Result :</span></h4>
                    </td>
                    <td class="auto-style38">
                        &nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">
                        &nbsp;</td>
                    <td class="auto-style18">
                        &nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style82">
                        <span style="color: #FFFFFF;">First Name :</span></td>
                    <td>
                        <asp:TextBox ID="firstNameTextbox" runat="server" Enabled="False" TabIndex="37" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style38"><span style="color: #FFFFFF;">*</span></td>
                    <td class="auto-style91"><span style="color: #FFFFFF;">Address 1 :</span></td>
                    <td class="auto-style18" rowspan="2">
                        <asp:TextBox ID="address1Textbox" runat="server" Enabled="False" Height="40px" style="overflow: auto" TabIndex="42" TextMode="MultiLine" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style18"><span style="color: #FFFFFF;">*</span></td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style82"><span style="color: #FFFFFF;">Last Name:</span></td>
                    <td class="auto-style81">
                        <asp:TextBox ID="lastNameTextbox" runat="server" Enabled="False" TabIndex="38" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style38"><span style="color: #FFFFFF;">*</span></td>
                    <td class="auto-style91">&nbsp;</td>
                    <td class="auto-style18">
                        &nbsp;</td>
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
                    <td class="auto-style91"><span style="color: #FFFFFF;">Address 2 :</span></td>
                    <td class="auto-style18" rowspan="2">
                        <asp:TextBox ID="address2Textbox" runat="server" Enabled="False" Height="40px" style="overflow: auto" TabIndex="43" TextMode="MultiLine" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style82"><span style="color: #FFFFFF;">Phone Number :</span></td>
                    <td class="auto-style81">
                        <asp:TextBox ID="phoneTextbox" runat="server" Enabled="False" TabIndex="39" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style91">&nbsp;</td>
                    <td class="auto-style18">
                        &nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style82"><span style="color: #FFFFFF;">Email Address :</span></td>
                    <td class="auto-style81">
                        <asp:TextBox ID="emailTextbox" runat="server" Enabled="False" TabIndex="40" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style91"><span style="color: #FFFFFF;">City :</span></td>
                    <td class="auto-style18">
                        <asp:TextBox ID="cityTextbox" runat="server" Enabled="False" TabIndex="44" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style18"><span style="color: #FFFFFF;">*</span></td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">
                        &nbsp;</td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style91"><span style="color: #FFFFFF;">Province / State :</span></td>
                    <td class="auto-style18">
                        <asp:TextBox ID="provinceTextbox" runat="server" Enabled="False" Width="151px" TabIndex="45"></asp:TextBox>
                    </td>
                    <td class="auto-style18"><span style="color: #FFFFFF;">*</span></td>
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
                    <td class="auto-style91"><span style="color: #FFFFFF;">Postal Code :</span></td>
                    <td class="auto-style18">
                        <asp:TextBox ID="postalCodeTextbox" runat="server" Enabled="False" Width="151px" TabIndex="46"></asp:TextBox>
                    </td>
                    <td class="auto-style18"><span style="color: #FFFFFF;">*</span></td>
                    <td class="auto-style18">&nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style81">&nbsp;</td>
                    <td class="auto-style82"><span style="color: #FFFFFF;">Company Name :</span></td>
                    <td class="auto-style81">
                        <asp:TextBox ID="companyTextbox" runat="server" Enabled="False" TabIndex="41" Width="151px"></asp:TextBox>
                    </td>
                    <td class="auto-style38">&nbsp;</td>
                    <td class="auto-style91"><span style="color: #FFFFFF;">Country :</span></td>
                    <td class="auto-style18">
                        <asp:TextBox ID="countryTextbox" runat="server" Enabled="False" Width="151px" TabIndex="47"></asp:TextBox>
                    </td>
                    <td class="auto-style18"><span style="color: #FFFFFF;">*</span></td>
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
                    <td class="auto-style91">&nbsp;</td>
                    <td class="auto-style18">
                        &nbsp;</td>
                    <td class="auto-style18">&nbsp;</td>
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
            </table>
            
        </asp:Panel>
        <div class="auto-style99">
            <table style="width: 100%; height: 37px;">
                <tr>
                    <td class="auto-style99"></td>
                    <td style="width: 231px;height: 35px;">
                        <asp:RadioButton ID="disableRadioButton" runat="server" AutoPostBack="True" Checked="True" Font-Overline="False" Font-Size="Small" 
                            OnCheckedChanged="disableRadioButton_CheckedChanged" GroupName="enableDisableRadioButtonGroup" TabIndex="48" Text="Disable Editing" />
&nbsp;
                        <asp:RadioButton ID="enableRadioButton" runat="server" AutoPostBack="True" Font-Size="Small" 
                            OnCheckedChanged="enableRadioButton_CheckedChanged" GroupName="enableDisableRadioButtonGroup" TabIndex="49" Text="Enable Editing" />
                    </td>
                    <td class="auto-style99"></td>
                </tr>
            </table>
            <asp:Panel ID="orderPanel" runat="server" BackImageUrl="image/wallpaper.jpg" style="background-position: center bottom">
                <table class="auto-style5">
                    <tr>
                        <td class="auto-style34">&nbsp;</td>
                        <td class="auto-style34">&nbsp;</td>
                        <td class="auto-style74" colspan="2">
                            &nbsp;</td>
                        <td class="auto-style38">&nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>&nbsp;</td>
                        <td class="auto-style38">&nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>&nbsp;</td>
                        <td class="auto-style50">&nbsp;</td>
                        <td class="auto-style50">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style34">&nbsp;</td>
                        <td class="auto-style34">&nbsp;</td>
                        <td class="auto-style74" colspan="2">
                            <h4><span style="color: #ffffff"><strong>Order Information:</strong></span></h4>
                        </td>
                        <td class="auto-style38">&nbsp;</td>
                        <td></td>
                        <td></td>
                        <td class="auto-style38">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td class="auto-style50">&nbsp;</td>
                        <td class="auto-style50">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style40"></td>
                        <td class="auto-style40"></td>
                        <td class="auto-style101"><span style="color: #ffffff">Rush Order :</span></td>
                        <td class="auto-style104">
                            <asp:CheckBoxList ID="rushCheckboxList" runat="server" Height="20px" Width="151px" CellPadding="0" CellSpacing="0" Font-Size="Small" RepeatDirection="Horizontal" TabIndex="50" ForeColor="White">
                                <asp:ListItem Value="true">Yes</asp:ListItem>
                                <asp:ListItem Value="false">No</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                        <td class="auto-style41">
                        </td>
                        <td class="auto-style103"></td>
                        <td class="auto-style103"></td>
                        <td class="auto-style41"></td>
                        <td class="auto-style105">
                            <span style="color: #ffffff">With Logo :</span></td>
                        <td class="auto-style103">
                            <asp:CheckBoxList ID="logoCheckboxList" runat="server" CellPadding="0" CellSpacing="0" Font-Size="Small" Height="20px" RepeatDirection="Horizontal" Width="151px" TabIndex="51" ForeColor="White">
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
                        <td class="auto-style45"><span style="color: #ffffff">Item # :</span></td>
                        <td class="auto-style40">
                            <asp:DropDownList ID="skuDropdownlist1" runat="server" Width="151px" OnSelectedIndexChanged="skuDropdownlist1_SelectedIndexChanged" AutoPostBack="True" TabIndex="52">
                            </asp:DropDownList>
                        </td>
                        <td class="auto-style41"><span style="color: #FFFFFF;">*</span></td>
                        <td class="auto-style48"><span style="color: #ffffff">Second Item # :</span></td>
                        <td class="auto-style42">
                            <asp:DropDownList ID="skuDropdownlist2" runat="server" Width="151px" OnSelectedIndexChanged="skuDropdownlist2_SelectedIndexChanged" AutoPostBack="True" TabIndex="53">
                            </asp:DropDownList>
                        </td>
                        <td class="auto-style41"></td>
                        <td class="auto-style48"><span style="color: #ffffff">Third Item # :</span></td>
                        <td class="auto-style43">
                            <asp:DropDownList ID="skuDropdownlist3" runat="server" AutoPostBack="True" TabIndex="54" Width="151px" OnSelectedIndexChanged="skuDropdownlist3_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="auto-style63">&nbsp;</td>
                        <td class="auto-style63"></td>
                    </tr>
                    <tr>
                        <td class="auto-style40"></td>
                        <td class="auto-style40"></td>
                        <td class="auto-style45">&nbsp;</td>
                        <td class="auto-style40">
                            <asp:TextBox ID="shortDescriptionTextbox1" runat="server" Enabled="False" Height="20px" style="overflow: auto" TabIndex="55" TextMode="MultiLine" Width="151px"></asp:TextBox>
                        </td>
                        <td class="auto-style41"></td>
                        <td class="auto-style48"></td>
                        <td class="auto-style42">
                            <asp:TextBox ID="shortDescriptionTextbox2" runat="server" Enabled="False" Height="20px" style="overflow: auto" TabIndex="56" TextMode="MultiLine" Width="151px"></asp:TextBox>
                        </td>
                        <td class="auto-style41"></td>
                        <td class="auto-style48">&nbsp;</td>
                        <td class="auto-style43">
                            <asp:TextBox ID="shortDescriptionTextbox3" runat="server" TabIndex="57" Width="151px" Enabled="False" Height="20px" style="overflow: auto" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style40">&nbsp;</td>
                        <td class="auto-style40">&nbsp;</td>
                        <td class="auto-style45"><span style="color: #ffffff">Quantity :</span></td>
                        <td class="auto-style103" colspan="4">
                            <asp:CheckBoxList ID="quantityCheckboxList" runat="server" CellPadding="0" CellSpacing="0" Font-Size="Small" Height="19px" RepeatDirection="Horizontal" RepeatLayout="Flow" Width="350px" TabIndex="58" ForeColor="White">
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
                            <span style="color: #FFFFFF;">*</span></td>
                        <td class="auto-style41">&nbsp;</td>
                        <td class="auto-style48"><span style="color: #ffffff">Date for Event :</span></td>
                        <td class="auto-style43">
                            <asp:TextBox ID="dateEventTextbox" runat="server" TabIndex="59" Width="106px"></asp:TextBox>
                            <asp:Button ID="dateEventButton" runat="server" OnClick="dateEventButton_Click" Text="..." Width="37px" TabIndex="60" />
                        </td>
                        <td colspan="2" rowspan="7">
                            <asp:Calendar ID="calendar" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" OnDayRender="calendar_DayRender" OnSelectionChanged="calendar_SelectionChanged" TabIndex="61" Visible="False" Width="200px">
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
                        <td class="auto-style65">&nbsp;</td>
                        <td class="auto-style65">
                        </td>
                        <td class="auto-style66"></td>
                        <td class="auto-style67">&nbsp;</td>
                        <td class="auto-style68">
                            &nbsp;</td>
                        <td class="auto-style66"></td>
                        <td class="auto-style67">&nbsp;</td>
                        <td class="auto-style69">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style40"></td>
                        <td class="auto-style40"></td>
                        <td class="auto-style45"><span style="color: #ffffff">Additional Info :</span></td>
                        <td colspan="4" rowspan="3">
                            <asp:TextBox ID="additionalInfoTextbox" runat="server" Height="66px" style="overflow: auto" TextMode="MultiLine" Width="400px" TabIndex="62"></asp:TextBox>
                        </td>
                        <td class="auto-style41"></td>
                        <td class="auto-style48">&nbsp;</td>
                        <td class="auto-style43">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style40"></td>
                        <td class="auto-style40"></td>
                        <td class="auto-style45">&nbsp;</td>
                        <td class="auto-style41"></td>
                        <td class="auto-style48">&nbsp;</td>
                        <td class="auto-style43">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style34">&nbsp;</td>
                        <td class="auto-style34">&nbsp;</td>
                        <td class="auto-style34">&nbsp;</td>
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
                        <td class="auto-style96">&nbsp;</td>
                        <td class="auto-style96">&nbsp;</td>
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
                        <td class="auto-style64" colspan="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td class="auto-style38">&nbsp;</td>
                        <td class="auto-style35">&nbsp;</td>
                        <td class="auto-style39">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style243"></td>
                        <td class="auto-style243"></td>
                        <td class="auto-style243"></td>
                        <td class="auto-style243"></td>
                        <td class="auto-style244"></td>
                        <td class="auto-style245" colspan="2"></td>
                        <td class="auto-style244"></td>
                        <td class="auto-style240"></td>
                        <td class="auto-style241"></td>
                        <td class="auto-style242" colspan="2"></td>
                    </tr>
                    <tr>
                        <td class="auto-style34">&nbsp;</td>
                        <td class="auto-style34">&nbsp;</td>
                        <td class="auto-style34">&nbsp;</td>
                        <td class="auto-style34">&nbsp;</td>
                        <td class="auto-style38">&nbsp;</td>
                        <td class="auto-style64" colspan="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="newQuoteLabel" runat="server" Font-Size="Small" ForeColor="White" TabIndex="63" Text="New Quote Created !" Visible="False"></asp:Label>
                        </td>
                        <td class="auto-style38">&nbsp;</td>
                        <td class="auto-style35">&nbsp;</td>
                        <td class="auto-style39">&nbsp;</td>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style34">&nbsp;</td>
                        <td class="auto-style34">&nbsp;</td>
                        <td class="auto-style34">&nbsp;</td>
                        <td class="auto-style34">&nbsp;</td>
                        <td class="auto-style38">&nbsp;</td>
                        <td class="auto-style64" colspan="2" rowspan="3">&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="quoteButton" runat="server" CssClass="quote" ImageUrl="~/image/quote.png" OnClick="quoteButton_Click" TabIndex="65" Width="50px" />
                            <em><span style="color: #ffffff"><strong>Quote</strong></span></em></td>
                        <td class="auto-style38">&nbsp;</td>
                        <td class="auto-style35">&nbsp;</td>
                        <td class="auto-style39">&nbsp;</td>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style34">
                            <asp:Label ID="uselessLabel" runat="server" style="display: none"></asp:Label>
                        </td>
                        <td class="auto-style34">&nbsp;</td>
                        <td class="auto-style218" colspan="2">
                            <asp:LinkButton ID="updateLinkButton" runat="server" Font-Size="Small" OnClick="updateLinkButton_Click" TabIndex="64" ForeColor="White">update Username and Password</asp:LinkButton>
                        </td>
                        <td class="auto-style38">&nbsp;</td>
                        <td class="auto-style38">&nbsp;</td>
                        <td class="auto-style35">&nbsp;</td>
                        <td class="auto-style39">&nbsp;</td>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style34">&nbsp;</td>
                        <td class="auto-style34">&nbsp;</td>
                        <td class="auto-style218" colspan="2">&nbsp;</td>
                        <td class="auto-style38">&nbsp;</td>
                        <td class="auto-style38">&nbsp;</td>
                        <td class="auto-style35">&nbsp;</td>
                        <td class="auto-style39">&nbsp;</td>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                </table>
                
            </asp:Panel>
            <asp:Panel ID="welcomePanel" runat="server" Height="280px" Width="500px" BackColor="White">
                <table class="auto-style111">
                    <tr>
                        <td class="auto-style64" colspan="2" style="background-color: tan"><strong><span class="auto-style106">Welcome To</span><br class="auto-style106"/> <span class="auto-style106">Ashlin Customer Enquiry Form</span></strong></td>
                    </tr>
                    <tr>
                        <td class="auto-style125"></td>
                        <td class="auto-style127"></td>
                    </tr>
                    <tr>
                        <td class="auto-style126">Do you want to use <strong>Look Up Wizard</strong> ?</td>
                        <td class="auto-style127">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
                            <asp:Button ID="yesButton" runat="server" BackColor="Green" Font-Bold="True" ForeColor="White" Height="23px" Text="Yes" Width="50px" OnClick="yesButton_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="welcomeNoButton" runat="server" BackColor="Red" Font-Bold="True" ForeColor="White" Height="23px" Text="No" Width="50px" TabIndex="1" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style131" colspan="2">
                            <asp:Image ID="welcomeWizardImage" runat="server" Height="70px" ImageUrl="~/image/wizard-black.png" Width="60px" TabIndex="2" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style128" colspan="2">
                            <asp:Button ID="welcomeSkipButton" runat="server" Text="Skip" Width="70px" OnClick="welcomeSkipButton_Click" TabIndex="3" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="asiPanel" runat="server" DefaultButton="asiNextButton" BackColor="White" Height="280px" Width="500px">
                <table class="auto-style111">
                    <tr>
                        <td colspan="2" class="auto-style132"><strong>Please Enter Your ASI Number</strong></td>
                    </tr>
                    <tr>
                        <td class="auto-style148">&nbsp;</td>
                        <td class="auto-style144">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style147"><strong>ASI # :</strong></td>
                        <td class="auto-style150">
                            <asp:TextBox ID="asiTextbox" runat="server" Width="178px" TabIndex="4"></asp:TextBox>
                            <asp:Button ID="asiNextButton" runat="server" BackColor="Green" Font-Bold="True" ForeColor="White" Height="23px" Text="Next" Width="60px" TabIndex="5" OnClick="asiNextButton_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style148">&nbsp;</td>
                        <td class="auto-style149">
                            &nbsp;</td>
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
                            <asp:Button ID="asiCancelButton" runat="server" BackColor="Red" Font-Bold="True" ForeColor="White" Height="23px" Text="Cancel" Width="60px" TabIndex="6" />
                            &nbsp;&nbsp;
                            <asp:Button ID="asiSkipButton" runat="server" Text="Skip" Width="70px" OnClick="asiSkipButton_Click" TabIndex="7" />
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
                        <td class="auto-style196" colspan="2"><strong>Customer Information :</strong></td>
                        <td class="auto-style191">
                            <asp:Label ID="tooManyResultLabel" runat="server" Font-Size="11pt" ForeColor="Red" Text="Too many results found" Visible="False" Font-Bold="True" TabIndex="8"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style186">First Name :</td>
                        <td class="auto-style187">
                            <asp:TextBox ID="searchFirstNameTextbox" runat="server" Width="155px" TabIndex="9"></asp:TextBox>
                        </td>
                        <td class="auto-style199"></td>
                    </tr>
                    <tr>
                        <td class="auto-style186">Last Name :</td>
                        <td class="auto-style187">
                            <asp:TextBox ID="searchLastNameTextbox" runat="server" Width="155px" TabIndex="10"></asp:TextBox>
                        </td>
                        <td class="auto-style144" rowspan="2">
                            <asp:ImageButton ID="searchButton" runat="server" ImageUrl="~/image/search.png" CssClass="search" Width="50px" OnClick="searchButton_Click" TabIndex="12"/>
                            <em><strong>Search</strong></em></td>
                    </tr>
                    <tr>
                        <td class="auto-style186">Company Name :</td>
                        <td class="auto-style187">
                            <asp:TextBox ID="searchCompanyTextbox" runat="server" Width="155px" TabIndex="11"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style186">Email :</td>
                        <td class="auto-style187">
                            <asp:TextBox ID="searchEmailTextbox" runat="server" Width="155px" TabIndex="12"></asp:TextBox>
                        </td>
                        <td class="auto-style199"></td>
                    </tr>
                    <tr>
                        <td class="auto-style128" colspan="3">
                            <asp:Button ID="searchCancelButton" runat="server" BackColor="Red" Font-Bold="True" ForeColor="White" Height="23px" Text="Cancel" Width="60px" TabIndex="13" />
                            &nbsp;&nbsp;
                            <asp:Button ID="searchBackButton" runat="server" OnClick="searchBackButton_Click" Text="Back" Width="70px" TabIndex="14" />
                        </td>
                    </tr>
                </table>
                
            </asp:Panel>
            <asp:Panel ID="resultPanel" runat="server" Height="325px" Width="550px" BackColor="White">
                <table class="auto-style174">
                    <tr>
                        <td class="auto-style170" colspan="2"><strong>Customer Search Result</strong></td>
                    </tr>
                    <tr>
                        <td class="auto-style179" rowspan="8">
                            <asp:ListBox ID="listbox" runat="server" Height="185px" Width="210px" AutoPostBack="True" OnSelectedIndexChanged="listbox_SelectedIndexChanged" TabIndex="15"></asp:ListBox>
                        </td>
                        <td class="auto-style195">
                            <asp:Button ID="resultSelectButton" runat="server" BackColor="Green" Font-Bold="True" ForeColor="White" Height="23px" OnClick="resultSelectButton_Click" Text="Select" Width="70px" TabIndex="16" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style177">Company Name :</td>
                    </tr>
                    <tr>
                        <td class="auto-style177">
                            <asp:TextBox ID="resultCompanyTextbox" runat="server" Width="250px" TabIndex="17"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style200">Email :</td>
                    </tr>
                    <tr>
                        <td class="auto-style177">
                            <asp:TextBox ID="resultEmailTextbox" runat="server" Width="250px" TabIndex="18"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style200">Phone :</td>
                    </tr>
                    <tr>
                        <td class="auto-style177">
                            <asp:TextBox ID="resultPhoneTextbox" runat="server" Width="250px" TabIndex="19"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style194"></td>
                    </tr>
                    <tr>
                        <td class="auto-style192" colspan="2">
                            <asp:Button ID="enterManuallyButton" runat="server" BackColor="Red" Font-Bold="True" ForeColor="White" Text="Enter Info Manually" TabIndex="20" />
                            &nbsp;&nbsp;
                            <asp:Button ID="resultBackButton" runat="server" Text="Back" Width="70px" OnClick="resultBackButton_Click" TabIndex="21" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="loginPanel" runat="server" DefaultButton="loginButton" Height="170px" Width="350px" BackColor="White">
                <table class="auto-style208">
                    <tr>
                        <td colspan="2" style="background-color: tan"><strong>Please Login To Continue</strong></td>
                    </tr>
                    <tr>
                        <td colspan="2" class="auto-style216"></td>
                    </tr>
                    <tr>
                        <td class="auto-style214">Username :</td>
                        <td class="auto-style205">
                            <asp:TextBox ID="usernameTextbox" runat="server" Width="170px" TabIndex="22"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style213">Password :</td>
                        <td class="auto-style211">
                            <asp:TextBox ID="passwordTextbox" TextMode="Password" runat="server" Width="170px" TabIndex="23"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style215" colspan="2">
                            <asp:Button ID="loginCancelButton" runat="server" BackColor="Red" Font-Bold="True" ForeColor="White" Height="23px" Text="Cancel" Width="60px" TabIndex="24" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="loginButton" runat="server" BackColor="Green" Font-Bold="True" ForeColor="White" Height="23px" Text="Login" Width="60px" OnClick="loginButton_Click" TabIndex="25" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"  style="background-color: tan">&nbsp;</td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="updatePanel" runat="server" DefaultButton="updateButton" BackColor="White" Height="170px" Width="350px">
                <table class="auto-style231">
                    <tr>
                        <td colspan="2" style="background-color: tan"><strong>Enter New Credentials</strong></td>
                    </tr>
                    <tr>
                        <td class="auto-style236">New Username :</td>
                        <td class="auto-style229">
                            <asp:TextBox ID="newUsernameTextbox" runat="server" Width="170px" TabIndex="26"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style236">New Password :</td>
                        <td class="auto-style235">
                            <asp:TextBox ID="newPasswordTextbox" runat="server" TextMode="Password" Width="170px" TabIndex="27"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style236">Enter Again :</td>
                        <td class="auto-style235">
                            <asp:TextBox ID="enterAgainTextbox" runat="server" TextMode="Password" Width="170px" TabIndex="28"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style227" colspan="2">
                            <asp:Button ID="updateCancelButton" runat="server" BackColor="Red" Font-Bold="True" ForeColor="White" Height="23px" Text="Cancel" Width="60px" TabIndex="29" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="updateButton" runat="server" BackColor="Green" Font-Bold="True" ForeColor="White" Height="23px" Text="Update" Width="60px" OnClick="updateButton_Click" TabIndex="30" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="background-color: tan">&nbsp;</td>
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
    <script type="text/javascript">
        //the funciton for rush checkbox list
        var objChkd1;
        var chkLst1 = document.getElementById('rushCheckboxList');
        function HandleOnCheckRush() {
            if (objChkd1 && objChkd1.checked)
                objChkd1.checked = false;
            objChkd1 = event.srcElement;
        }

        // the function for logo checkbox list
        var objChkd2;
        var chkLst2 = document.getElementById('logoCheckboxList');
        function HandleOnCheckLogo() {
            if (objChkd2 && objChkd2.checked)
                objChkd2.checked = false;
            objChkd2 = event.srcElement;
        }
    </script>
</body>
</html>
