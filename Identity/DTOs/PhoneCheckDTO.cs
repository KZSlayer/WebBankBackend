using System.ComponentModel.DataAnnotations;

namespace Identity.DTOs
{
    public class PhoneCheckDTO
    {
        public int SenderId { get; set; }

        [Required]
        [Phone]
        [StringLength(12)]
        public string PhoneNumber { get; set; }
        public string CorrelationId { get; set; }
    }
}
