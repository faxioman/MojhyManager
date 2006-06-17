<%@ Page Language="C#" MasterPageFile="~/frontend.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="_default" Title="Mojhy Manager - Home Page" %>
<asp:Content ID="menu" ContentPlaceHolderID="cntmenu" Runat="Server">
MENU
</asp:Content>
<asp:Content ID="main" ContentPlaceHolderID="cntmain" Runat="Server">
<script language="javascript" type="text/javascript">
// This method will be called after the method has been executed
// and the result has been sent to the client.
function getServerTime_callback(res)
{
  alert(res.value);
}
</script>
<input type="button" value="vedi data server" onclick="_default.GetServerTime(getServerTime_callback)" />
</asp:Content>

