
namespace XYZ_shop.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string? AvatarUrl { get; set; }
        public int? UserProfileId { get; set; }

        public virtual List<GameReviewEntity> Reviews { get; set; }
        public virtual UserProfileEntity? UserProfile { get; set; }
        public virtual List<UserEntity> MyFriends { get; set; }
        public virtual List<UserEntity> WhoIsMyFriends { get; set; }
        public virtual List<GameEntity> CreatedGames { get; set; }
        public virtual List<GameEntity> ModifiedGames { get; set; }
        public virtual List<CommunityChatMessageEntity> CommunityChatMessages { get; set; }
    }
}
