namespace Payments.DTOs
{
    public class ConsumerKafkaDTO
    {
        public int PaymentTransactionId { get; set; }
        public bool Success { get; set; }
    }
}
