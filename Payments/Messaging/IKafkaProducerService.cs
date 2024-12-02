using Payments.DTOs;

namespace Payments.Messaging
{
    public interface IKafkaProducerService
    {
        Task SendMessageAsync(string topic, ProducerKafkaDTO producerKafkaDTO);
    }
}
