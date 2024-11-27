using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Payments.Models
{
    [Table("phone_number_ranges")]
    public class PhoneNumberRange
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("PaymentProvider")]
        public int PaymentProviderId { get; set; }

        [Required]
        [StringLength(3)]
        public string Prefix { get; set; }

        [Required]
        public long StartRange { get; set; }

        [Required]
        public long EndRange { get; set; }

        public virtual PaymentProvider PaymentProvider { get; set; }
    }
}
