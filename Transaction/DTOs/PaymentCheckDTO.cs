namespace Transaction.DTOs
{
    public class PaymentCheckDTO
    {
        public int PaymentTransactionId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
