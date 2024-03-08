using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Core.Interfaces.Services;
using TechCraftsmen.User.Core.Services.Implementation;
using TechCraftsmen.User.Data.Relational;
using TechCraftsmen.User.Data.Relational.Repositories;
using TechCraftsmen.User.Services.Mapping;

namespace TechCraftsmen.User.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var appSettings = $"appsettings.api.{builder.Environment.EnvironmentName}.json";

            builder.Configuration.AddJsonFile(appSettings, optional: false, reloadOnChange: false);

            var configuration = builder.Configuration;

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            builder.Services.AddRelationalDBContext(configuration.GetSection("ConnectionStrings"));

            builder.Services.AddScoped<ICrudRepository<Core.Entities.User>, UserRepository>();

            builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Local"))
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
