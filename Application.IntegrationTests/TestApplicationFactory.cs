using Application.Data.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace Application.IntegrationTests
{
    public class TestApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgresContainer
            = new PostgreSqlBuilder()
                .WithDatabase("testdb")
                .WithUsername("test_user")
                .WithPassword("test_password")
                .Build();

        private string connectionString => _postgresContainer.GetConnectionString();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<ApplicationDbContext>>();

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                });
            });
        }

        public async Task InitializeAsync()
        {
            await _postgresContainer.StartAsync();

            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await db.Database.MigrateAsync();

            db.Metrics.RemoveRange(db.Metrics);
            db.Results.RemoveRange(db.Results);
            await db.SaveChangesAsync();


        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _postgresContainer.DisposeAsync();
        }
    }
}