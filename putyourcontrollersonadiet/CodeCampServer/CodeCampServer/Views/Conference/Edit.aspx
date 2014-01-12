<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<ConferenceEditModel>" %>
<%@ Import Namespace="CodeCampServerLite.Helpers"%>

<%@ Import Namespace="CodeCampServerLite.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit
</asp:Content>
<asp:Content ContentPlaceHolderID="PageID" runat="server">editEventPage</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Edit</h2>
    <% using (Html.BeginForm())%>
    <%{ %>
        <p>
        <%=Html.LabelFor(m => m.Name) %>
        <%=Html.TextBoxFor(m => m.Name) %>
        <%=Html.ValidationMessageFor(m => m.Name) %>
        <%=Html.HiddenFor(m => m.Id) %>
        </p>
        <table>
            <tr>
                <th>First Name</th>
                <th>Last Name</th>
                <th>Email</th>
            </tr>
            <%
    for (int i = 0; i < Model.Attendees.Length; i++)
    {
        var count = i;
        %>
            <tr>
                <td>
                    <%= Html.HiddenFor(m => m.Attendees[count].Id) %>
                    <%= Html.TextBoxFor(m => m.Attendees[count].FirstName)%>
                </td>
                <td><%= Html.TextBoxFor(m => m.Attendees[count].LastName)%></td>
                <td><%= Html.TextBoxFor(m => m.Attendees[count].Email)%></td>
            </tr>
        <%
    } %>
        </table>
        <p>
            <input type="submit" value="Save" />
        </p>
    <%} %>
</asp:Content>
