using XYZ_shop.Application.Dtos;

namespace XYZ_shop.Application.Abstractions.Services
{
    public interface IChatService
    {
        Task AddChatMessage(string message);
        List<ChatMessageDto> GetMessages();
    }
}
