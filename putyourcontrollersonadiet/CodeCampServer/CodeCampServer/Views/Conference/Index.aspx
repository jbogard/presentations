<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<ConferenceListModel[]>" %>
<%@ Import Namespace="CodeCampServerLite"%>
<%@ Import Namespace="CodeCampServerLite.Helpers"%>
<%@ Import Namespace="CodeCampServerLite.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ContentPlaceHolderID="PageID" runat="server"><%= SiteNav.Page.Conference.Index %></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% if ((Model != null) && Model.Any()) { %>
    <table>
    <tr><th>Id</th><th>Conference Name</th><th>Session Count</th><th>Attendee Count</th></tr>
    <%
		int count = 0;
		foreach (var model in Model) { %>
            <tr>
                <td><%=Html.ActionLink("Edit", "Edit", new { eventname =model.Name }, new { rel = "Edit " + model.Name })%></td>
                <td><%=Html.ActionLink(model.Name, "Show", new { eventname = model.Name }, new { rel = "Show " + model.Name})%></td>
                <td><%= Html.DisplayFor(m => m[count].SessionCount) %></td>
                <td><%= Html.DisplayFor(m => m[count].AttendeeCount)%></td>
            </tr>
        <%
			count++;
		} %> 
    </table>
    <% } %>
    
    <form action="<%= Url.Action("Index") %>" method="get">
        <label for="minSessions">Minimum Session Count:</label>
        <input id="minSessions" name="minSessions" type="text" />
        <%= Html.SubmitButton() %>
    </form>
    <%= Html.ActionLink("Show XML", "Index", "ConferenceXml") %>

</asp:Content>
