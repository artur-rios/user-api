using TechCraftsmen.User.Core.Entities;

namespace TechCraftsmen.User.Core.Interfaces.Repositories
{
    public interface ICrudRepository<T> where T : BaseEntity
    {
        int Create(T entity);
        IQueryable<T> GetByFilter(IDictionary<string, object> filters);
        void Update(T entity);
        void Delete(T entity);
    }
}
