using XYZ_shop.Application.Dtos;

namespace XYZ_shop.Application.Abstractions.Services
{
    public interface IGameReviewService
    {
        AddGameReviewResultDto Add(AddGameReviewDto dto);
        EditGameReviewResultDto Edit(EditGameReviewDto dto);
        DeleteGameReviewResultDto Delete(int reviewId);
    }
}
