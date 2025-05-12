using System.Collections.Generic;
using PROG7311_POE_ST10150702.Models;

namespace PROG7311_POE_ST10150702.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int FarmerCount { get; set; }
        public int EmployeeCount { get; set; }
        public int ProductCount { get; set; }

        public List<Farmer> Farmers { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Product> Products { get; set; }

        public Employee NewEmployee { get; set; }
    }
}