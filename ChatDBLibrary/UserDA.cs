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

        public IEnumerable<User> GetUsers(int currentUserId)
        {
            List<User> users = new List<User>();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                var cmdGetUsers = new NpgsqlCommand(@"
            SELECT DISTINCT u.user_id, u.username
            FROM chatapp_user u
            JOIN chathistory ch ON u.user_id = ch.receiver_id OR u.user_id = ch.sender_id
            WHERE (ch.sender_id = @currentUserId OR ch.receiver_id = @currentUserId)
              AND u.user_id != @currentUserId;", conn);

                cmdGetUsers.Parameters.AddWithValue("currentUserId", currentUserId);

                conn.Open();
                var rdr = cmdGetUsers.ExecuteReader();

                while (rdr.Read())
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


        public IEnumerable<Messages> GetMessages()
        {
            List<Messages> messages = new List<Messages>();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                var cmdGetMessages = new NpgsqlCommand("SELECT ch.sender_id, sender.username AS sender_name, ch.receiver_id, receiver.username AS receiver_name FROM chathistory ch JOIN chatapp_user sender ON ch.sender_id = sender.user_id JOIN chatapp_user receiver ON ch.receiver_id = receiver.user_id;", conn);

                conn.Open();
                var rdr = cmdGetMessages.ExecuteReader();

                while (rdr.Read())
                {
                    var message = new Messages()
                    {
                        Sender_id = rdr.GetInt32(0),
                        
                        Receiver_id = rdr.GetInt32(2),
                        
                    };

                    messages.Add(message);
                }
                rdr.Close();
                conn.Close();
            }
            return messages;
        }

        public IEnumerable<Messages> GetMessages(int senderId, int receiverId)
        {
            List<Messages> messages = new List<Messages>();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                var cmdGetMessages = new NpgsqlCommand(@"
            SELECT ch.sender_id, sender.username AS sender_name, ch.receiver_id, receiver.username AS receiver_name, ch.message
            FROM chathistory ch
            JOIN chatapp_user sender ON ch.sender_id = sender.user_id
            JOIN chatapp_user receiver ON ch.receiver_id = receiver.user_id
            WHERE (ch.sender_id = @senderId AND ch.receiver_id = @receiverId)
               OR (ch.sender_id = @receiverId AND ch.receiver_id = @senderId);", conn);

                cmdGetMessages.Parameters.AddWithValue("senderId", senderId);
                cmdGetMessages.Parameters.AddWithValue("receiverId", receiverId);

                conn.Open();
                var rdr = cmdGetMessages.ExecuteReader();

                while (rdr.Read())
                {
                    var message = new Messages()
                    {
                        Sender_id = rdr.GetInt32(0),
                        
                        Receiver_id = rdr.GetInt32(2),
                        
                        Message = rdr.GetString(4)
                    };

                    messages.Add(message);
                }
                rdr.Close();
                conn.Close();
            }
            return messages;
        }


        public void AddMessage(Messages message)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                var cmdAddMessage = new NpgsqlCommand(@"
            INSERT INTO chathistory (sender_id, receiver_id, message) 
            VALUES (@sender_id, @receiver_id, @message);", conn);

                cmdAddMessage.Parameters.AddWithValue("sender_id", message.Sender_id);
                cmdAddMessage.Parameters.AddWithValue("receiver_id", message.Receiver_id);
                cmdAddMessage.Parameters.AddWithValue("message", message.Message);

                conn.Open();
                cmdAddMessage.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void ReactToMessage(int messageId, int userId, string reaction)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                var cmdReactToMessage = new NpgsqlCommand(@"
                    INSERT INTO reactions (message_id, user_id, reaction) 
                    VALUES (@message_id, @user_id, @reaction);", conn);

                cmdReactToMessage.Parameters.AddWithValue("message_id", messageId);
                cmdReactToMessage.Parameters.AddWithValue("user_id", userId);
                cmdReactToMessage.Parameters.AddWithValue("reaction", reaction);

                conn.Open();
                cmdReactToMessage.ExecuteNonQuery();
                conn.Close();
            }
        }

    }
}
