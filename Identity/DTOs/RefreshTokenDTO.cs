using System.ComponentModel.DataAnnotations;

namespace Identity.DTOs
{
    public class RefreshTokenDTO
    {
        [Required]
        [StringLength(500)]
        public string RefreshToken { get; set; }

        [Required]
        [StringLength(64)]
        public string DeviceID { get; set; }

    }
}
