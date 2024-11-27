using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Payments.Models
{
    [Table("payment_transactions")]
    public class PaymentTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [ForeignKey("ServiceCategory")]
        public int ServiceCategoryId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(3)]
        public string Currency { get; set; }

        [Required]
        [MaxLength(64)]
        public string Status { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public ServiceCategory ServiceCategory { get; set; }
    }
}
