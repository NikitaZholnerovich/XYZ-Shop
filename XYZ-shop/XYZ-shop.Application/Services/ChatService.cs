using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Application.Dtos;
using XYZ_shop.Domain.Entities;

namespace XYZ_shop.Application.Services
{
    public class ChatService : IChatService
    {
        private const string DefaultAvatarUrl = "/images/default-avatar.png";

        private readonly IAuthService _authService;
        private readonly ICommunityChatMessageRepository _communityChatMessageRepository;
        private readonly IChatNotifier _chatNotifier;

        public ChatService(
            ICommunityChatMessageRepository communityChatMessageRepository,
            IAuthService authService,
            IChatNotifier chatNotifier)
        {
            _communityChatMessageRepository = communityChatMessageRepository;
            _authService = authService;
            _chatNotifier = chatNotifier;
        }

        public async Task AddChatMessage(string message)
        {
            var user = _authService.GetUser()!;
            var avatarUrl = ResolveAvatarUrl(user.AvatarUrl);

            var newMessage = new CommunityChatMessageEntity
            {
                MessageText = message,
                CreatedAt = DateTime.UtcNow,
                UserId = user.Id,
                CreatedByUser = user
            };

            _communityChatMessageRepository.Add(newMessage);
            await _chatNotifier.NotifyAsync(user.Id, user.Login, avatarUrl, message);
        }

        public List<ChatMessageDto> GetMessages()
        {
            var currentUserId = _authService.GetUserId();

            return _communityChatMessageRepository.GetAllMessagesWithUsers()
                .Select(x => new ChatMessageDto
                {
                    Message = x.MessageText,
                    TimeStamp = x.CreatedAt,
                    UserName = x.CreatedByUser.Login,
                    AvatarUrl = ResolveAvatarUrl(x.CreatedByUser.AvatarUrl),
                    UserId = x.CreatedByUser.Id,
                    IsOwnMessage = currentUserId == x.CreatedByUser.Id
                })
                .ToList();
        }

        private static string ResolveAvatarUrl(string? avatarUrl)
        {
            return string.IsNullOrWhiteSpace(avatarUrl) ? DefaultAvatarUrl : avatarUrl;
        }
    }
}
