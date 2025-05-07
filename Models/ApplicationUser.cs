using Microsoft.AspNetCore.Identity;

namespace PROG7311_POE_ST10150702.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}