<%@ Page Language="C#" Inherits="ifmouseraspnet.Default" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
	<title>Default</title>
</head>
<body>
	<form id="form1" runat="server">
		<asp:Button id="button1" runat="server" Text="Click me!" OnClick="button1Clicked" />
	</form>
    <h3>Список пользователей</h3>
    <asp:Table id="TableUsers" 
        GridLines="Both" 
        HorizontalAlign="Left" 
        Font-Names="Verdana" 
        Font-Size="8pt" 
        CellPadding="15" 
        CellSpacing="0" 
        Runat="server"/>
</body>
</html>
