using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Identity.DTOs
{
    public class SignUpDTO
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(12)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }
    }
}
