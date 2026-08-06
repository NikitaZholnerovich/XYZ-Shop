namespace XYZ_shop.Application.Dtos
{
    public class EditGameReviewResultDto
    {
        public GameReviewOperationStatus Status { get; set; }
        public GameReviewDto? Review { get; set; }
    }
}
