using Microsoft.AspNetCore.SignalR;

namespace BlazorChatSignalR.Server.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> Users = new Dictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            string username = Context.GetHttpContext().Request.Query["username"];
            Users.Add(Context.ConnectionId,username);
            await AddMessageToChat(string.Empty, $"{username} joined the party!");
            await AddMessageToChat("", "User connected!");
            await base.OnConnectedAsync();
        }

        public async Task OnDisconnectedAsync(Exception? exception)
        {
            string username = Users.FirstOrDefault(z => z.Key == Context.ConnectionId).Value;
            await AddMessageToChat(string.Empty, $"{username} left!");
        }

        public async Task AddMessageToChat(string user, string message)
        {
            await Clients.All.SendAsync("GetThatMessageDude", user, message);
        }
    }
}
