using System.ComponentModel.DataAnnotations;

namespace Payments.DTOs
{
    public class ChangeServiceCategoryDescriptionDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string NewDescription { get; set; }
    }
}
