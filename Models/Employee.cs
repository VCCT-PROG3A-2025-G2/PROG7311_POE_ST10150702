// Models/Employee.cs
using PROG7311_POE_ST10150702.Models;

public class Employee
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Link to Identity User (which contains email/password)
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}