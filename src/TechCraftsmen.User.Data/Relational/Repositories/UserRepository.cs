using Microsoft.EntityFrameworkCore;
using TechCraftsmen.User.Core.Interfaces.Repositories;

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

        public IQueryable<Core.Entities.User> GetByFilter(HashSet<string> filters)
        {
            // TODO
            throw new NotImplementedException();
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
