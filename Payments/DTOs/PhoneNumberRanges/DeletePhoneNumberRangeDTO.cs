using System.ComponentModel.DataAnnotations;

namespace Payments.DTOs.PhoneNumberRanges
{
    public class DeletePhoneNumberRangeDTO
    {
        [Required]
        public string PaymentProviderName { get; set; }

        [Required]
        [StringLength(3)]
        public string Prefix { get; set; }
    }
}
