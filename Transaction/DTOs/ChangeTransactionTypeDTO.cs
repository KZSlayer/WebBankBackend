using System.ComponentModel.DataAnnotations;

namespace Transaction.DTOs
{
    public class ChangeTransactionTypeDTO
    {
        [Required]
        [MaxLength(64)]
        public string ExistName { get; set; }
        [Required]
        [MaxLength(64)]
        public string NewName { get; set; }

        [Required]
        [MaxLength(255)]
        public string NewDescription { get; set; }
    }
}
