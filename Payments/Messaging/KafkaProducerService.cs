using Confluent.Kafka;
using Payments.DTOs.KafkaDTOs;
using System.Text.Json;

namespace Payments.Messaging
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<KafkaProducerService> _logger;
        public KafkaProducerService(IConfiguration configuration, ILogger<KafkaProducerService> logger)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };
            _producer = new ProducerBuilder<Null, string>(config).Build();
            _logger = logger;
        }

        public async Task SendMessageAsync(string topic, ProducerKafkaDTO producerKafkaDTO)
        {
            try
            {
                var jsonMessage = JsonSerializer.Serialize(producerKafkaDTO);
                _logger.LogInformation($"Отправляем сообщение в топик: {topic} - содержащее следующие данные: {jsonMessage}");
                await _producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMessage });
            }
            catch (ProduceException<Null, string> ex)
            {
                _logger.LogError($"Ошибка отправки сообщения: {ex.Error.Reason}");
            }
        }
    }
}
