using TechCraftsmen.User.Configuration.Authorization;
using TechCraftsmen.User.Configuration.DependencyInjection;
using TechCraftsmen.User.Core.Configuration;
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

            builder.Services.Configure<AuthenticationTokenConfiguration>(builder.Configuration.GetSection("AuthenticationTokenSettings"));

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
            builder.Services.AddRelationalContext(builder.Configuration.GetSection("ConnectionStrings"));
            builder.Services.AddModelValidators();
            builder.Services.AddRelationalRepositories();
            builder.Services.AddDomainRules();
            builder.Services.AddServices();
            builder.Services.AddFilterValidators();

            var app = builder.Build();

            if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Local"))
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseMiddleware<JwtMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
