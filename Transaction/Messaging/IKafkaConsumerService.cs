namespace Transaction.Messaging
{
    public interface IKafkaConsumerService
    {
        Task StartConsumingAsync(CancellationToken cancellationToken);
        Task CreateAccountAsync(int message, CancellationToken cancellationToken);
    }
}
