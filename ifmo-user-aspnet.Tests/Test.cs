using NUnit.Framework;
using System;

namespace ifmouseraspnet.Tests
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void GeneratedPasswordLengthIsTenSymbols()
        {
            var _password = RandomPassword.Generate(10);
            Assert.IsTrue(_password.Length == 10);
        }
    }
}
