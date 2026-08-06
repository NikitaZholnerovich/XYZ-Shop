using XYZ_shop.Application.Dtos;

namespace XYZ_shop.Application.Abstractions.Services
{
    public interface IProfileService
    {
        ProfileDto? GetProfile(int userId);
        UpdateProfileResultDto UpdateProfile(int userId, UpdateProfileDto dto);
    }
}
