using Confluent.Kafka;
using Identity.DTOs;
using Identity.Models;
using Identity.Services;
using System.Text.Json;

namespace Identity.Messaging
{
    public class KafkaConsumerService : IKafkaConsumerService
    {
        private readonly IConsumer<string?, string> _consumer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IKafkaProducerService _producer;
        public KafkaConsumerService(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory, IKafkaProducerService producer)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = configuration["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            _consumer = new ConsumerBuilder<string?, string>(config).Build();
            _serviceScopeFactory = serviceScopeFactory;
            _producer = producer;
        }
        public async Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            await Task.Run(async () =>
            {
                var topics = new List<string>
                {
                    "phone-query",
                };
                _consumer.Subscribe(topics);
                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var result = _consumer.Consume(cancellationToken);
                        Console.WriteLine($"Получено сообщение из топика {result.Topic}: {result.Message.Value}");
                        switch (result.Topic)
                        {
                            case "phone-query":
                                var phoneCheck = JsonSerializer.Deserialize<PhoneCheckDTO>(result.Message.Value);
                                var existUser = await FindUserByPhone(phoneCheck.PhoneNumber, cancellationToken);
                                var phoneCheckResponse = new PhoneCheckResultDTO
                                {
                                    SenderId = phoneCheck.SenderId,
                                    RecipientId = existUser.Id == phoneCheck.SenderId ? null : existUser?.Id,
                                    CorrelationId = phoneCheck.CorrelationId
                                };
                                await _producer.SendPhoneCheckResponseAsync("phone-response", phoneCheckResponse);
                                break;
                            default:
                                Console.WriteLine($"Неизвестный топик: {result.Topic}");
                                break;
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Потребление сообщений остановлено.");
                }
            }, cancellationToken);
        }
        public async Task<User> FindUserByPhone(string phoneNumber, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                    var user = await userService.GetByPhoneAsync(phoneNumber);
                    _consumer.Commit();
                    return user;
                }
            }
            catch (Exception)
            {
                return new User();
            }
        }
    }
}
