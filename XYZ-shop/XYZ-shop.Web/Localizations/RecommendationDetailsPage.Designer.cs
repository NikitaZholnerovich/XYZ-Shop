namespace XYZ_shop.Web.Localizations
{
    using System.Globalization;
    using System.Resources;

    public class RecommendationDetailsPage
    {
        private static readonly ResourceManager ResourceManager =
            new ResourceManager("XYZ_shop.Web.Localizations.RecommendationDetailsPage", typeof(RecommendationDetailsPage).Assembly);

        public static string Back_To_Recommendations => ResourceManager.GetString(nameof(Back_To_Recommendations), CultureInfo.CurrentUICulture)!;
        public static string No_Image => ResourceManager.GetString(nameof(No_Image), CultureInfo.CurrentUICulture)!;
        public static string Genres => ResourceManager.GetString(nameof(Genres), CultureInfo.CurrentUICulture)!;
        public static string Publishers => ResourceManager.GetString(nameof(Publishers), CultureInfo.CurrentUICulture)!;
        public static string Developers => ResourceManager.GetString(nameof(Developers), CultureInfo.CurrentUICulture)!;
        public static string Visit_Official_Website => ResourceManager.GetString(nameof(Visit_Official_Website), CultureInfo.CurrentUICulture)!;
        public static string About_This_Game => ResourceManager.GetString(nameof(About_This_Game), CultureInfo.CurrentUICulture)!;
        public static string Games_In_This_Series => ResourceManager.GetString(nameof(Games_In_This_Series), CultureInfo.CurrentUICulture)!;
        public static string No_Series => ResourceManager.GetString(nameof(No_Series), CultureInfo.CurrentUICulture)!;
        public static string Game_Not_Found => ResourceManager.GetString(nameof(Game_Not_Found), CultureInfo.CurrentUICulture)!;
        public static string Could_Not_Be_Found => ResourceManager.GetString(nameof(Could_Not_Be_Found), CultureInfo.CurrentUICulture)!;
        public static string Hours => ResourceManager.GetString(nameof(Hours), CultureInfo.CurrentUICulture)!;
    }
}
