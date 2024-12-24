
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Payments.DTOs.KafkaDTOs;
using Payments.Services.BaseServices;
using System.Text.Json;

namespace Payments.Messaging
{
    public class KafkaConsumerService : IKafkaConsumerService
    {
        private readonly IConsumer<Null, string> consumer;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<KafkaConsumerService> _logger;
        public KafkaConsumerService(IConfiguration configuration, IServiceScopeFactory _serviceScopeFactory, ILogger<KafkaConsumerService> logger)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = configuration["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };
            consumer = new ConsumerBuilder<Null, string>(config).Build();
            serviceScopeFactory = _serviceScopeFactory;
            _logger = logger;
        }

        public async Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            await Task.Run(async () =>
            {
                consumer.Subscribe("payment-transaction-result");
                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var result = consumer.Consume();
                        var message = JsonSerializer.Deserialize<ConsumerKafkaDTO>(result.Message.Value);
                        _logger.LogInformation($"Пришло сообщение из топика: {result.Topic} - содержащее следующие данные: {result.Message.Value}");
                        using (var scope = serviceScopeFactory.CreateScope())
                        {
                            var paymentTS = scope.ServiceProvider.GetRequiredService<IPaymentTransactionService>();
                            if (message.Success)
                            {
                                await paymentTS.UpdatePaymentTransactionStatusAsync(message.PaymentTransactionId, "Успешно");
                            }
                            else
                            {
                                await paymentTS.UpdatePaymentTransactionStatusAsync(message.PaymentTransactionId, "Отклонено");
                            }
                        }
                        consumer.Commit();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }, cancellationToken);
        }
    }
}
