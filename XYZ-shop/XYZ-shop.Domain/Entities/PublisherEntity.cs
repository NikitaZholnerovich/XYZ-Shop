
namespace XYZ_shop.Domain.Entities
{
    public class PublisherEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual List<GameEntity> Games { get; set; }
    }
}