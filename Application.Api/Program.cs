using Application.Core.Interfaces;
using Application.Data.Extensions;
using Application.Services;
using Microsoft.EntityFrameworkCore.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationDbContext();


builder.Services.AddScoped<IFileProcessingService, FileProcessingService>();

var app = builder.Build();

await MigrationsHelper.ApplyMigrationsAsync(app.Services);

app.UseHttpsRedirection();

app.Run();
