using Microsoft.EntityFrameworkCore;

using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Domain.Entities;
using XYZ_shop.Infrastructure.Data;
using XYZ_shop.Infrastructure.Repositories;

namespace XYZ_shop.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(XyzDbContext context) : base(context) { }

        public UserEntity GetFirst()
        {
            return _dbSet
                .First();
        }

        public override void Add(UserEntity model)
        {
            throw new NotImplementedException("You can create new user only by using method Registration");
        }

        //public UserEntity? GetByNameAndPassword(string login, string password)
        //{
        //    var hash = GetHashOfPassword(password);
        //    return _dbSet
        //        .FirstOrDefault(x => x.Name == login && x.Password == hash);
        //}

        //public bool IsNameUniq(string login)
        //{
        //    return !_dbSet.Any(x => x.Name == login);
        //}

        //public void Registration(UserEntity user)
        //{
        //    var hash = GetHashOfPassword(user.Password);
        //    user.Password = hash;
        //    user.Role = Enums.UserRole.User;
        //    user.Language = Enums.Language.English;

        //    _dbSet.Add(user);
        //    _context.SaveChanges();
        //}

        private string GetHashOfPassword(string password)
        {
            // "Password"
            // "Possword"
            // "Posswor"

            password = password.Replace("a", "o");
            return password.Substring(0, password.Length - 1);
        }

        //public void UpdateLanguage(int userId, Language language)
        //{
        //    var user = _dbSet.First(x => x.Id == userId);
        //    user.Language = language;
        //    _context.SaveChanges();
        //}

        //public void UpdateProfile(UserData userData)
        //{
        //    var user = _dbSet.First(x => x.Id == userData.Id);
        //    user.FirstName = userData.FirstName;
        //    user.LastName = userData.LastName;
        //    user.Mobilephone = userData.Mobilephone;
        //    _context.SaveChanges();
        //}
    }
}
