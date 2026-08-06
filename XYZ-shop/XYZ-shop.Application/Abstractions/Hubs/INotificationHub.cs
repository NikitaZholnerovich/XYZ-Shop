namespace XYZ_shop.Application.Abstractions.Hubs
{
    public interface INotificationHub
    {
        Task NewGameAdded(string gameName, string urlCover);
    }
}
