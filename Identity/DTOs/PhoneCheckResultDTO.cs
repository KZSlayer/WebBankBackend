using System.ComponentModel.DataAnnotations;

namespace Identity.DTOs
{
    public class PhoneCheckResultDTO
    {
        public int SenderId { get; set; }

        public int? RecipientId { get; set; }
        public string CorrelationId { get; set; }
    }
}
