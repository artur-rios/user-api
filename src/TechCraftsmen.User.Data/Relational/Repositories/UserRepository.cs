using Microsoft.EntityFrameworkCore;
using System.Text;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Data.Relational.Configuration;

namespace TechCraftsmen.User.Data.Relational.Repositories
{
    public class UserRepository : ICrudRepository<Core.Entities.User>
    {
        private readonly RelationalDBContext _dbContext;

        public UserRepository(IDbContextFactory<RelationalDBContext> dbContextFactory)
        {
            _dbContext = dbContextFactory.CreateDbContext();
        }

        public int Create(Core.Entities.User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user.Id;
        }

        public Core.Entities.User? GetById(int id)
        {
            return _dbContext.Find<Core.Entities.User>(id);
        }

        public IQueryable<Core.Entities.User> GetByFilter(IDictionary<string, object> filters)
        {
            var query = new StringBuilder("SELECT * FROM dbo.Users");

            if (filters != null && filters.Count > 0)
            {
                query.Append(" WHERE ");

                for (int index = 0; index < filters.Count; index++)
                {
                    var filter = filters.ElementAt(index);

                    query.Append($"{filter.Key} = {filter.Value}");

                    if (index < filters.Count - 1)
                    {
                        query.Append(" AND ");
                    }
                }
            }

            return _dbContext.Users.FromSqlRaw(query.ToString());
        }

        public void Update(Core.Entities.User user)
        {
            _dbContext.Update(user);
            _dbContext.SaveChanges();
        }

        public void Delete(Core.Entities.User user)
        {
            _dbContext.Remove(user);
            _dbContext.SaveChanges();
        }
    }
}
