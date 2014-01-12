<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Redirect</h2>
    <p>
        Your text is: <%= ViewData["text"] %>
    </p>
    <form action="/dynamic/goto">
        <label for="gotoUrl">Go To:</label>
        <input type="text" id="gotoUrl" name="gotoUrl" />
        <input type="submit" value="Submit"/>
    </form>
</asp:Content>
