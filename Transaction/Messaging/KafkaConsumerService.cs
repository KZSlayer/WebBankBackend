using Confluent.Kafka;
using Transaction.Models;
using Transaction.Services;

namespace Transaction.Messaging
{
    public class KafkaConsumerService : IKafkaConsumerService
    {
        private readonly IConsumer<string, int> _consumer;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public KafkaConsumerService(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = configuration["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            _consumer = new ConsumerBuilder<string, int>(config).Build();
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _consumer.Subscribe("user-created");

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var result = _consumer.Consume(cancellationToken);
                        Console.WriteLine($"Получено сообщение: {result.Message.Value}");
                        Console.WriteLine($"Value: {result.Message.Value}");
                        CreateAccountAsync(result.Message.Value, cancellationToken).Wait(cancellationToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Потребление сообщений остановлено.");
                }
            }, cancellationToken);
        }

        public async Task CreateAccountAsync(int message, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            Console.WriteLine($"Обрабатываем сообщение: {message}");
            try
            {
                var account = new Account
                {
                    UserId = message,
                    Balance = 0,
                    Currency = "RUB",
                };
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
                    await accountService.CreateAccountAsync(account);
                }
                _consumer.Commit();
            }
            catch (Exception)
            {
                Console.WriteLine("Не получилось создать объект класса Account или ещё какая ошибка тут");
            }
        }
    }
}
