<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CodeCampServerLite.Models.AttendeeConfirmationModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	AttendeeConfirmation
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Attendee Confirmation</h2>
    
    Congratulations, 
    <%= Html.DisplayFor(x => x.FirstName) %><%= Html.DisplayFor(x => x.LastName) %>
    is successfully registered for 
    <%= Html.DisplayFor(x => x.EventName) %>
    

</asp:Content>
