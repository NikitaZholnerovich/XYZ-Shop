namespace XYZ_shop.Web.Localizations
{
    using System.Globalization;
    using System.Resources;

    public class GameDetailsPage
    {
        private static readonly ResourceManager ResourceManager =
            new ResourceManager("XYZ_shop.Web.Localizations.GameDetailsPage", typeof(GameDetailsPage).Assembly);

        public static string Back_To_Catalog => ResourceManager.GetString(nameof(Back_To_Catalog), CultureInfo.CurrentUICulture)!;
        public static string Edit_Game => ResourceManager.GetString(nameof(Edit_Game), CultureInfo.CurrentUICulture)!;
        public static string Delete_Game => ResourceManager.GetString(nameof(Delete_Game), CultureInfo.CurrentUICulture)!;
        public static string Free => ResourceManager.GetString(nameof(Free), CultureInfo.CurrentUICulture)!;
        public static string About_This_Game => ResourceManager.GetString(nameof(About_This_Game), CultureInfo.CurrentUICulture)!;
        public static string Reviews => ResourceManager.GetString(nameof(Reviews), CultureInfo.CurrentUICulture)!;
        public static string Recommended => ResourceManager.GetString(nameof(Recommended), CultureInfo.CurrentUICulture)!;
        public static string Reviews_Title => ResourceManager.GetString(nameof(Reviews_Title), CultureInfo.CurrentUICulture)!;
        public static string Add_To_Cart => ResourceManager.GetString(nameof(Add_To_Cart), CultureInfo.CurrentUICulture)!;
        public static string Add_To_Wishlist => ResourceManager.GetString(nameof(Add_To_Wishlist), CultureInfo.CurrentUICulture)!;
        public static string Write_A_Review => ResourceManager.GetString(nameof(Write_A_Review), CultureInfo.CurrentUICulture)!;
        public static string Share_Your_Thoughts => ResourceManager.GetString(nameof(Share_Your_Thoughts), CultureInfo.CurrentUICulture)!;
        public static string Rating => ResourceManager.GetString(nameof(Rating), CultureInfo.CurrentUICulture)!;
        public static string Submit => ResourceManager.GetString(nameof(Submit), CultureInfo.CurrentUICulture)!;
        public static string Cancel => ResourceManager.GetString(nameof(Cancel), CultureInfo.CurrentUICulture)!;
        public static string No_Reviews_Yet => ResourceManager.GetString(nameof(No_Reviews_Yet), CultureInfo.CurrentUICulture)!;
        public static string Not_Reviewed => ResourceManager.GetString(nameof(Not_Reviewed), CultureInfo.CurrentUICulture)!;
        public static string Edited => ResourceManager.GetString(nameof(Edited), CultureInfo.CurrentUICulture)!;
        public static string Sort_By => ResourceManager.GetString(nameof(Sort_By), CultureInfo.CurrentUICulture)!;
        public static string Sort_Newest => ResourceManager.GetString(nameof(Sort_Newest), CultureInfo.CurrentUICulture)!;
        public static string Sort_Highest => ResourceManager.GetString(nameof(Sort_Highest), CultureInfo.CurrentUICulture)!;
        public static string Sort_Lowest => ResourceManager.GetString(nameof(Sort_Lowest), CultureInfo.CurrentUICulture)!;
        public static string Edit_Review => ResourceManager.GetString(nameof(Edit_Review), CultureInfo.CurrentUICulture)!;
        public static string Delete_Review => ResourceManager.GetString(nameof(Delete_Review), CultureInfo.CurrentUICulture)!;
        public static string Save_Review => ResourceManager.GetString(nameof(Save_Review), CultureInfo.CurrentUICulture)!;
        public static string Confirm_Delete_Review => ResourceManager.GetString(nameof(Confirm_Delete_Review), CultureInfo.CurrentUICulture)!;
    }
}
