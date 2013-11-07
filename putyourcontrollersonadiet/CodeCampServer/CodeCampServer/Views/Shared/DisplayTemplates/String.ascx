<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<span id="<%= ViewData.TemplateInfo.GetFullHtmlFieldId(null) %>">        
    <%= Html.Encode(ViewData.TemplateInfo.FormattedModelValue) %>        
</span>
