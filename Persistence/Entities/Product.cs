using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string? SKU { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
