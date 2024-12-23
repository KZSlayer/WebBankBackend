using Confluent.Kafka;
using System.Text.Json;
using Transaction.DTOs;

namespace Transaction.Messaging
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<string?, string> _producer;
        public KafkaProducerService(IConfiguration configuration)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };
            _producer = new ProducerBuilder<string?, string>(config).Build();
        }

        public async Task SendMessageAsync(string topic, PaymentResultDTO paymentResultDTO)
        {
            try
            {
                var jsonMessage = JsonSerializer.Serialize(paymentResultDTO);
                await _producer.ProduceAsync(topic, new Message<string?, string> { Value = jsonMessage });
            }
            catch (ProduceException<string?, string> ex)
            {
                Console.WriteLine($"Ошибка отправки сообщения: {ex.Error.Reason}");
            }
        }
        public async Task SendCheckPhoneAsync(string topic, CheckPhoneDTO checkPhoneDTO)
        {
            try
            {
                var jsonMessage = JsonSerializer.Serialize(checkPhoneDTO);
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
                Console.WriteLine($"Ошибка отправки сообщения: {ex.Error.Reason}");
            }
        }
    }
}
