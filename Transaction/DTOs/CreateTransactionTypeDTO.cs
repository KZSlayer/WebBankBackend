using System.ComponentModel.DataAnnotations;

namespace Transaction.DTOs
{
    public class CreateTransactionTypeDTO
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
    }
}
