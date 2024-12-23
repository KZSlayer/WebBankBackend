using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Transaction.Models
{
    [Table("transactions")]
    public class Transactions
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("FromAccount")]
        public long? FromAccountNumber { get; set; }

        [ForeignKey("ToAccount")]
        public long? ToAccountNumber { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [ForeignKey("TransactionType")]
        public int TransactionTypeId { get; set; }

        [Required]
        [MaxLength(64)]
        public string Status { get; set; }

        public DateTime Timestamp { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
