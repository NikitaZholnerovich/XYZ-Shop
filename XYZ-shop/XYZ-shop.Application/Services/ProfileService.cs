using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Application.Dtos;
using XYZ_shop.Domain.Entities;
using XYZ_shop.Domain.Enums;

namespace XYZ_shop.Application.Services
{
    public class ProfileService : IProfileService
    {
        private const string DefaultAvatarUrl = "/images/default-avatar.png";

        private readonly IUserRepository _userRepository;

        public ProfileService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ProfileDto? GetProfile(int userId)
        {
            var user = _userRepository.GetWithProfile(userId);
            return user == null ? null : ToDto(user);
        }

        public UpdateProfileResultDto UpdateProfile(int userId, UpdateProfileDto dto)
        {
            var user = _userRepository.GetWithProfile(userId);
            if (user == null)
            {
                return new UpdateProfileResultDto { Success = false };
            }

            var language = Enum.IsDefined(typeof(Language), dto.Language)
                ? dto.Language
                : user.Language;

            _userRepository.UpdateProfile(new UserEntity
            {
                Id = userId,
                AvatarUrl = dto.NewAvatarUrl,
                UserProfile = new UserProfileEntity
                {
                    Email = dto.Email.Trim(),
                    FirstName = dto.FirstName?.Trim(),
                    LastName = dto.LastName?.Trim(),
                    Mobilephone = dto.Mobilephone?.Trim(),
                    BirthDate = dto.BirthDate,
                }
            });

            var languageChanged = language is Language.English or Language.Russian
                && language != user.Language;

            if (languageChanged)
            {
                _userRepository.UpdateLanguage(userId, language);
                user.Language = language;
            }

            var updated = _userRepository.GetWithProfile(userId);
            return new UpdateProfileResultDto
            {
                Success = true,
                LanguageChanged = languageChanged,
                Profile = updated == null ? null : ToDto(updated)
            };
        }

        private static ProfileDto ToDto(UserEntity user)
        {
            return new ProfileDto
            {
                Login = user.Login,
                Language = user.Language,
                Email = user.UserProfile?.Email ?? string.Empty,
                FirstName = user.UserProfile?.FirstName,
                LastName = user.UserProfile?.LastName,
                Mobilephone = user.UserProfile?.Mobilephone,
                BirthDate = user.UserProfile?.BirthDate,
                AvatarUrl = string.IsNullOrWhiteSpace(user.AvatarUrl) ? DefaultAvatarUrl : user.AvatarUrl,
            };
        }
    }
}
