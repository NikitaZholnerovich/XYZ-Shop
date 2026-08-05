namespace XYZ_shop.Web.Localizations
{
    using System.Globalization;
    using System.Resources;

    public class LayoutPage
    {
        private static readonly ResourceManager ResourceManager =
            new ResourceManager("XYZ_shop.Web.Localizations.LayoutPage", typeof(LayoutPage).Assembly);

        public static string Nav_Steam => ResourceManager.GetString(nameof(Nav_Steam), CultureInfo.CurrentUICulture)!;
        public static string Nav_Catalog => ResourceManager.GetString(nameof(Nav_Catalog), CultureInfo.CurrentUICulture)!;
        public static string Nav_CommunityChat => ResourceManager.GetString(nameof(Nav_CommunityChat), CultureInfo.CurrentUICulture)!;
        public static string Nav_Recommendations => ResourceManager.GetString(nameof(Nav_Recommendations), CultureInfo.CurrentUICulture)!;
        public static string Nav_About => ResourceManager.GetString(nameof(Nav_About), CultureInfo.CurrentUICulture)!;
        public static string Logout => ResourceManager.GetString(nameof(Logout), CultureInfo.CurrentUICulture)!;
        public static string Login => ResourceManager.GetString(nameof(Login), CultureInfo.CurrentUICulture)!;
        public static string Register => ResourceManager.GetString(nameof(Register), CultureInfo.CurrentUICulture)!;
        public static string Search_Placeholder => ResourceManager.GetString(nameof(Search_Placeholder), CultureInfo.CurrentUICulture)!;
        public static string Search_Button => ResourceManager.GetString(nameof(Search_Button), CultureInfo.CurrentUICulture)!;
        public static string Footer_About_Company => ResourceManager.GetString(nameof(Footer_About_Company), CultureInfo.CurrentUICulture)!;
        public static string Footer_Jobs => ResourceManager.GetString(nameof(Footer_Jobs), CultureInfo.CurrentUICulture)!;
        public static string Footer_Steamworks => ResourceManager.GetString(nameof(Footer_Steamworks), CultureInfo.CurrentUICulture)!;
        public static string Footer_Support => ResourceManager.GetString(nameof(Footer_Support), CultureInfo.CurrentUICulture)!;
        public static string Footer_Privacy_Policy => ResourceManager.GetString(nameof(Footer_Privacy_Policy), CultureInfo.CurrentUICulture)!;
        public static string Footer_Legal => ResourceManager.GetString(nameof(Footer_Legal), CultureInfo.CurrentUICulture)!;
        public static string Footer_Copyright => ResourceManager.GetString(nameof(Footer_Copyright), CultureInfo.CurrentUICulture)!;
    }
}
