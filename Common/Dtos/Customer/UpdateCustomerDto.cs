using System.ComponentModel.DataAnnotations;

namespace Common.Dtos.Customer
{
    public class UpdateCustomerDto
    {

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name must be at most 50 characters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name must be at most 50 characters")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100, ErrorMessage = "Email must be at most 100 characters")]
        public string? Email { get; set; }
    }
}
