<%@ Page Language="C#" Inherits="ifmouseraspnet.Default" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
	<title>Default</title>
</head>
<body>
    <h3>Авторизация пользователя</h3>
    <form id="authUser" runat="server">
        <table class="auto-style1">  
                <tr>  
                    <td>Логин:</td>  
                    <td>  
                        <asp:TextBox ID="_login_auth" runat="server"></asp:TextBox>  
                    </td>  
                </tr>  
                <tr>  
                    <td>Пароль:</td>  
                    <td>  
                        <asp:TextBox ID="_password_auth" runat="server" TextMode="Password"></asp:TextBox>  
                    </td>  
                </tr>  
                <tr>  
                    <td>Статус:</td>  
                    <td>  
                        <asp:Label ID="is_authorized" runat="server" /> 
                    </td>  
                </tr>  
                <tr>  
                    <td>  
                        <asp:Button ID="_reg_auth" runat="server" Text="Авторизоваться" OnClick="reg_auth_Clicked" />  
                    </td>  
                </tr>  
            </table>  
        </form>
    <h3>Регистрация/создание/редактирование пользователя</h3>
    <p>Для редактирования пользователя введите его логин</p>
    <form id="regUser" runat="server">
        <table class="auto-style1">  
                <tr>  
                    <td>Логин:</td>  
                    <td>  
                        <asp:TextBox ID="_login" runat="server"></asp:TextBox>  
                    </td>  
                </tr>  
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
                    <td>Пароль:</td>  
                    <td>  
                        <asp:TextBox ID="_password" runat="server" TextMode="Password"></asp:TextBox>  
                    </td>  
                </tr>  
                <tr>  
                    <td>  
                        <asp:Button ID="_reg_submit" runat="server" Text="Сохранить" OnClick="reg_submit_Clicked" />  
                    </td>  
                </tr>  
            </table>  
    </form>
    <h3>Список пользователей</h3>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="GridViewUsers" runat="server" AutoGenerateColumns = "false">
            <Columns>
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="Login" HeaderText="Login" />
                <asp:BoundField DataField="Fname" HeaderText="Фамилия" />
                <asp:BoundField DataField="Name" HeaderText="Имя" />
                <asp:BoundField DataField="Lname" HeaderText="Отчество" />
            </Columns>
        </asp:GridView>  
    </div>
    </form>      
</body>
</html>
