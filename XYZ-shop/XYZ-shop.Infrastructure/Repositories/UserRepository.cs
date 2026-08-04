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

        public UserEntity? GetWithProfile(int id)
        {
            return _dbSet
                .Include(u => u.UserProfile)
                .FirstOrDefault(x => x.Id == id);
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
                user.UserProfile = new UserProfileEntity
                {
                    Email = userData.UserProfile?.Email ?? string.Empty,
                    FirstName = userData.UserProfile?.FirstName,
                    LastName = userData.UserProfile?.LastName,
                    Mobilephone = userData.UserProfile?.Mobilephone,
                    BirthDate = userData.UserProfile?.BirthDate,
                    CreatedAt = DateTime.UtcNow,
                };
            }
            else
            {
                user.UserProfile.Email = userData.UserProfile?.Email ?? user.UserProfile.Email;
                user.UserProfile.FirstName = userData.UserProfile?.FirstName;
                user.UserProfile.LastName = userData.UserProfile?.LastName;
                user.UserProfile.Mobilephone = userData.UserProfile?.Mobilephone;
                user.UserProfile.BirthDate = userData.UserProfile?.BirthDate;
                user.UserProfile.ModifiedAt = DateTime.UtcNow;
            }

            if (userData.AvatarUrl != null)
            {
                user.AvatarUrl = userData.AvatarUrl;
            }

            _context.SaveChanges();
        }
    }
}
