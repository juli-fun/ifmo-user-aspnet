using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

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

            if (!Page.IsPostBack) BindGridView();
        }

        public void reg_submit_Clicked(object sender, EventArgs args)
        {
            var index = Users.Find(x => x.Login == _login.Text);

            Label isAuth = this.FindControl("is_authorized") as Label;
            if (index.Login == _login.Text)
            {
                index.Email = _email.Text;
                index.Fname = _fname.Text;
                index.Name = _name.Text;
                index.Lname = _lname.Text;
            }
            else
            {
                Users.Add(new User(_email.Text, _login.Text, _password.Text,
                _fname.Text, _name.Text, _lname.Text));

            }

            // Сохраняем юзеров в файл
            User_db.Write(Users, "users.json");

            // Перезагрузим страницу
            Response.Redirect(Request.RawUrl);
        }

        public void reg_auth_Clicked(object sender, EventArgs args)
        {
            var index = Users.Find(x => x.Login == _login_auth.Text);
            index.Auth_Passwd(_password_auth.Text);

            Label isAuth = this.FindControl("is_authorized") as Label;

            if (index.Is_Authorized)
            {
                isAuth.Text = "Авторизован!";
            } else
            {
                isAuth.Text = "Ошибка авторизации!";
            }
        }

        private void BindGridView()
        {
            GridViewUsers.DataSource = Users;
            GridViewUsers.DataBind();
        }
    }
}
