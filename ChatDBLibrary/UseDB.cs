using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatDBLibrary.Model;

namespace ChatDBLibrary
{
    public class UseDB
    {
        private DBContext db;
        public UseDB()
        {
            db = new("Server=127.0.0.1; Port=5432; Database=public; User Id=postgres; Password=heroO726;");
        }   
    }
}
