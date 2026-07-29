
using System.ComponentModel.DataAnnotations;

namespace XYZ_shop.Domain.Entities
{
    public class GameReviewEntity : BaseEntity
    {
        [Required, MinLength(3), MaxLength(5000)]
        public string Text { get; set; }

        [Range(1, 10)]
        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int AuthorId { get; set; }
        public int GameId { get; set; }

        public virtual UserEntity Author { get; set; }
        public virtual GameEntity Game { get; set; }
    }
}
