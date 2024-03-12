using TechCraftsmen.User.Api.Configuration;
using TechCraftsmen.User.Core.Mapping;

namespace TechCraftsmen.User.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var appSettings = $"appsettings.api.{builder.Environment.EnvironmentName}.json";

            builder.Configuration.AddJsonFile(appSettings, optional: false, reloadOnChange: false);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
            builder.Services.AddRelationalContext(builder.Configuration.GetSection("ConnectionStrings"));
            builder.Services.AddModelValidators();
            builder.Services.AddRelationalRepositories();
            builder.Services.AddDomainRules();
            builder.Services.AddServices();

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
