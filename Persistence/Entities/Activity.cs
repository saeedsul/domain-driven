using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities
{
    public class Activity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string? FromAddress { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string? ToEmailAddress { get; set; }

        [Required]
        [MaxLength(100)]
        public string? FromName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BouncedDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? OpenedDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? SentDate { get; set; }

    }
}
