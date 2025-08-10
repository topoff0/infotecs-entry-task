using Application.Data.Data;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services)
        {
            var basePath = AppContext.BaseDirectory;
            var envPath = Path.Combine(basePath, "../../../..", ".env");
            Env.Load(envPath);

            // Get postgres data from .env
            var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
            var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
            var database = Environment.GetEnvironmentVariable("POSTGRES_DB");
            var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
            var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");

            string connectionString
                = $"Host={host};Username={user};Password={password};Database={database};Port={port}";

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();

            return services;
        }
    }
}