
using XYZ_shop.Domain.Entities;

namespace XYZ_shop.Application.Abstractions.Repositories
{
    public interface ICommunityChatMessageRepository : IBaseRepository<CommunityChatMessageEntity>
    {
        List<CommunityChatMessageEntity> GetAllMessagesWithUsers();
    }
}
