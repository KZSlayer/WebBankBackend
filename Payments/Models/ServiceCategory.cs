using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Payments.Models
{
    [Table("service_categories")]
    public class ServiceCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public ICollection<PaymentProvider> PaymentProviders { get; set; }
        public ICollection<PaymentTransaction> PaymentTransactions { get; set; }
    }
}
