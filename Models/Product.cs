using System.ComponentModel.DataAnnotations;

namespace PROG7311_POE_ST10150702.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public DateTime ProductionDate { get; set; }

        public int FarmerId { get; set; }
    }

}
