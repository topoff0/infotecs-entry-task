using Application.Data.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationDbContext();

var app = builder.Build();

await MigrationsHelper.ApplyMigrationsAsync(app.Services);

app.UseHttpsRedirection();

app.Run();
