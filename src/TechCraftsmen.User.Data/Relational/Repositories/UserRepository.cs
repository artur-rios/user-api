using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Text;
using TechCraftsmen.User.Core.Exceptions;
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
            RelationalDBConfiguration.Tables.TryGetValue(nameof(Core.Entities.User), out string? tableName);

            if (tableName is null)
            {
                throw new NotFoundException("Entity not mapped to relational table!");
            }

            var query = new StringBuilder($"SELECT * FROM sc_user_api.{tableName}");

            if (filters != null && filters.Count > 0)
            {
                query.Append(" WHERE ");

                for (int index = 0; index < filters.Count; index++)
                {
                    var filter = filters.ElementAt(index);

                    if (filter.Value is string || filter.Value is DateTime)
                    {
                        query.Append($"{filter.Key} = '{filter.Value}'");
                    }
                    else
                    {
                        query.Append($"{filter.Key} = {filter.Value}");
                    }

                    if (index < filters.Count - 1)
                    {
                        query.Append(" AND ");
                    }
                }
            }

            return _dbContext.Users.FromSql(FormattableStringFactory.Create(query.ToString()));
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
