
using Confluent.Kafka;

namespace Identity.Messaging
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<string, int> _producer;
        public KafkaProducerService(IProducer<string, int> producer)
        {
            _producer = producer;
        }

        public async Task SendMessageAsync(string topic, int userID)
        {
            try
            {
                var result = await _producer.ProduceAsync(
                    topic, 
                    new Message<string, int> 
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
