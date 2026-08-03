using System.Text.Json.Serialization;

namespace XYZ_shop.Application.Dtos.Rawg
{
    public class RawgGameDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        [JsonPropertyName("background_image")]
        public string BackgroundImage { get; set; }

        public double? Rating { get; set; }
        public DateTime? Released { get; set; }
        public string Description { get; set; }

        [JsonPropertyName("description_raw")]
        public string DescriptionRaw { get; set; }

        public int? Metacritic { get; set; }

        public int? Playtime { get; set; }
        public string Website { get; set; }

        public List<RawgGameGenreDto> Genres { get; set; }
        public List<RawgGamePlatformDto> Platforms { get; set; }

        [JsonPropertyName("short_screenshots")]
        public List<RawgScreenshotDto> ShortScreenshots { get; set; }

        public List<RawgDeveloperDto> Developers { get; set; }
        public List<RawgPublisherDto> Publishers { get; set; }

        [JsonPropertyName("esrb_rating")]
        public RawgEsrbRatingDto EsrbRating { get; set; }

        [JsonIgnore]
        public string ImageUrl =>
            !string.IsNullOrWhiteSpace(BackgroundImage)
                ? BackgroundImage
                : ShortScreenshots?.FirstOrDefault()?.Image;

        [JsonIgnore]
        public string GenreSummary =>
            Genres == null || Genres.Count == 0
                ? string.Empty
                : string.Join(", ", Genres.Take(2).Select(g => g.Name));

        [JsonIgnore]
        public string PublishersSummary =>
            Publishers == null || Publishers.Count == 0
                ? string.Empty
                : string.Join(", ", Publishers.Select(p => p.Name));

        [JsonIgnore]
        public string DevelopersSummary =>
            Developers == null || Developers.Count == 0
                ? string.Empty
                : string.Join(", ", Developers.Select(d => d.Name));
    }
}
