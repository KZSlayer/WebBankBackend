using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Transaction.DTOs
{
    public class TransferByPhoneDTO
    {
        [Required]
        [StringLength(12)]
        public string PhoneNumber { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
