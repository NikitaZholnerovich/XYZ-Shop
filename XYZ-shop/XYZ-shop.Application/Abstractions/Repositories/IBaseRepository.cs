
using XYZ_shop.Domain.Entities;

namespace XYZ_shop.Application.Abstractions.Repositories
{
    public interface IBaseRepository<DataModel>
         where DataModel : BaseEntity
    {
        public void Add(DataModel model);
        public List<DataModel> GetAll();
        public void Remove(DataModel model);
        public DataModel? Get(int id);
        public void Update(DataModel model);
        public void Update(List<DataModel> models);
        public void Delete(int id);
        void DeleteRange(List<DataModel> models);
        public bool Any();
        void Delete(List<int> ids);
        List<DataModel> GetByIds(List<int> ids);
    }
}
