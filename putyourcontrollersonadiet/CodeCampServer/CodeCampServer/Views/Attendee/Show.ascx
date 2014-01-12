<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ConferenceShowModel.AttendeeModel[]>" %>
<%@ Import Namespace="CodeCampServerLite.Models"%>

    <table>
    <tr><th>Attendee</th><th>Email</th></tr>
    <%foreach (var attendee in Model)%>
        <%{ %>
            <tr><td><%=Html.Encode(attendee.FirstName)%>&nbsp;<%=Html.Encode(attendee.LastName)%></td><td><%=Html.Encode(attendee.Email)%></td></tr>
        <%} %> 
    </table>
