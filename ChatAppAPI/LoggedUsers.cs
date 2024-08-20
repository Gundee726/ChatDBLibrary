using System.Collections.Concurrent;

namespace ChatAppAPI
{
    public class LoggedUsers
    {
        public ConcurrentDictionary<int, UserContext> users;

        public LoggedUsers()
        {
            users = new ConcurrentDictionary<int, UserContext>();
        }
    }
}
