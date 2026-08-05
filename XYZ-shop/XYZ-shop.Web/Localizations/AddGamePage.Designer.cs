namespace XYZ_shop.Web.Localizations
{
    using System.Globalization;
    using System.Resources;

    public class AddGamePage
    {
        private static readonly ResourceManager ResourceManager =
            new ResourceManager("XYZ_shop.Web.Localizations.AddGamePage", typeof(AddGamePage).Assembly);

        public static string Title => ResourceManager.GetString(nameof(Title), CultureInfo.CurrentUICulture)!;
        public static string Back_To_Catalog => ResourceManager.GetString(nameof(Back_To_Catalog), CultureInfo.CurrentUICulture)!;
        public static string Label_Title => ResourceManager.GetString(nameof(Label_Title), CultureInfo.CurrentUICulture)!;
        public static string Placeholder_Title => ResourceManager.GetString(nameof(Placeholder_Title), CultureInfo.CurrentUICulture)!;
        public static string Image_URL => ResourceManager.GetString(nameof(Image_URL), CultureInfo.CurrentUICulture)!;
        public static string Description => ResourceManager.GetString(nameof(Description), CultureInfo.CurrentUICulture)!;
        public static string Placeholder_Description => ResourceManager.GetString(nameof(Placeholder_Description), CultureInfo.CurrentUICulture)!;
        public static string Price => ResourceManager.GetString(nameof(Price), CultureInfo.CurrentUICulture)!;
        public static string Genres => ResourceManager.GetString(nameof(Genres), CultureInfo.CurrentUICulture)!;
        public static string Hold_Ctrl => ResourceManager.GetString(nameof(Hold_Ctrl), CultureInfo.CurrentUICulture)!;
        public static string Publisher => ResourceManager.GetString(nameof(Publisher), CultureInfo.CurrentUICulture)!;
        public static string Select_Publisher => ResourceManager.GetString(nameof(Select_Publisher), CultureInfo.CurrentUICulture)!;
        public static string Save_Game => ResourceManager.GetString(nameof(Save_Game), CultureInfo.CurrentUICulture)!;
        public static string Cancel => ResourceManager.GetString(nameof(Cancel), CultureInfo.CurrentUICulture)!;
    }
}
