
using Confluent.Kafka;

namespace Identity.Messaging
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<string, string> _producer;
        public KafkaProducerService(IProducer<string, string> producer)
        {
            _producer = producer;
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
