
using Microsoft.EntityFrameworkCore;
using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Domain.Entities;
using XYZ_shop.Infrastructure.Data;

namespace XYZ_shop.Infrastructure.Repositories
{
    public abstract class BaseRepository<DataModel>
        : IBaseRepository<DataModel> where DataModel : BaseEntity
    {
        protected XyzDbContext _context;
        protected DbSet<DataModel> _dbSet;

        public BaseRepository(XyzDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<DataModel>();
        }

        public virtual void Add(DataModel model)
        {
            _dbSet.Add(model);
            _context.SaveChanges();
        }

        public virtual void Remove(DataModel model)
        {
            _dbSet.Remove(model);
            _context.SaveChanges();
        }

        public virtual DataModel? Get(int id)
        {
            return _dbSet.FirstOrDefault(x => x.Id == id);
        }

        public virtual bool Any()
        {
            return _dbSet.Any();
        }

        public virtual List<DataModel> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual void Update(DataModel model)
        {
            _dbSet.Update(model);
            _context.SaveChanges();
        }

        public virtual void Delete(int id)
        {
            var user = _dbSet.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                _dbSet.Remove(user);
                _context.SaveChanges();
            }
        }

        public virtual void Delete(List<int> ids)
        {
            var models = _dbSet.Where(x => ids.Contains(x.Id));
            if (models.Any())
            {
                _dbSet.RemoveRange(models);
                _context.SaveChanges();
            }
        }

        public virtual List<DataModel> GetByIds(List<int> ids)
        {
            var foodItems = _dbSet.Where(x => ids.Contains(x.Id)).ToList();
            return foodItems;
        }

        public void Update(List<DataModel> models)
        {
            _dbSet.UpdateRange(models);
            _context.SaveChanges();
        }

        public void DeleteRange(List<DataModel> models)
        {
            _dbSet.RemoveRange(models);
            _context.SaveChanges();
        }
    }
}
