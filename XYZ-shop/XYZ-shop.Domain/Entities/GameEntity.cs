
using System.ComponentModel.DataAnnotations.Schema;

namespace XYZ_shop.Domain.Entities
{
    public class GameEntity : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int PublisherId { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? ModifiedByUserId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public double? AverageRating { get; set; }
        public int? ReviewsCount { get; set; }
        public int? PositiveReviewsCount { get; set; }

        public virtual PublisherEntity Publisher { get; set; }
        public virtual UserEntity CreatedByUser { get; set; }
        public virtual UserEntity ModifiedByUser { get; set; }
        public virtual List<GameReviewEntity> GameReviews { get; set; }
        public virtual List<GameGenreEntity> GameGenres { get; set; }
    }
}
