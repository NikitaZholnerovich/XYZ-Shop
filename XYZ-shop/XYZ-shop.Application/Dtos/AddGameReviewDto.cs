namespace XYZ_shop.Application.Dtos
{
    public class AddGameReviewDto
    {
        public int GameId { get; set; }
        public string Text { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}
