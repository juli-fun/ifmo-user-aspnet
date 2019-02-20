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
            int index = -1;
            index = Users.FindIndex(x => x.Login == _login.Text);

            if (index >= 0 && Users[index].Login == _login.Text)
            {
                Users[index].Email = _email.Text;
                Users[index].Fname = _fname.Text;
                Users[index].Name = _name.Text;
                Users[index].Lname = _lname.Text;
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
            int index = -1;
            Label isAuth = this.FindControl("is_authorized") as Label;

            index = Users.FindIndex(x => x.Login == _login_auth.Text);

            if (index >= 0)
            {
                Users[index].Auth_Passwd(_password_auth.Text);

                if (Users[index].Is_Authorized)
                {
                    isAuth.Text = "Авторизован!";
                }
                else
                {
                    isAuth.Text = "Ошибка авторизации!";
                }
            }
            else
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
