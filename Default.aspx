<%@ Page Language="C#" Inherits="ifmouseraspnet.Default" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
	<title>Default</title>
</head>
<body>
    <h3>Регистрация/создание пользователя</h3>
    <form id="regUser" runat="server">
        <table class="auto-style1">  
                <tr>  
                    <td>Фамилия:</td>  
                    <td>  
                        <asp:TextBox ID="_fname" runat="server"></asp:TextBox>  
                    </td>  
                </tr>  
                <tr>  
                    <td>Имя:</td>  
                    <td>  
                        <asp:TextBox ID="_name" runat="server"></asp:TextBox>  
                    </td>  
                </tr>
                <tr>  
                    <td>Отчество:</td>  
                    <td>  
                        <asp:TextBox ID="_lname" runat="server"></asp:TextBox>  
                    </td>  
                </tr>  
                <tr>  
                    <td>E-mail:</td>  
                    <td>  
                        <asp:TextBox ID="_email" runat="server"></asp:TextBox>  
                    </td>  
                </tr> 
                <tr>  
                    <td>Логин:</td>  
                    <td>  
                        <asp:TextBox ID="_login" runat="server"></asp:TextBox>  
                    </td>  
                </tr>  
                <tr>  
                    <td>Пароль:</td>  
                    <td>  
                        <asp:TextBox ID="_password" runat="server" TextMode="Password"></asp:TextBox>  
                    </td>  
                </tr>  
                <tr>  
                    <td>  
                        <asp:Button ID="_reg_submit" runat="server" Text="Зарегистрировать" OnClick="reg_submit_Clicked" />  
                    </td>  
                </tr>  
            </table>  
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
