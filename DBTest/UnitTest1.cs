using ChatDBLibrary;

namespace DBTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetUsers()
        {
            var userDA = new UserDA("Server=127.0.0.1;Port=5432;Database=windows;User Id=postgres;Password=0726;");
            var users = userDA.GetUsers();

            Assert.IsTrue(users.Count() == 6);
        }
        [TestMethod]
        public void CheckFirstUserName()
        {
            var userDA = new UserDA("Server=127.0.0.1;Port=5432;Database=windows;User Id=postgres;Password=0726;");
            var users = userDA.GetUsers();
            Assert.IsNotNull(users);
            Assert.IsTrue(users.Any()); 
            var firstUser = users.Last();
            Assert.AreEqual("Zendaya", firstUser.Username);
        }
    }
}