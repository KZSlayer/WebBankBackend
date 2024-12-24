using System.ComponentModel.DataAnnotations;

namespace Payments.DTOs.PhoneNumberRanges
{
    public class EditPhoneNumberRangesPrefixDTO
    {
        [Required]
        public string PaymentProviderName { get; set; }

        [Required]
        [StringLength(3)]
        public string CurrentPrefix { get; set; }

        [Required]
        [StringLength(3)]
        public string NewPrefix { get; set; }
    }
}
