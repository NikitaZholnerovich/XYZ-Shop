using Microsoft.EntityFrameworkCore;
using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Domain.Entities;
using XYZ_shop.Domain.Enums;
using XYZ_shop.Infrastructure.Data;

namespace XYZ_shop.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(XyzDbContext context) : base(context) { }

        public UserEntity GetFirst()
        {
            return _dbSet.First();
        }

        public override void Add(UserEntity model)
        {
            throw new NotImplementedException("You can create new user only by using method Register");
        }

        public UserEntity? GetByLogin(string login)
        {
            return _dbSet.FirstOrDefault(x => x.Login == login);
        }

        public bool IsLoginUnique(string login)
        {
            return !_dbSet.Any(x => x.Login == login);
        }

        public void Register(UserEntity user)
        {
            user.Role = UserRole.User;
            user.Language = Language.English;

            _dbSet.Add(user);
            _context.SaveChanges();
        }

        public void UpdateLanguage(int userId, Language language)
        {
            var user = _dbSet.First(x => x.Id == userId);
            user.Language = language;
            _context.SaveChanges();
        }

        public void UpdateProfile(UserEntity userData)
        {
            var user = _dbSet
                .Include(u => u.UserProfile)
                .First(x => x.Id == userData.Id);

            if (user.UserProfile == null)
            {
                return;
            }

            user.UserProfile.FirstName = userData.UserProfile?.FirstName;
            user.UserProfile.LastName = userData.UserProfile?.LastName;
            user.UserProfile.Mobilephone = userData.UserProfile?.Mobilephone;
            _context.SaveChanges();
        }
    }
}
