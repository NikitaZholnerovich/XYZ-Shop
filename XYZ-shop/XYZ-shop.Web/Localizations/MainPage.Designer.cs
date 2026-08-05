namespace XYZ_shop.Web.Localizations
{
    using System.Globalization;
    using System.Resources;

    public class MainPage
    {
        private static readonly ResourceManager ResourceManager =
            new ResourceManager("XYZ_shop.Web.Localizations.MainPage", typeof(MainPage).Assembly);

        public static string Title => ResourceManager.GetString(nameof(Title), CultureInfo.CurrentUICulture)!;
        public static string Featured => ResourceManager.GetString(nameof(Featured), CultureInfo.CurrentUICulture)!;
        public static string Special_Offers => ResourceManager.GetString(nameof(Special_Offers), CultureInfo.CurrentUICulture)!;
        public static string View => ResourceManager.GetString(nameof(View), CultureInfo.CurrentUICulture)!;
        public static string Add_To_Cart => ResourceManager.GetString(nameof(Add_To_Cart), CultureInfo.CurrentUICulture)!;
        public static string Catalog => ResourceManager.GetString(nameof(Catalog), CultureInfo.CurrentUICulture)!;
    }
}
