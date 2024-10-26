using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Text;
using TechCraftsmen.User.Data.Configuration;
using TechCraftsmen.User.Domain.Interfaces;
using TechCraftsmen.User.Utils.Exceptions;

namespace TechCraftsmen.User.Data.Repositories
{
    public class UserRepository(IDbContextFactory<RelationalDbContext> dbContextFactory)
        : ICrudRepository<Domain.Entities.User>
    {
        private readonly RelationalDbContext _dbContext = dbContextFactory.CreateDbContext();

        public int Create(Domain.Entities.User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user.Id;
        }

        public IQueryable<Domain.Entities.User> GetByFilter(IDictionary<string, object> filters, bool track)
        {
            RelationalDbConfiguration.Tables.TryGetValue(nameof(Domain.Entities.User), out string? tableName);

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

        public void Update(Domain.Entities.User user)
        {
            _dbContext.Update(user);
            _dbContext.SaveChanges();
        }

        public void Delete(Domain.Entities.User user)
        {
            _dbContext.Remove(user);
            _dbContext.SaveChanges();
        }
    }
}
