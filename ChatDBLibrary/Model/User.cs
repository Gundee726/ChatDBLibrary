using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatDBLibrary.Model
{
    public class User
    {
        public int User_id { get; set; }

        public string Username { get; set; }
    }

    public class Messages
    {
        public int Sender_id { get; set; }
        public string Sender_name { get; set; }
        public int Receiver_id { get; set; }
        public string Receiver_name { get; set; }
        public string Message { get; set; }
    }
}
