
using Microsoft.EntityFrameworkCore;
using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Domain.Entities;
using XYZ_shop.Infrastructure.Data;

namespace XYZ_shop.Infrastructure.Repositories
{
    public class CommunityChatMessageRepository : BaseRepository<CommunityChatMessageEntity>, ICommunityChatMessageRepository
    {
        public CommunityChatMessageRepository(XyzDbContext context) : base(context)
        {
        }

        public List<CommunityChatMessageEntity> GetAllMessagesWithUsers()
        {
            return _dbSet
                .Include(x => x.CreatedByUser)
                .ToList();
        }
    }
}
