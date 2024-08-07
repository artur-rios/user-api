using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Configuration.Authorization;
using TechCraftsmen.User.Configuration.DependencyInjection;
using TechCraftsmen.User.Configuration.Middleware;
using TechCraftsmen.User.Core.Configuration;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Mapping;

namespace TechCraftsmen.User.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            string appSettings = $"appsettings.api.{builder.Environment.EnvironmentName}.json";

            builder.Configuration.AddJsonFile(appSettings, optional: false, reloadOnChange: false);

            builder.Services.Configure<AuthenticationTokenConfiguration>(builder.Configuration.GetSection("AuthenticationTokenSettings"));

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
            builder.Services.AddRelationalContext(builder.Configuration.GetSection("ConnectionStrings"));
            builder.Services.AddModelValidators();
            builder.Services.AddRelationalRepositories();
            builder.Services.AddDomainRules();
            builder.Services.AddServices();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    ErrorDto[] errors = context.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .Select(e => new ErrorDto()
                        {
                            Parameter = e.Key,
                            Error = e.Value?.Errors.First().ErrorMessage
                        }).ToArray();

                    DataResultDto<ErrorDto[]> result = new DataResultDto<ErrorDto[]>(errors, "Invalid query string");

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
