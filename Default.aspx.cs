using System;
using System.Web;
using System.Web.UI;
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
            string json_fromfile;

            using (StreamReader sr = new StreamReader("users.json"))
            {
                json_fromfile = sr.ReadToEnd();
            }

            Users = JsonConvert.DeserializeObject<List<User>>(json_fromfile);
        }

        public void button1Clicked(object sender, EventArgs args)
        {
            button1.Text = Users[1].Get_Short_Initials();
        }
    }
}
