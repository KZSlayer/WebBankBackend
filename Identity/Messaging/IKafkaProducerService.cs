namespace Identity.Messaging
{
    public interface IKafkaProducerService
    {
        Task SendMessageAsync(string topic, int userID);
    }
}
