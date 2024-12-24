using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Payments.DTOs.PaymentProviderDTOs
{
    public class PaymentProviderDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string ServiceCategoryName { get; set; }
    }
}
