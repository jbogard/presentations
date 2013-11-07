<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CodeCampServerLite.Models.AttendeeEditModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Register
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Register</h2>
    
    <% using (Html.BeginForm("Register", "Conference")) %>
    <%{ %>
    
        <%= Html.EditorForModel() %>
		<%= Html.EditorFor(m => m.EventId) %>

        <input type="submit" value="Register" />
    <%} %>
</asp:Content>
