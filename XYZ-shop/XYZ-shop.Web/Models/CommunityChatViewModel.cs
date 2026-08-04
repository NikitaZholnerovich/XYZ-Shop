using XYZ_shop.Application.Dtos;

namespace XYZ_shop.Web.Models
{
    public class CommunityChatViewModel
    {
        public List<ChatMessageDto> ChatMessages { get; set; } = new();
        public int CurrentUserId { get; set; }
    }
}
