using Transaction.DTOs;

namespace Transaction.Messaging
{
    public interface IKafkaProducerService
    {
        Task SendMessageAsync(string topic, PaymentResultDTO paymentResultDTO);
        Task SendCheckPhoneAsync(string topic, CheckPhoneDTO checkPhoneDTO);
    }
}
