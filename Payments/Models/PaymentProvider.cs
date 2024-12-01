using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payments.Models
{
    [Table("payment_providers")]
    public class PaymentProvider
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [ForeignKey("ServiceCategory")]
        public int ServiceCategoryId { get; set; }

        public ServiceCategory ServiceCategory { get; set; }
    }
}
