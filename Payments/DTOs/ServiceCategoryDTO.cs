using System.ComponentModel.DataAnnotations;

namespace Payments.DTOs
{
    public class ServiceCategoryDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
    }
}
