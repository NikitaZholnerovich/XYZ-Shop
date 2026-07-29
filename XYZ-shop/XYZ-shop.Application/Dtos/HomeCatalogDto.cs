namespace XYZ_shop.Application.Dtos
{
    public class HomeCatalogDto
    {
        public List<GameDto> Featured { get; set; } = new();
        public List<GameDto> SpecialOffers { get; set; } = new();
    }
}
