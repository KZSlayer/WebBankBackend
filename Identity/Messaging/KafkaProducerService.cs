using Confluent.Kafka;
using Identity.DTOs;
using System.Text.Json;

namespace Identity.Messaging
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<string, string> _producer;
        private readonly ILogger<KafkaProducerService> _logger;
        public KafkaProducerService(IConfiguration configuration, ILogger<KafkaProducerService> logger)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };
            _producer = new ProducerBuilder<string, string>(config).Build();
            _logger = logger;
        }

        public async Task SendMessageAsync(string topic, string userID)
        {
            try
            {
                _logger.LogInformation($"Отправляем в топик: {topic} - следующие данные: {userID}");
                var result = await _producer.ProduceAsync(
                    topic,
                    new Message<string, string> 
                    {
                        Key = $"userId-{userID}",
                        Value = userID 
                    });
            }
            catch (ProduceException<Null, int> ex)
            {
                _logger.LogError($"Ошибка отправки сообщения: {ex.Error.Reason}");
            }
        }
        public async Task SendPhoneCheckResponseAsync(string topic, PhoneCheckResultDTO phoneCheckResultDTO)
        {
            try
            {
                var jsonMessage = JsonSerializer.Serialize(phoneCheckResultDTO);
                _logger.LogInformation($"Отправляем в топик: {topic} - следующие данные: {jsonMessage}");
                var result = await _producer.ProduceAsync(
                    topic,
                    new Message<string, string>
                    {
                        Key = phoneCheckResultDTO.SenderId.ToString(),
                        Value = jsonMessage
                    });
            }
            catch (ProduceException<Null, int> ex)
            {
                _logger.LogError($"Ошибка отправки сообщения: {ex.Error.Reason}");
            }
        }
    }
}
