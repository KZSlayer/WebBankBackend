using Confluent.Kafka;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using Transaction.DTOs;
using Transaction.Models;
using Transaction.Services.BaseServices;

namespace Transaction.Messaging
{
    public class KafkaConsumerService : IKafkaConsumerService
    {
        private readonly IConsumer<string?, string> _consumer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IKafkaProducerService _producer;
        private readonly PendingRequestsStore _pendingRequestsStore;
        public KafkaConsumerService(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory, IKafkaProducerService producer, PendingRequestsStore pendingRequestsStore)
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
            _pendingRequestsStore = pendingRequestsStore;
        }

        public async Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            await Task.Run(async () =>
            {
                var topics = new List<string>
                {
                    "user-created",
                    "payment-transaction-check",
                    "phone-response"
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
                            case "phone-response":
                                var phoneResponse = JsonSerializer.Deserialize<CheckPhoneResultDTO>(result.Message.Value);

                                if (_pendingRequestsStore.TryRemove(phoneResponse.CorrelationId, out var tcs))
                                {
                                    tcs.SetResult(phoneResponse?.RecipientId);
                                }
                                else
                                {
                                    Console.WriteLine($"Получен ответ с неизвестным CorrelationId: {phoneResponse.CorrelationId}");
                                    continue;
                                }
                                _consumer.Commit(result);
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
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
                    var account = new Account
                    {
                        UserId = userID,
                        AccountNumber = await accountService.GenerateAccountNumber(),
                        Balance = 0,
                        Currency = "RUB",
                    };
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
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
                    var account = await accountService.FindByUserIdAsync(paymentCheckDTO.UserId);
                    await accountService.DecreaseBalanceAsync(account, paymentCheckDTO.Amount); // Переделать?
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
