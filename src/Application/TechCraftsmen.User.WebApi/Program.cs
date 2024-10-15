using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.WebApi.Authorization;
using TechCraftsmen.User.WebApi.DependencyInjection;
using TechCraftsmen.User.WebApi.Middleware;
using TechCraftsmen.User.Core.Configuration;
using TechCraftsmen.User.WebApi.ValueObjects;

namespace TechCraftsmen.User.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            string appSettings = $"appsettings.api.{builder.Environment.EnvironmentName}.json";

            builder.Configuration.AddJsonFile(appSettings, optional: false, reloadOnChange: false);

            builder.Services.Configure<AuthenticationTokenConfiguration>(
                builder.Configuration.GetSection("AuthenticationTokenSettings"));

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddRelationalContext(builder.Configuration.GetSection("ConnectionStrings"));
            builder.Services.AddModelValidators();
            builder.Services.AddRelationalRepositories();
            builder.Services.AddDomainRules();
            builder.Services.AddServices();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    string[] errors = context.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .Select(e => $"Parameter: {e.Key} | Error: {e.Value?.Errors.First().ErrorMessage}").ToArray();

                    WebApiOutput<string> result = new(string.Empty, errors);

                    return new BadRequestObjectResult(result);
                };
            });

            WebApplication app = builder.Build();

            if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Local"))
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<JwtMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}