
using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Domain.Entities;
using XYZ_shop.Infrastructure.Data;

namespace XYZ_shop.Infrastructure.Repositories
{
    public class PublisherRepository : BaseRepository<PublisherEntity>, IPublisherRepository
    {
        public PublisherRepository(XyzDbContext context) : base(context)
        {
        }
    }
}
