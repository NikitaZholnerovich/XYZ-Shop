namespace XYZ_shop.Web.Models.Api.GameReviewApi
{
    public class AddGameReviewRequest
    {
        public int GameId { get; set; }
        public string Text { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}
