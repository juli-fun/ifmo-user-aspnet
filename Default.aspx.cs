using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using System.Collections.Generic; // для List
using Newtonsoft.Json; // для чтения и сохранения списка пользователей в/из JSON

namespace ifmouseraspnet
{
public partial class Default : System.Web.UI.Page
    {
        // Создаем список пользователей
        List<User> Users = new List<User>();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Читаем список пользователей из файла
            Users = User_db.Read("users.json");

            GridViewUsers.DataSource = Users;
            GridViewUsers.DataBind();
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        public void reg_submit_Clicked(object sender, EventArgs args)
        {
            Users.Add(new User(_email.Text,
            _login.Text, _password.Text, _fname.Text, _name.Text, _lname.Text));

            // Сохраняем юзеров в файл
            User_db.Write(Users, "users.json");

            // Перезагрузим страницу
            Response.Redirect(Request.RawUrl);
        }
    }
}
