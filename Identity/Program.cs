using Identity.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

string connectionString =
    $"Host={builder.Configuration["IDENTITY_DB_HOST"]};" +
    $"Port={builder.Configuration["IDENTITY_DB_PORT"]}" +
    $"Database={builder.Configuration["IDENTITY_DB_NAME"]}" +
    $"Username={builder.Configuration["IDENTITY_DB_USER"]}" +
    $"Password={builder.Configuration["IDENTITY_DB_PASSWORD"]}";

builder.Services.AddDbContext<IdentityDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
