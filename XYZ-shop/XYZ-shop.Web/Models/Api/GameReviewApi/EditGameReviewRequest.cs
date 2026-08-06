namespace XYZ_shop.Web.Models.Api.GameReviewApi
{
    public class EditGameReviewRequest
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}
