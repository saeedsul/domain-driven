using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
