using ChatDBLibrary;

namespace DBTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetUsers()
        {
            var userDA = new UserDA("Server=127.0.0.1;Port=5432;Database=postgres;User Id=postgres;Password=heroO726;");
            var users = userDA.GetUsers(4);

            Assert.IsTrue(users.Count() == 5);
        }
    }
}