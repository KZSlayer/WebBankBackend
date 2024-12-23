namespace Identity.Messaging
{
    public interface IKafkaConsumerService
    {
        Task StartConsumingAsync(CancellationToken cancellationToken);
    }
}
