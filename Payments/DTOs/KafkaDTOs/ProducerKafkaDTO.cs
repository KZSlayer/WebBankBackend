namespace Payments.DTOs.KafkaDTOs
{
    public class ProducerKafkaDTO
    {
        public int PaymentTransactionId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
