<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if (Model == null) { %>
    <%= ViewData.ModelMetadata.NullDisplayText %>
<% } else if (ViewData.TemplateInfo.TemplateDepth > 1) { %>
    <%= ViewData.ModelMetadata.SimpleDisplayText %>
<% } else { %>
    <% foreach (var prop in ViewData.ModelMetadata.Properties.Where(
           pm => pm.ShowForDisplay 
               && !ViewData.TemplateInfo.Visited(pm))) { %>
        <% if (prop.HideSurroundingHtml) { %>
            <%= Html.Display(prop.PropertyName) %>
        <% } else { %>
        <p>
            <div>
                <span style="float: left; text-align: right; width: 150px; margin-right: 5px;">
                    <% if (prop.IsRequired) { %>
                        <span class="required">****</span>
                    <% } %>
                    <%= Html.Label(prop.PropertyName) %>:
                </span>
                <span style="float: left;">
                    <%= Html.Editor(prop.PropertyName) %>
                    <%= Html.ValidationMessage(prop.PropertyName) %>
                </span>
                <div style="clear: both; height: 0px; margin: 0px; padding: 0px;"></div>
            </div>
        </p>
        <% } %>
    <% } %>
<% } %>