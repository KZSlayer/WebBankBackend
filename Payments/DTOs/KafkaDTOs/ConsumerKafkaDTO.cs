namespace Payments.DTOs.KafkaDTOs
{
    public class ConsumerKafkaDTO
    {
        public int PaymentTransactionId { get; set; }
        public bool Success { get; set; }
    }
}
