namespace XYZ_shop.Web.Localizations
{
    using System.Globalization;
    using System.Resources;

    public class ProfilePage
    {
        private static readonly ResourceManager ResourceManager =
            new ResourceManager("XYZ_shop.Web.Localizations.ProfilePage", typeof(ProfilePage).Assembly);

        public static string Title => ResourceManager.GetString(nameof(Title), CultureInfo.CurrentUICulture)!;
        public static string Avatar => ResourceManager.GetString(nameof(Avatar), CultureInfo.CurrentUICulture)!;
        public static string Email => ResourceManager.GetString(nameof(Email), CultureInfo.CurrentUICulture)!;
        public static string First_Name => ResourceManager.GetString(nameof(First_Name), CultureInfo.CurrentUICulture)!;
        public static string Last_Name => ResourceManager.GetString(nameof(Last_Name), CultureInfo.CurrentUICulture)!;
        public static string Phone => ResourceManager.GetString(nameof(Phone), CultureInfo.CurrentUICulture)!;
        public static string Birth_Date => ResourceManager.GetString(nameof(Birth_Date), CultureInfo.CurrentUICulture)!;
        public static string Language => ResourceManager.GetString(nameof(Language), CultureInfo.CurrentUICulture)!;
        public static string English => ResourceManager.GetString(nameof(English), CultureInfo.CurrentUICulture)!;
        public static string Russian => ResourceManager.GetString(nameof(Russian), CultureInfo.CurrentUICulture)!;
        public static string Save => ResourceManager.GetString(nameof(Save), CultureInfo.CurrentUICulture)!;
    }
}
