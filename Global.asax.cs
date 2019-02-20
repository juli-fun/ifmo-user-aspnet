using System;
using System.Web;
using System.IO;
using System.Security.Cryptography; // для md5
using System.Text; // для md5
using System.Collections.Generic; // для List
using Newtonsoft.Json; // для чтения и сохранения списка пользователей в/из JSON

// для восстановления пароля
using System.Net.Mail;

namespace ifmouseraspnet
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
        }
    }

    // Класс для работы с базой в виде JSON-файла
    public static class User_db
    {
        public static List<User> Read(string _filename)
        {
            // Читаем список пользователей из файла
            string json_fromfile;

            using (StreamReader sr = new StreamReader(_filename))
            {
                json_fromfile = sr.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<List<User>>(json_fromfile);
        }

        public static void Write(List<User> _users, string _filename)
        {
            // Сохраняем юзеров в файл
            var json = JsonConvert.SerializeObject(_users, Formatting.Indented);
            Console.WriteLine(json);

            using (StreamWriter sw = new StreamWriter(_filename, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(json);
                sw.Close();
            }
        }
    }

    public class User
    {
        [JsonProperty]
        private string email, login, fname, name, lname, auth_key = "";
        [JsonProperty]
        private DateTime created_at, updated_at, birth_date;
        [JsonProperty]
        private bool is_blocked = false;
        [JsonProperty]
        private bool is_authorized = false;
        [JsonProperty]
        private bool is_registered = false;
        [JsonProperty]
        private string password_md5;

        // Для обновления пользователя мы сделали
        // методы set для атрибтов объекта

        [JsonIgnore]
        public string Email
        {
            get { return email; }
            set { email = value; updated_at = DateTime.Now; }
        }

        [JsonIgnore]
        public string Login
        {
            get { return login; }
            set { login = value; updated_at = DateTime.Now; }
        }

        [JsonIgnore]
        public string Fname
        {
            get { return fname; }
            set { fname = value; updated_at = DateTime.Now; }
        }

        [JsonIgnore]
        public string Name
        {
            get { return name; }
            set { name = value; updated_at = DateTime.Now; }
        }

        [JsonIgnore]
        public string Lname
        {
            get { return lname; }
            set { lname = value; updated_at = DateTime.Now; }
        }

        [JsonIgnore]
        public bool Is_Registered
        {
            get { return is_registered; }
            set {; }
        }

        [JsonIgnore]
        public bool Is_Blocked
        {
            get { return is_blocked; }
            set {; }
        }

        [JsonIgnore]
        public bool Is_Authorized
        {
            get { return is_authorized; }
            set {; }
        }

        public User()
        {

        }

        // Cоздание пользователя / регистрация
        // Параметры, которые помечены звездочкой (*), - обязательные,
        // также обязательный сделан логин. Остальные - необязательные
        public User(string _email, string _login, string _password = "",
            string _fname = "Surname", string _name = "Name",
            string _lname = "Middlename",
            string _birth_date = "1 / 1 / 1970 0:0:0 AM")
        {
            // Если пароль пустой, генерируем пароль и для простоты выводим
            // его в консоль (да, небезопасно, но это же учебное задание)
            if (_password == "")
            {
                _password = RandomPassword.Generate(10);
                Console.WriteLine("Пароль: " + _password);
            }

            // Берем значения из параметров
            email = _email;
            login = _login;
            fname = _fname;
            name = _name;
            lname = _lname;
            birth_date = DateTime.Parse(_birth_date,
                System.Globalization.CultureInfo.InvariantCulture);

            // Хешируем пароль
            password_md5 = MD5_encode(_password);

            // Временные метки
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }

        // Служебная функция для создания md5-хэша
        private string MD5_encode(string _password)
        {
            MD5 md5 = MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(_password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            return sb.ToString();
        }

        // Авторизация по паролю
        public int Auth_Passwd(string _password)
        {
            if (MD5_encode(_password) == password_md5)
            {
                is_authorized = true;
                Console.WriteLine("Authorized by password!");
                return 0;
            }
            else
            {
                Console.WriteLine("Not authorized by password!");
                return 1;
            }
        }

        public string GetKey()
        {
            if (is_authorized)
            {
                auth_key = RandomPassword.Generate(10);
                return auth_key;
            }
            else { return ""; }
        }

        public int RevokeKey()
        {
            if (is_authorized)
            {
                auth_key = "";
                return 0;
            }
            else { return 1; }
        }

        // Авторизация по ключу
        public int Auth_by_Key(string _auth_key)
        {
            // Ключа нет - авторизации нет
            if (auth_key == "") return 1;

            if (_auth_key == auth_key)
            {
                is_authorized = true;
                Console.WriteLine("Authorized by key!");
                return 0;
            }
            else
            {
                Console.WriteLine("Not authorized by key!");
                return 1;
            }
        }

        // Восстановление пароля
        public int Recover_Passwd()
        {
            // Генерируем новый пароль
            var password = RandomPassword.Generate(10);
            password_md5 = MD5_encode(password);

            // Отправляем его на почту (надо только указать работающий релей)
            MailMessage mail = new MailMessage("user@company.com", email);
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.company.com";
            mail.Subject = "Password recovery";
            mail.Body = "New password: " + password;
            client.Send(mail);

            return 0;
        }

        // Расчет возраста
        public int Get_Age()
        {
            return (int.Parse(DateTime.Now.ToString("yyyyMMdd")) -
                int.Parse(birth_date.ToString("yyyyMMdd"))) / 10000;
        }

        // Вывод полных ФИО
        public string Get_Full_Initials()
        {
            return fname + " " + name + " " + lname;
        }

        // Вывод сокращенных ФИО
        public string Get_Short_Initials()
        {
            return fname + " " + name.Substring(0, 1) + "." +
                lname.Substring(0, 1) + ".";
        }

        // Регистрация
        public int Register()
        {
            if (is_authorized)
            {
                is_registered = true;
                return 0;
            }
            else { return 1; }
        }

        // Отмена регистрации
        public int UnRegister()
        {
            if (is_authorized)
            {
                is_registered = false;
                return 0;
            }
            else { return 1; }
        }

        // Удаление пользователя реализуется деструктором по умолчанию (~User)
        // и отдельного описания не требует
    }

    // Класс для генерации паролей
    public class RandomPassword
    {
        // Define default min and max password lengths.
        private static int DEFAULT_MIN_PASSWORD_LENGTH = 8;
        private static int DEFAULT_MAX_PASSWORD_LENGTH = 10;

        // Define supported password characters divided into groups.
        // You can add (or remove) characters to (from) these groups.
        private static string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
        private static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
        private static string PASSWORD_CHARS_NUMERIC = "23456789";
        private static string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";

        /// <summary>
        /// Generates a random password.
        /// </summary>
        /// <returns>
        /// Randomly generated password.
        /// </returns>
        /// <remarks>
        /// The length of the generated password will be determined at
        /// random. It will be no shorter than the minimum default and
        /// no longer than maximum default.
        /// </remarks>
        public static string Generate()
        {
            return Generate(DEFAULT_MIN_PASSWORD_LENGTH,
                            DEFAULT_MAX_PASSWORD_LENGTH);
        }

        /// <summary>
        /// Generates a random password of the exact length.
        /// </summary>
        /// <param name="length">
        /// Exact password length.
        /// </param>
        /// <returns>
        /// Randomly generated password.
        /// </returns>
        public static string Generate(int length)
        {
            return Generate(length, length);
        }

        /// <summary>
        /// Generates a random password.
        /// </summary>
        /// <param name="minLength">
        /// Minimum password length.
        /// </param>
        /// <param name="maxLength">
        /// Maximum password length.
        /// </param>
        /// <returns>
        /// Randomly generated password.
        /// </returns>
        /// <remarks>
        /// The length of the generated password will be determined at
        /// random and it will fall with the range determined by the
        /// function parameters.
        /// </remarks>
        public static string Generate(int minLength,
                                      int maxLength)
        {
            // Make sure that input parameters are valid.
            if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                return null;

            // Create a local array containing supported password characters
            // grouped by types. You can remove character groups from this
            // array, but doing so will weaken the password strength.
            char[][] charGroups = new char[][]
            {
            PASSWORD_CHARS_LCASE.ToCharArray(),
            PASSWORD_CHARS_UCASE.ToCharArray(),
            PASSWORD_CHARS_NUMERIC.ToCharArray(),
            PASSWORD_CHARS_SPECIAL.ToCharArray()
            };

            // Use this array to track the number of unused characters in each
            // character group.
            int[] charsLeftInGroup = new int[charGroups.Length];

            // Initially, all characters in each group are not used.
            for (int i = 0; i < charsLeftInGroup.Length; i++)
                charsLeftInGroup[i] = charGroups[i].Length;

            // Use this array to track (iterate through) unused character groups.
            int[] leftGroupsOrder = new int[charGroups.Length];

            // Initially, all character groups are not used.
            for (int i = 0; i < leftGroupsOrder.Length; i++)
                leftGroupsOrder[i] = i;

            // Because we cannot use the default randomizer, which is based on the
            // current time (it will produce the same "random" number within a
            // second), we will use a random number generator to seed the
            // randomizer.

            // Use a 4-byte array to fill it with random bytes and convert it then
            // to an integer value.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = BitConverter.ToInt32(randomBytes, 0);

            // Now, this is real randomization.
            Random random = new Random(seed);

            // This array will hold password characters.
            char[] password = null;

            // Allocate appropriate memory for the password.
            if (minLength < maxLength)
                password = new char[random.Next(minLength, maxLength + 1)];
            else
                password = new char[minLength];

            // Index of the next character to be added to password.
            int nextCharIdx;

            // Index of the next character group to be processed.
            int nextGroupIdx;

            // Index which will be used to track not processed character groups.
            int nextLeftGroupsOrderIdx;

            // Index of the last non-processed character in a group.
            int lastCharIdx;

            // Index of the last non-processed group.
            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

            // Generate password characters one at a time.
            for (int i = 0; i < password.Length; i++)
            {
                // If only one character group remained unprocessed, process it;
                // otherwise, pick a random character group from the unprocessed
                // group list. To allow a special character to appear in the
                // first position, increment the second parameter of the Next
                // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
                if (lastLeftGroupsOrderIdx == 0)
                    nextLeftGroupsOrderIdx = 0;
                else
                    nextLeftGroupsOrderIdx = random.Next(0,
                                                         lastLeftGroupsOrderIdx);

                // Get the actual index of the character group, from which we will
                // pick the next character.
                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                // Get the index of the last unprocessed characters in this group.
                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                // If only one unprocessed character is left, pick it; otherwise,
                // get a random character from the unused character list.
                if (lastCharIdx == 0)
                    nextCharIdx = 0;
                else
                    nextCharIdx = random.Next(0, lastCharIdx + 1);

                // Add this character to the password.
                password[i] = charGroups[nextGroupIdx][nextCharIdx];

                // If we processed the last character in this group, start over.
                if (lastCharIdx == 0)
                    charsLeftInGroup[nextGroupIdx] =
                                              charGroups[nextGroupIdx].Length;
                // There are more unprocessed characters left.
                else
                {
                    // Swap processed character with the last unprocessed character
                    // so that we don't pick it until we process all characters in
                    // this group.
                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] =
                                    charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }
                    // Decrement the number of unprocessed characters in
                    // this group.
                    charsLeftInGroup[nextGroupIdx]--;
                }

                // If we processed the last group, start all over.
                if (lastLeftGroupsOrderIdx == 0)
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                // There are more unprocessed groups left.
                else
                {
                    // Swap processed group with the last unprocessed group
                    // so that we don't pick it until we process all groups.
                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                    leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }
                    // Decrement the number of unprocessed groups.
                    lastLeftGroupsOrderIdx--;
                }
            }

            // Convert password characters into a string and return the result.
            return new string(password);
        }
    }
}
