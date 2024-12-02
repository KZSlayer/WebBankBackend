using Confluent.Kafka;
using System.Text.Json;
using Transaction.DTOs;
using Transaction.Models;
using Transaction.Services;

namespace Transaction.Messaging
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
                    "user-created",
                    "payment-transaction-check"
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
                            case "user-created":
                                if (int.TryParse(result.Message.Value, out int userId))
                                {
                                    await CreateAccountAsync(userId, cancellationToken);
                                }
                                else
                                {
                                    Console.WriteLine("Ошибка: Невозможно преобразовать сообщение в int");
                                }
                                break;
                            case "payment-transaction-check":
                                var paymentCheck = JsonSerializer.Deserialize<PaymentCheckDTO>(result.Message.Value);
                                Console.WriteLine("Deserialize сообщение");
                                var hasSufficientBalance = await CheckAccountBalanceAsync(paymentCheck, cancellationToken);
                                Console.WriteLine($"Проверили хватает ли баланса: {hasSufficientBalance}");
                                var paymentResult = new PaymentResultDTO
                                {
                                    PaymentTransactionId = paymentCheck.PaymentTransactionId,
                                    Success = hasSufficientBalance
                                };
                                Console.WriteLine($"PaymentTransactionId: {paymentResult.PaymentTransactionId}");
                                await _producer.SendMessageAsync("payment-transaction-result", paymentResult);
                                Console.WriteLine($"Отправили сообщение");
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

        public async Task CreateAccountAsync(int userID, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            try
            {
                var account = new Account
                {
                    UserId = userID,
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
        public async Task<bool> CheckAccountBalanceAsync(PaymentCheckDTO paymentCheckDTO, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            try
            {
                Console.WriteLine("Мы в CheckAccountBalanceAsync");
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
                    await accountService.DecreaseBalanceAsync(paymentCheckDTO.UserId, paymentCheckDTO.Amount);
                }
                _consumer.Commit();
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Ошибка CheckAccountBalanceAsync");
                return false;
            }
        }
    }
}
