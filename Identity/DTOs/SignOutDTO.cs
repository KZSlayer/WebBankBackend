using System.ComponentModel.DataAnnotations;

namespace Identity.DTOs
{
    public class SignOutDTO
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        [StringLength(64)]
        public string DeviceID { get; set; }
    }
}
