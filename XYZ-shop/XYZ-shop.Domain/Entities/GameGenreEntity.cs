
namespace XYZ_shop.Domain.Entities
{
    public class GameGenreEntity : BaseEntity
    {
        public string Name { get; set; }

        public virtual List<GameEntity> Games { get; set; }
    }
}
