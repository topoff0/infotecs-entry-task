using Application.Api.Extensions;
using Application.Core.Interfaces.Calculations;
using Application.Core.Interfaces.Data;
using Application.Core.Interfaces.Parsers;
using Application.Core.Interfaces.Services;
using Application.Core.Interfaces.Validations;
using Application.Data.Extensions;
using Application.Data.Repositories;
using Application.Services.Calculations;
using Application.Services.DataProcessing;
using Application.Services.FileProcessing;
using Application.Services.Parsers;
using Application.Services.Validations;

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

builder.Services.AddScoped<IMetricRepository, MetricRepository>();
builder.Services.AddScoped<IResultRepository, ResultRepository>();
builder.Services.AddScoped<IFileProcessingService, FileProcessingService>();
builder.Services.AddScoped<IResultService, ResultService>();
builder.Services.AddScoped<IMetricService, MetricService>();

builder.Services.AddSingleton<IMetricCalculator, MetricCalculator>();
builder.Services.AddSingleton<ICsvMetricsParser, CsvMetricsParser>();
builder.Services.AddSingleton<IMetricValidator, MetricValidator>();
builder.Services.AddSingleton<IResultValidator, ResultValidator>();

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
