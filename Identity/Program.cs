using Identity.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<IdentityDbContext>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
