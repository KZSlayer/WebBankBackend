﻿namespace Payments.Messaging
{
    public interface IKafkaConsumerService
    {
        Task StartConsumingAsync(CancellationToken cancellationToken);
    }
}
