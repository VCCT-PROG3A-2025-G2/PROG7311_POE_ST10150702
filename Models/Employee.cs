namespace PROG7311_POE_ST10150702.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }

        // This will link to the Identity User
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}