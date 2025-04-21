using System.ComponentModel.DataAnnotations;

namespace Common.Dtos.Activity
{
    public class AddActivityDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must be at most 100 characters")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100, ErrorMessage = "Email must be at most 100 characters")]
        public string? FromAddress { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100, ErrorMessage = "Email must be at most 100 characters")]
        public string? ToEmailAddress { get; set; }

        [Required(ErrorMessage = "From name is required")]
        [StringLength(100, ErrorMessage = "From name must be at most 100 characters")]
        public string? FromName { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
