using Payments.Data;
using Payments.Filters;
using Payments.Messaging;
using Payments.Repositories;
using Payments.Services;
using Payments.Services.BaseServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<PaymentsDbContext>();
builder.Services.AddScoped<IPhoneNumberRangesRepository, PhoneNumberRangesRepository>();
builder.Services.AddScoped<IPhoneNumberRangesService, PhoneNumberRangesService>();
builder.Services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();
builder.Services.AddScoped<IPaymentTransactionService, PaymentTransactionService>();
builder.Services.AddScoped<IPaymentProviderRepository, PaymentProviderRepository>();
builder.Services.AddScoped<IPaymentProviderService, PaymentProviderService>();
builder.Services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
builder.Services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>();
builder.Services.AddSingleton<IHostedService, KafkaConsumerBackgroundService>();
builder.Services.AddScoped<CustomExceptionFilter>();
builder.Services.AddScoped<IPayPhoneService, PayPhoneService>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
