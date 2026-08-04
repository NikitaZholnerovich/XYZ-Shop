
using XYZ_shop.Domain.Entities;
using XYZ_shop.Domain.Enums;

namespace XYZ_shop.Application.Abstractions.Repositories
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        UserEntity? GetByLogin(string login);
        UserEntity? GetWithProfile(int id);
        bool IsLoginUnique(string login);
        void Register(UserEntity user);
        void UpdateLanguage(int userId, Language language);
        void UpdateProfile(UserEntity userData);
    }
}
