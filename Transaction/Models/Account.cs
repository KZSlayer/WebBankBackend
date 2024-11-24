using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Transaction.Models
{
    [Table("accounts")]
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        [Required]
        [MaxLength(3)]
        public string Currency { get; set; }

        public ICollection<Transactions> SentTransactions { get; set; }
        public ICollection<Transactions> ReceivedTransactions { get; set; }
    }
}
