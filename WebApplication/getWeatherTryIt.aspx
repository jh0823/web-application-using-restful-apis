<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="getWeatherTryIt.aspx.cs" Inherits="WebApplication2.WebForm3" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>Get Weather Information</h2>
<p>This service will take in an address and find the next 5 days average temperatures in Fahrenheit.</p>
<p>Follow this template when entering the address:</p>
<p>Address: 1313 Disneyland Drive</p>
<p>City: Anaheim</p>
<p>State: CA</p>
<p>
    <asp:Label ID="Label1" runat="server" Text="Enter Address"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
</p>
<p>
    <asp:Label ID="Label2" runat="server" Text="Enter City"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
</p>
<p>
    <asp:Label ID="Label3" runat="server" Text="Enter State"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
</p>
<p>
    <asp:Label ID="Label4" runat="server"></asp:Label>
</p>
<p>&nbsp;</p>
<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Get Next 5 Day Averages" />
</p>
<p>
    <asp:TextBox ID="TextBox4" runat="server" Height="200px" ReadOnly="True" TextMode="MultiLine" Width="500px"></asp:TextBox>
</p>
</asp:Content>
