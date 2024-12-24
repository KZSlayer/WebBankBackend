using System.ComponentModel.DataAnnotations;

namespace Payments.DTOs
{
    public class ChangeServiceCategoryNameDTO
    {
        [Required]
        [StringLength(100)]
        public string CurrentName { get; set; }

        [Required]
        [StringLength(100)]
        public string NewName { get; set; }
    }
}
