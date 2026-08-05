namespace XYZ_shop.Web.Localizations
{
    using System.Globalization;
    using System.Resources;

    public class CommunityChatPage
    {
        private static readonly ResourceManager ResourceManager =
            new ResourceManager("XYZ_shop.Web.Localizations.CommunityChatPage", typeof(CommunityChatPage).Assembly);

        public static string Title => ResourceManager.GetString(nameof(Title), CultureInfo.CurrentUICulture)!;
        public static string Write_A_Message => ResourceManager.GetString(nameof(Write_A_Message), CultureInfo.CurrentUICulture)!;
        public static string Send => ResourceManager.GetString(nameof(Send), CultureInfo.CurrentUICulture)!;
    }
}
