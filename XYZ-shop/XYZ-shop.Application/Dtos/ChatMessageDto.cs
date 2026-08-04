namespace XYZ_shop.Application.Dtos
{
    public class ChatMessageDto
    {
        public string Message { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = "/images/default-avatar.png";
        public bool IsOwnMessage { get; set; }
        public int UserId { get; set; }
    }
}
