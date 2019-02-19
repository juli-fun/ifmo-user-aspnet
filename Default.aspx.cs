﻿using System;
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

            var th = new TableHeaderRow();

            var tc1 = new TableHeaderCell();
            tc1.Controls.Add(new LiteralControl("Фамилия"));
            th.Cells.Add(tc1);

            var tc2 = new TableHeaderCell();
            tc2.Controls.Add(new LiteralControl("Имя"));
            th.Cells.Add(tc2);

            var tc3 = new TableHeaderCell();
            tc3.Controls.Add(new LiteralControl("Отчество"));
            th.Cells.Add(tc3);

            var tc4 = new TableHeaderCell();
            tc4.Controls.Add(new LiteralControl("Логин"));
            th.Cells.Add(tc4);

            var tc5 = new TableHeaderCell();
            tc5.Controls.Add(new LiteralControl("E-Mail"));
            th.Cells.Add(tc5);

            TableUsers.Rows.Add(th);

            foreach (var _User in Users)
            {
                TableRow r = new TableRow();

                TableCell c1 = new TableCell();
                c1.Controls.Add(new LiteralControl(_User.Fname));
                r.Cells.Add(c1);

                TableCell c2 = new TableCell();
                c2.Controls.Add(new LiteralControl(_User.Name));
                r.Cells.Add(c2);

                TableCell c3 = new TableCell();
                c3.Controls.Add(new LiteralControl(_User.Lname));
                r.Cells.Add(c3);

                TableCell c4 = new TableCell();
                c4.Controls.Add(new LiteralControl(_User.Login));
                r.Cells.Add(c4);

                TableCell c5 = new TableCell();
                c5.Controls.Add(new LiteralControl(_User.Email));
                r.Cells.Add(c5);

                TableUsers.Rows.Add(r);
            }
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
