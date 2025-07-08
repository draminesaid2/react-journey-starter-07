<%@ Control Language="C#" AutoEventWireup="true" Inherits="ViewUserControl<Crm.ViewModels.AdminViewModel>" %>
<%@ Import Namespace="Crm.Extensions" %>
<%@ Import Namespace="Crm.Library.Globalization.Extensions" %>

<div class="box" id="index-statistics">
	<div class="header clearfix">
			<div class="right actions pager">
				<a href="#" id="create-index" class="btn btn-default btn-sm" onclick="$.ajax({
																															url: Helper.resolveUrl('~/Contact/CreateIndex'),
																															beforeSend: function(data) {
																																$('a#create-index').addClass('busy');
																															},
																															success: function(data) {
																																$('a#create-index').removeClass('busy');
																																$('#index-statistics-details').replaceWith('<div class=entry>' + data + '</div>');
																															}    
																													});
																													return false;">
					<%= Html.Localize("CreateIndex") %>
				</a>
			</div>
		<h2>
			<%= Html.ToggleBoxIcon(true) %>
			<%= Html.Localize("Information") %>
		</h2>
	</div>
	
	<ul class="entry" id="index-statistics-details">
		<li><%=Html.Localize("Index Modified") %><span>&nbsp;<%= Model.ContactIndexDateUtc.ToLocalTime() %></span></li>
		<li><%=Html.Localize("Dictionary Modified") %><span>&nbsp;<%= Model.ContactDictionaryDateUtc.ToLocalTime() %></span></li>
	</ul>
</div>
