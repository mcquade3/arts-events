<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ArtsEvents.ascx.cs" Inherits="widgets_CalendarEvents" %>

<%@ Register Src="~/Workarea/PageBuilder/PageControls/PageHost.ascx" TagName="PageHost" TagPrefix="cms" %>
<%@ Register Src="~/Workarea/PageBuilder/PageControls/DropZone.ascx" TagName="DropZone" TagPrefix="cms" %>
<%@ Register Assembly="Ektron.Cms.Widget" Namespace="Ektron.Cms.PageBuilder" TagPrefix="pb" %>
<%@ Register TagPrefix="cms" Namespace="Ektron.Cms.Controls" Assembly="Ektron.Cms.Controls" %>

<asp:MultiView ID="ViewSet" runat="server">
    <asp:View ID="View" runat="server">
        <div class="row">
            <div class="col-sm-12">
                <h4><strong>Upcoming Performances:</strong></h4>
                <asp:Literal ID="litEvents" runat="server" />
            </div>
        </div>
    </asp:View>
    <asp:View ID="Edit" runat="server">
        <table>
            <tr>
                <td>CalendarID:</td>
                <td>
                    <asp:TextBox ID="calendarIDtextBox" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Taxonomy Path:</td>
                <td>
                    <asp:TextBox ID="taxonomyTextBox" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Number of Events Displayed:</td>
                <td>
                    <asp:TextBox ID="numEventsTextBox" runat="server" type="number" min="0"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <asp:Button ID="CancelButton" runat="server" Text="Cancel" OnClick="CancelButton_Click"/>
        <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click"/>
    </asp:View>
</asp:MultiView>