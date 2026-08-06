namespace XYZ_shop.Web.Models
{
    public class GameReviewViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int Rating { get; set; }
        public bool IsRecommended { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string? AuthorAvatarUrl { get; set; }
        public int AuthorId { get; set; }
        public int GameId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool CanManage { get; set; }
    }
}
