using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Transaction.DTOs
{
    public class DepositDTO
    {

        [Required]
        public decimal Amount { get; set; }
    }
}
