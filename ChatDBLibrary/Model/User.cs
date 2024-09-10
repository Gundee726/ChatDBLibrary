using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatDBLibrary.Model
{
    public class User
    {
        [Column("user_id")]
        public int User_id { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("avatar")]
        public string Avatar { get; set; }
        public DateTime? LastActiveDate { get; set; }
    }

    public class CurrentUserRequest
    {
        public string CurrentUserId { get; set; }
    }

    public class MessagesRequest
    {
        public int Sender_id { get; set; }
        public int Receiver_id { get; set; }
    }

    public class LastActiveDates
    {
        [Column("user_id")]
        public int User_id { get; set; }
        [Column("last_active_date")]
        public DateTime LastActiveDate { get; set; } 
    }

    public class Messages
    {
        public int chat_id { get; set; }
        [Column("sender_id")]
        public int Sender_id { get; set; }
        //public string Sender_name { get; set; }
        [Column("receiver_id")]
        public int Receiver_id { get; set; }
        //public string Receiver_name { get; set; }
        [Column("message")]
        public string Message { get; set; }
    }
}
