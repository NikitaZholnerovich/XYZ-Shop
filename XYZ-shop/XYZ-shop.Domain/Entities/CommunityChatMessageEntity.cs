
namespace XYZ_shop.Domain.Entities
{
    public class CommunityChatMessageEntity : BaseEntity
    {
        public string MessageText { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }

        public virtual UserEntity CreatedByUser { get; set; }
    }
}
