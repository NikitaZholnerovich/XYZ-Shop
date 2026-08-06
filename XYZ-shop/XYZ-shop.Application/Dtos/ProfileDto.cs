using XYZ_shop.Domain.Enums;

namespace XYZ_shop.Application.Dtos
{
    public class ProfileDto
    {
        public string Login { get; set; } = string.Empty;
        public Language Language { get; set; } = Language.English;
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Mobilephone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
