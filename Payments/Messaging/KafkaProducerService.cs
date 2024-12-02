using Confluent.Kafka;
using Payments.DTOs;
using System.Text.Json;

namespace Payments.Messaging
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<Null, string> _producer;
        public KafkaProducerService(IConfiguration configuration)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task SendMessageAsync(string topic, ProducerKafkaDTO producerKafkaDTO)
        {
            try
            {
                var jsonMessage = JsonSerializer.Serialize(producerKafkaDTO);
                await _producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMessage });
            }
            catch (ProduceException<Null, string> ex)
            {
                Console.WriteLine($"Ошибка отправки сообщения: {ex.Error.Reason}");
            }
        }
    }
}
