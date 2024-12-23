using System.ComponentModel.DataAnnotations;

namespace Transaction.DTOs
{
    public class CheckPhoneDTO
    {
        public int SenderId { get; set; }

        [Required]
        [Phone]
        [StringLength(12)]
        public string PhoneNumber { get; set; }
        public string CorrelationId { get; set; }
    }
}
