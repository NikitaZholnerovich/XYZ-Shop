using Microsoft.AspNetCore.SignalR;
using XYZ_shop.Application.Abstractions.Hubs;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Web.Hubs;

namespace XYZ_shop.Web.Services
{
    public class ChatNotifier : IChatNotifier
    {
        private readonly IHubContext<CommunityChatHub, ICommunityChatHub> _hub;

        public ChatNotifier(IHubContext<CommunityChatHub, ICommunityChatHub> hub)
        {
            _hub = hub;
        }

        public Task NotifyAsync(int userId, string userName, string avatarUrl, string message)
        {
            return _hub.Clients.All.SendChatMessage(userId, userName, avatarUrl, message);
        }
    }
}
