namespace PROG7311_POE_ST10150702.Models
{
    public class Farmer
    {
        public int FarmerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
