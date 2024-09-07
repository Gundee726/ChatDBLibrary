using Microsoft.AspNetCore.SignalR;
using Users;

namespace ChatAppAPI
{
    public class Mainhub : Hub
    {

        private List<string> connections = new List<string>();
        private LoggedUsers loggedUsers;
        private ILogger<Mainhub> _logger;
        public Mainhub(LoggedUsers users, ILogger<Mainhub> logger)
        {
            loggedUsers = users;
            //test pull
        }

        public async Task SendMessage(User toUser, string message)
        {
            await Clients.Client(toUser.connectionId).SendAsync("MessageReceived", toUser, message);
        }
        public async override Task OnConnectedAsync()
        {
            // _logger.LogInformation(1001,"User has connected","connectionid=");
            Console.WriteLine($"Connected users = {loggedUsers.users.Count}");



            connections.Add(Context.ConnectionId);
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
        }


    }
}
