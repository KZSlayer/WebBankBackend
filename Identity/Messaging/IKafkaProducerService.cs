using Identity.DTOs;

namespace Identity.Messaging
{
    public interface IKafkaProducerService
    {
        Task SendMessageAsync(string topic, string userID);
        Task SendPhoneCheckResponseAsync(string topic, PhoneCheckResultDTO phoneCheckResultDTO);
    }
}
