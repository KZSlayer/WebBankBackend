using Confluent.Kafka;
using Identity.DTOs;
using Identity.Models;
using Identity.Services;
using Identity.Services.Exceptions;
using System.Text.Json;

namespace Identity.Messaging
{
    public class KafkaConsumerService : IKafkaConsumerService
    {
        private readonly IConsumer<string?, string> _consumer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IKafkaProducerService _producer;
        private readonly ILogger<KafkaConsumerService> _logger;
        public KafkaConsumerService(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory, IKafkaProducerService producer, ILogger<KafkaConsumerService> logger)
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
            _logger = logger;
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
                        _logger.LogInformation($"Получено сообщение из топика {result.Topic} со следующими данными: {result.Message.Value}");
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
                    _logger.LogDebug("Потребление сообщений остановлено.");
                }
            }, cancellationToken);
        }
        public async Task<User?> FindUserByPhone(string phoneNumber, CancellationToken cancellationToken)
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
            catch (UserNotFoundException)
            {
                _logger.LogError("Пользователь не найден!");
                return new User();
            }
        }
    }
}
