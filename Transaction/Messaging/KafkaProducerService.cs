using Confluent.Kafka;
using System.Text.Json;
using Transaction.DTOs;

namespace Transaction.Messaging
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<string?, string> _producer;
        private readonly ILogger<KafkaProducerService> _logger;
        public KafkaProducerService(IConfiguration configuration, ILogger<KafkaProducerService> logger)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };
            _producer = new ProducerBuilder<string?, string>(config).Build();
            _logger = logger;
        }

        public async Task SendMessageAsync(string topic, PaymentResultDTO paymentResultDTO)
        {
            try
            {
                var jsonMessage = JsonSerializer.Serialize(paymentResultDTO);
                _logger.LogInformation($"Сообщение отправляется в топик: {topic} - содержит следующие данные: {jsonMessage}");
                await _producer.ProduceAsync(topic, new Message<string?, string> { Value = jsonMessage });
            }
            catch (ProduceException<string?, string> ex)
            {
                _logger.LogError($"Ошибка отправки сообщения: {ex.Error.Reason}");
            }
        }
        public async Task SendCheckPhoneAsync(string topic, CheckPhoneDTO checkPhoneDTO)
        {
            try
            {
                var jsonMessage = JsonSerializer.Serialize(checkPhoneDTO);
                _logger.LogInformation($"Сообщение отправляется в топик: {topic} - содержит следующие данные: {jsonMessage}");
                await _producer.ProduceAsync(
                    topic,
                    new Message<string?, string>
                    {
                        Key = checkPhoneDTO.SenderId.ToString(),
                        Value = jsonMessage
                    });
            }
            catch (ProduceException<string?, string> ex)
            {
                _logger.LogError($"Ошибка отправки сообщения: {ex.Error.Reason}");
            }
        }
    }
}
