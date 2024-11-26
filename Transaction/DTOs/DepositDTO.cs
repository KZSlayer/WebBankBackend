using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Transaction.DTOs
{
    public class DepositDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
    }
}
