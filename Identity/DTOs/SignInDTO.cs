using System.ComponentModel.DataAnnotations;

namespace Identity.DTOs
{
    public class SignInDTO
    {
        [Required]
        [Phone]
        [StringLength(12)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(64)]
        public string DeviceID { get; set; }
    }
}
