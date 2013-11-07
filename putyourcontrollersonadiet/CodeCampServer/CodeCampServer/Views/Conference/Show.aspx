<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CodeCampServerLite.Models.ConferenceShowModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Show
</asp:Content>
<asp:Content ContentPlaceHolderID="Scripts" runat="server">
<% if (false) { %>
    <script src="../../Scripts/jquery-1.3.2-vsdoc.js" type="text/javascript"></script>
<% } %>
<script language="javascript" type="text/javascript">

    $(function() {
        $('#showAttendees').click(function() {
            var url = '<%= Url.Action("Show", "Attendee", new { eventName = Model.Name }) %>';
            
            // Keep a reference to the show attendees button for later
            var button = $(this);
            
            var callback = function(data, textStatus) {
                
                // Place HTML returned from AJAX inside special p tag below
                $('#attendees').html(data);
                
                // Hide the show attendees button
                button.hide();
            };
            
            // Issue an AJAX GET, to return HTML (and not JSON)
            $.get(url, null, callback, 'html');
        });
    });

</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.DisplayFor(m => m.Name) %></h2>

    <p>
    <table>
        <tr><th>Speaker</th><th>Title</th></tr>
    <%
		int count = 0;
    	foreach (var session in Model.Sessions)%>
        <%{ %>
            <tr>
                <td>
					<%=Html.DisplayFor(m => m.Sessions[count].SpeakerFirstName)%>
					<%=Html.DisplayFor(m => m.Sessions[count].SpeakerLastName)%>
				</td>
                <td><%=Html.DisplayFor(m => m.Sessions[count].Title)%></td>
            </tr>
        <%
			count++;
		} %> 
    </table>
    </p>

    <button id="showAttendees">Show Attendees</button>
    <p id="attendees">
    </p>

</asp:Content>

