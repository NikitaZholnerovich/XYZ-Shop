namespace XYZ_shop.Application.Dtos
{
    public class AddGameDto
    {
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int PublisherId { get; set; }
        public List<int> SelectedGenreIds { get; set; } = new();
    }
}
