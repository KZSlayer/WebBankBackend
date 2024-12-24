using System.ComponentModel.DataAnnotations;

namespace Identity.DTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        [StringLength(255)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(255)]
        public string NewPassword { get; set; }
    }
}
