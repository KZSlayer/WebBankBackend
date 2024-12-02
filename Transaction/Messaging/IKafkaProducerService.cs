using Transaction.DTOs;

namespace Transaction.Messaging
{
    public interface IKafkaProducerService
    {
        Task SendMessageAsync(string topic, PaymentResultDTO paymentResultDTO);
    }
}
