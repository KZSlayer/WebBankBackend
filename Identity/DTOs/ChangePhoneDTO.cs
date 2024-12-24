using System.ComponentModel.DataAnnotations;

namespace Identity.DTOs
{
    public class ChangePhoneDTO
    {
        [Required]
        [Phone]
        [StringLength(12)]
        public string PhoneNumber { get; set; }
    }
}
