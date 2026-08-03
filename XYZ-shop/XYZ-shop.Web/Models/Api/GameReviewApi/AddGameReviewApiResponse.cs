namespace XYZ_shop.Web.Models.Api.GameReviewApi
{
    public class AddGameReviewApiResponse : BaseApiResponse
    {
        public string Author { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
