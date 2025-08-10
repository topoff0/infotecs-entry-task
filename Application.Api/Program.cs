using Application.Api.Extensions;
using Application.Core.Interfaces;
using Application.Data.Extensions;
using Application.Services.DataProcessing;
using Application.Services.FileProcessing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplicationDbContext();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Application API",
        Version = "v1",
    });
});

builder.Services.AddScoped<IFileProcessingService, FileProcessingService>();
builder.Services.AddScoped<IResultsProcessingService, ResultsProcessingService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Application API v1");
        options.RoutePrefix = string.Empty;
    });
}

await MigrationsHelper.ApplyMigrationsAsync(app.Services);

app.UseExceptionHandling();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
