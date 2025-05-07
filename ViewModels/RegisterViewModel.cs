using System.ComponentModel.DataAnnotations;

namespace PROG7311_POE_ST10150702.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        // Farmer-specific fields (now always required)
        [Required(ErrorMessage = "Region is required")]
        [Display(Name = "Region")]
        public string Region { get; set; }

        [Required(ErrorMessage = "You must accept the POPPIA Act")]
        [Display(Name = "Accept POPPIA Act")]
        public bool AcceptedPOPPIA { get; set; }
    }
}