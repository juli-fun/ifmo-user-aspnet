using NUnit.Framework;
using System;

namespace ifmouseraspnet.Tests
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void TestPasswordGeneration()
        {
            var _password = User.Generate_Password();
            Assert.IsTrue(_password.Length == 10);
        }

        [Test]
        public void TestAuthentication()
        {
            string password = "P@ssw0rd#123";

            var User_test = new User("test@test.ru",
            "test", password, "Test", "Test", "Test");

            User_test.Auth_Passwd(password);

            Assert.IsTrue(User_test.Is_Authorized);
        }

        [Test]
        public void TestRegistration()
        {
            string password = "P@ssw0rd#123";

            var User_test = new User("test@test.ru",
            "test", password, "Test", "Test", "Test");

            User_test.Auth_Passwd(password);
            User_test.Register();

            Assert.IsTrue(User_test.Is_Registered);
        }

        [Test]
        public void TestAgeCount()
        {
            string birth_date_str = "1 / 1 / 1970 0:0:0 AM";

            var User_test = new User("test@test.ru",
            "test", "P@ssw0rd#123", "Test", "Test", "Test", birth_date_str);

            var birth_date = DateTime.Parse(birth_date_str,
                System.Globalization.CultureInfo.InvariantCulture);

            int age_now = (int.Parse(DateTime.Now.ToString("yyyyMMdd")) -
                int.Parse(birth_date.ToString("yyyyMMdd"))) / 10000;

            Assert.IsTrue(age_now == User_test.Get_Age());
        }
    }
}
