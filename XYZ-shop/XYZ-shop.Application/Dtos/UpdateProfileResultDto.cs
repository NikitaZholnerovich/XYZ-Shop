namespace XYZ_shop.Application.Dtos
{
    public class UpdateProfileResultDto
    {
        public bool Success { get; set; }
        public bool LanguageChanged { get; set; }
        public ProfileDto? Profile { get; set; }
    }
}
