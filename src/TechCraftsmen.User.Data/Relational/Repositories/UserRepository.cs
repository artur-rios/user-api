using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Text;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Data.Relational.Configuration;

namespace TechCraftsmen.User.Data.Relational.Repositories
{
    public class UserRepository(IDbContextFactory<RelationalDbContext> dbContextFactory)
        : ICrudRepository<Core.Entities.User>
    {
        private readonly RelationalDbContext _dbContext = dbContextFactory.CreateDbContext();

        public int Create(Core.Entities.User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user.Id;
        }

        public IQueryable<Core.Entities.User> GetByFilter(IDictionary<string, object> filters, bool track)
        {
            RelationalDbConfiguration.Tables.TryGetValue(nameof(Core.Entities.User), out string? tableName);

            if (tableName is null)
            {
                throw new CustomException(["Entity not mapped to relational table"]);
            }

            StringBuilder query = new($"SELECT * FROM sc_user_api.{tableName}");

            if (filters.Count == 0)
            {
                return _dbContext.Users.FromSql(FormattableStringFactory.Create(query.ToString()));
            }

            query.Append(" WHERE ");

            for (int index = 0; index < filters.Count; index++)
            {
                KeyValuePair<string, object> filter = filters.ElementAt(index);

                if (filter.Value is string or DateTime)
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

            return track
                ? _dbContext.Users.FromSql(FormattableStringFactory.Create(query.ToString()))
                : _dbContext.Users.FromSql(FormattableStringFactory.Create(query.ToString())).AsNoTracking();
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