using System.ComponentModel.DataAnnotations;

namespace Common.Dtos.Product
{
    public class AddProductDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be at most 50 characters")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, ErrorMessage = "Description must be at most 100 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "SKU is required")]
        [StringLength(50, ErrorMessage = "SKU must be at most 50 characters")]
        public string? SKU { get; set; }
    }
}
