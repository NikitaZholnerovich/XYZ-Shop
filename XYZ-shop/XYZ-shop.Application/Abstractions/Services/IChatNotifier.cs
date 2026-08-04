namespace XYZ_shop.Application.Abstractions.Services
{
    public interface IChatNotifier
    {
        Task NotifyAsync(int userId, string userName, string avatarUrl, string message);
    }
}
