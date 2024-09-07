using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//hi
namespace ChatDBLibrary
{
    public class DBContext
    {
        public string ConnectionString {  get; set; }

        public UserDA user;

        public DBContext(string connectionString) {
            this.ConnectionString = connectionString;
            user = new UserDA(connectionString);
        }
    }
}
