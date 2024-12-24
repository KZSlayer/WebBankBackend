using System.ComponentModel.DataAnnotations;

namespace Payments.DTOs.PaymentProviderDTOs
{
    public class ChangePaymentProviderServiceCategoryIdDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string ServiceCategoryName { get; set; }
    }
}
