using ChatDBLibrary.Model;
using Npgsql;

namespace ChatDBLibrary
{
    public class UserDA
    {

        public UserDA(string connectionString) {
            this.connectionString = connectionString;
        }
        public string connectionString { get; set; }

        public IEnumerable<User> GetUsers()
        {
            List<User> users = new List<User>();
            using(var conn = new NpgsqlConnection(connectionString))
            {
                var cmdGetUsers = new NpgsqlCommand("SELECT user_id, username FROM chatapp_user;", conn);

                conn.Open();
                var rdr = cmdGetUsers.ExecuteReader();

                while(rdr.Read())
                {
                    var user = new User()
                    {
                        User_id = rdr.GetInt32(0),
                        Username = rdr.GetString(1),
                    };

                    users.Add(user);
                }
                rdr.Close();
                conn.Close();
            }
            return users;
        }

    }
}
