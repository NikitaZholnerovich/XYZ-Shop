namespace XYZ_shop.Web.Models
{
    public class SteamHomeViewModel
    {
        public List<SteamGameViewModel> Featured { get; set; } = new();
        public List<SteamGameViewModel> SpecialOffers { get; set; } = new();
    }
}
