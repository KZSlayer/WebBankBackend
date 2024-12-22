
using Confluent.Kafka;

namespace Identity.Messaging
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<string, string> _producer;
        public KafkaProducerService(IConfiguration configuration)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task SendMessageAsync(string topic, string userID)
        {
            try
            {
                var result = await _producer.ProduceAsync(
                    topic, 
                    new Message<string, string> 
                    {
                        Key = $"userId-{userID}",
                        Value = userID 
                    });
                Console.WriteLine($"Отправлено сообщение в {result.TopicPartitionOffset}");
                Console.WriteLine($"{result.Value}");
            }
            catch (ProduceException<Null, int> ex)
            {
                Console.WriteLine($"Ошибка отправки сообщения: {ex.Error.Reason}");
            }
            
        }
    }
}
