<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="WebApplication2.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>About Page</h2>
    <h3>This web application will provide take in a user&#39;s input in the form of an address and provide the following information:</h3>
    <p>Get News will list the latests new articles revolving around the given city and state from the user.</p>
    <p>Create Map will generate a map centered around the given address and will allow&nbsp; for the user to add a marker onto the map and will update the map to adjust for distance and zoom so that both markers will be displayed to the user</p>
    <p>Get Weather will calculate the average temperatures for the next 5 days for that given city and state</p>
    <p>Get Solar Data will compare two cities and states by looking at the annual solar intensity and the monthly averages for those locations and return which location has a higher solar intensity</p>
    <p>&nbsp;</p>
</asp:Content>
