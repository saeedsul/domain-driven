using System.ComponentModel.DataAnnotations;

namespace Common.Dtos.Order
{
    public class UpdateOrderDto
    {
        [Required(ErrorMessage = "ProductId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than zero")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "CustomerId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be greater than zero")]
        public int CustomerId { get; set; }
        public string? Status { get; set; }
    }
}
