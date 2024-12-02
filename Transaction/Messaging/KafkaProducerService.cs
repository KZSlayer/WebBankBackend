using Confluent.Kafka;
using System.Text.Json;
using Transaction.DTOs;

namespace Transaction.Messaging
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

        public async Task SendMessageAsync(string topic, PaymentResultDTO paymentResultDTO)
        {
            try
            {
                var jsonMessage = JsonSerializer.Serialize(paymentResultDTO);
                Console.WriteLine("Serialize сообщение");
                await _producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMessage });
            }
            catch (ProduceException<Null, string> ex)
            {
                Console.WriteLine($"Ошибка отправки сообщения: {ex.Error.Reason}");
            }
        }
    }
}
