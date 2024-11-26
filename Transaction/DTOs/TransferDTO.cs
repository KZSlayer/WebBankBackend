using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Transaction.DTOs
{
    public class TransferDTO
    {
        [Required]
        public int FromAccountUserId { get; set; }

        [Required]
        public int ToAccountUserId { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
