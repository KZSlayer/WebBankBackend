using System.ComponentModel.DataAnnotations;

namespace Identity.DTOs
{
    public class UserPhoneDTO
    {
        [Required]
        [Phone]
        [StringLength(12)]
        public string PhoneNumber { get; set; }
    }
}
