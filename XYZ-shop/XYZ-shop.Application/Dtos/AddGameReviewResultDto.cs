namespace XYZ_shop.Application.Dtos
{
    public class AddGameReviewResultDto
    {
        public GameReviewOperationStatus Status { get; set; }
        public GameReviewDto? Review { get; set; }
    }
}
