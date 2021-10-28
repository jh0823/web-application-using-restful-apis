<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" ValidateRequest="false" CodeBehind="getNewsTryIt.aspx.cs" Inherits="WebApplication2.WebForm1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Get News</h2>
<p>Given address, retrieve news articles published revolving around that area:</p>
    <p>Follow this template when entering in address for news:</p>
<p>City: <span style="color: rgb(34, 34, 34); font-family: Roboto, arial, sans-serif; font-size: 14px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">Anaheim</span></p>
<p>State: CA</p>
<p>
&nbsp;<asp:Label ID="Label1" runat="server" Text="Enter City"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
</p>
<p>
    <asp:Label ID="Label2" runat="server" Text="Enter State"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
</p>
<p>
    <asp:Label ID="Label3" runat="server"></asp:Label>
    </p>
<p>
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Get News" />
</p>
<p>&nbsp;</p>
<p>
    <asp:TextBox ID="TextBox3" runat="server" Height="400px" ReadOnly="True" TextMode="MultiLine" Width="800px"></asp:TextBox>
    </p>
    

</asp:Content>