namespace XYZ_shop.Web.Localizations
{
    using System.Globalization;
    using System.Resources;

    public class RecommendationsPage
    {
        private static readonly ResourceManager ResourceManager =
            new ResourceManager("XYZ_shop.Web.Localizations.RecommendationsPage", typeof(RecommendationsPage).Assembly);

        public static string Title => ResourceManager.GetString(nameof(Title), CultureInfo.CurrentUICulture)!;
        public static string Subtitle => ResourceManager.GetString(nameof(Subtitle), CultureInfo.CurrentUICulture)!;
        public static string Search_Placeholder => ResourceManager.GetString(nameof(Search_Placeholder), CultureInfo.CurrentUICulture)!;
        public static string Search => ResourceManager.GetString(nameof(Search), CultureInfo.CurrentUICulture)!;
        public static string Results_For => ResourceManager.GetString(nameof(Results_For), CultureInfo.CurrentUICulture)!;
        public static string No_Image => ResourceManager.GetString(nameof(No_Image), CultureInfo.CurrentUICulture)!;
        public static string Games_In_Series => ResourceManager.GetString(nameof(Games_In_Series), CultureInfo.CurrentUICulture)!;
        public static string No_Games_Found => ResourceManager.GetString(nameof(No_Games_Found), CultureInfo.CurrentUICulture)!;
        public static string Try_Different => ResourceManager.GetString(nameof(Try_Different), CultureInfo.CurrentUICulture)!;
        public static string Popular_Games => ResourceManager.GetString(nameof(Popular_Games), CultureInfo.CurrentUICulture)!;
        public static string New_Releases => ResourceManager.GetString(nameof(New_Releases), CultureInfo.CurrentUICulture)!;
        public static string Rating => ResourceManager.GetString(nameof(Rating), CultureInfo.CurrentUICulture)!;
    }
}
