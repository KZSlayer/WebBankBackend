using Payments.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PaymentsDbContext>();
var app = builder.Build();
app.MapGet("/", () => "Hello World!");

app.Run();
