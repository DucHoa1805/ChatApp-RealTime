using ChatApp_RealTime.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp_RealTime.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private static readonly Dictionary<string, string> _connectedUsers = new();

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            var username = Context.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(username))
            {
                _connectedUsers[Context.ConnectionId] = username;
                await Clients.All.SendAsync("UserConnected", username);
                await Clients.Caller.SendAsync("OnlineUsers", _connectedUsers.Values.Distinct().ToList());
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (_connectedUsers.TryGetValue(Context.ConnectionId, out var username))
            {
                _connectedUsers.Remove(Context.ConnectionId);
                await Clients.All.SendAsync("UserDisconnected", username);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string receiverUsername, string content)
        {
            var senderUsername = Context.User?.Identity?.Name;
            if (string.IsNullOrEmpty(senderUsername))
                return;

            var sender = await _chatService.GetUserByUsernameAsync(senderUsername);
            var receiver = await _chatService.GetUserByUsernameAsync(receiverUsername);

            if (sender == null || receiver == null)
                return;

            var message = await _chatService.SendMessageAsync(sender.Id, receiver.Id, content);

            var payload = new
            {
                id = message.Id,
                senderUsername = sender.Username,
                receiverUsername = receiver.Username,
                content = message.Content,
                timestamp = message.Timestamp.ToString("o")
            };

            // Send to sender and all connections of the receiver
            await Clients.Caller.SendAsync("ReceiveMessage", payload);

            var receiverConnections = _connectedUsers
                .Where(kv => kv.Value == receiverUsername)
                .Select(kv => kv.Key)
                .ToList();

            if (receiverConnections.Count > 0)
                await Clients.Clients(receiverConnections).SendAsync("ReceiveMessage", payload);
        }
    }
}
