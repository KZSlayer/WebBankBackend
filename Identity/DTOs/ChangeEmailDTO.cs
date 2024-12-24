using System.ComponentModel.DataAnnotations;

namespace Identity.DTOs
{
    public class ChangeEmailDTO
    {
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }
    }
}
