using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Payments.DTOs
{
    public class PayPhoneDTO
    {
        [Required]
        [StringLength(12)]
        public string PhoneNumber { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
    }
}
