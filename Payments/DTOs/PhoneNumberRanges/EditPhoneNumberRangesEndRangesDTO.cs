using System.ComponentModel.DataAnnotations;

namespace Payments.DTOs.PhoneNumberRanges
{
    public class EditPhoneNumberRangesEndRangesDTO
    {
        [Required]
        public string PaymentProviderName { get; set; }

        [Required]
        [StringLength(3)]
        public string Prefix { get; set; }

        [Required]
        public long EndRange { get; set; }
    }
}
