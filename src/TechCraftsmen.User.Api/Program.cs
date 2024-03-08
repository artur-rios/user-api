using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Core.Interfaces.Services;
using TechCraftsmen.User.Core.Services.Implementation;
using TechCraftsmen.User.Data.Relational.Repositories;

namespace TechCraftsmen.User.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<ICrudRepository<Core.Entities.User>, UserRepository>();

            builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
