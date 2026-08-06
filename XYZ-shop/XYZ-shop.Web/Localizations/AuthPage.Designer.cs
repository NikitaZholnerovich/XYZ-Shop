namespace XYZ_shop.Web.Localizations
{
    using System.Globalization;
    using System.Resources;

    public class AuthPage
    {
        private static readonly ResourceManager ResourceManager =
            new ResourceManager("XYZ_shop.Web.Localizations.AuthPage", typeof(AuthPage).Assembly);

        public static string Login_Title => ResourceManager.GetString(nameof(Login_Title), CultureInfo.CurrentUICulture)!;
        public static string Register_Title => ResourceManager.GetString(nameof(Register_Title), CultureInfo.CurrentUICulture)!;
        public static string Login => ResourceManager.GetString(nameof(Login), CultureInfo.CurrentUICulture)!;
        public static string Password => ResourceManager.GetString(nameof(Password), CultureInfo.CurrentUICulture)!;
        public static string Register => ResourceManager.GetString(nameof(Register), CultureInfo.CurrentUICulture)!;
        public static string Back_To_Login => ResourceManager.GetString(nameof(Back_To_Login), CultureInfo.CurrentUICulture)!;
        public static string Deny_Message => ResourceManager.GetString(nameof(Deny_Message), CultureInfo.CurrentUICulture)!;
    }
}
