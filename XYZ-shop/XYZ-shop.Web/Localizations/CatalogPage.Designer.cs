namespace XYZ_shop.Web.Localizations
{
    using System.Globalization;
    using System.Resources;

    public class CatalogPage
    {
        private static readonly ResourceManager ResourceManager =
            new ResourceManager("XYZ_shop.Web.Localizations.CatalogPage", typeof(CatalogPage).Assembly);

        public static string Title => ResourceManager.GetString(nameof(Title), CultureInfo.CurrentUICulture)!;
        public static string Back_To_Store_Button => ResourceManager.GetString(nameof(Back_To_Store_Button), CultureInfo.CurrentUICulture)!;
        public static string Add_New_Game_Button => ResourceManager.GetString(nameof(Add_New_Game_Button), CultureInfo.CurrentUICulture)!;
        public static string Filter => ResourceManager.GetString(nameof(Filter), CultureInfo.CurrentUICulture)!;
        public static string Genre => ResourceManager.GetString(nameof(Genre), CultureInfo.CurrentUICulture)!;
        public static string All_Genres => ResourceManager.GetString(nameof(All_Genres), CultureInfo.CurrentUICulture)!;
        public static string Publisher => ResourceManager.GetString(nameof(Publisher), CultureInfo.CurrentUICulture)!;
        public static string All_Publishers => ResourceManager.GetString(nameof(All_Publishers), CultureInfo.CurrentUICulture)!;
        public static string Max_Price => ResourceManager.GetString(nameof(Max_Price), CultureInfo.CurrentUICulture)!;
        public static string Sort_By => ResourceManager.GetString(nameof(Sort_By), CultureInfo.CurrentUICulture)!;
        public static string Default => ResourceManager.GetString(nameof(Default), CultureInfo.CurrentUICulture)!;
        public static string Title_Sort => ResourceManager.GetString(nameof(Title_Sort), CultureInfo.CurrentUICulture)!;
        public static string Price_Sort => ResourceManager.GetString(nameof(Price_Sort), CultureInfo.CurrentUICulture)!;
        public static string Direction => ResourceManager.GetString(nameof(Direction), CultureInfo.CurrentUICulture)!;
        public static string Ascending => ResourceManager.GetString(nameof(Ascending), CultureInfo.CurrentUICulture)!;
        public static string Descending => ResourceManager.GetString(nameof(Descending), CultureInfo.CurrentUICulture)!;
        public static string Apply => ResourceManager.GetString(nameof(Apply), CultureInfo.CurrentUICulture)!;
        public static string Reset => ResourceManager.GetString(nameof(Reset), CultureInfo.CurrentUICulture)!;
        public static string Delete_Game => ResourceManager.GetString(nameof(Delete_Game), CultureInfo.CurrentUICulture)!;
        public static string All_Games => ResourceManager.GetString(nameof(All_Games), CultureInfo.CurrentUICulture)!;
        public static string View => ResourceManager.GetString(nameof(View), CultureInfo.CurrentUICulture)!;
        public static string Add_To_Cart => ResourceManager.GetString(nameof(Add_To_Cart), CultureInfo.CurrentUICulture)!;
        public static string No_Games_Found => ResourceManager.GetString(nameof(No_Games_Found), CultureInfo.CurrentUICulture)!;
        public static string Try_Changing_Filters => ResourceManager.GetString(nameof(Try_Changing_Filters), CultureInfo.CurrentUICulture)!;
        public static string Prev => ResourceManager.GetString(nameof(Prev), CultureInfo.CurrentUICulture)!;
        public static string Next => ResourceManager.GetString(nameof(Next), CultureInfo.CurrentUICulture)!;
    }
}
