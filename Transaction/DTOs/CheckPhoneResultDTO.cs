namespace Transaction.DTOs
{
    public class CheckPhoneResultDTO
    {
        public int SenderId { get; set; }

        public int? RecipientId { get; set; }
        public string CorrelationId { get; set; }
    }
}