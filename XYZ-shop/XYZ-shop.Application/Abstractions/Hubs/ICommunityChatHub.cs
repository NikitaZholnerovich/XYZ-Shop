namespace XYZ_shop.Application.Abstractions.Hubs
{
    public interface ICommunityChatHub
    {
        Task SendChatMessage(int userId, string userName, string avatarUrl, string message);
    }
}
