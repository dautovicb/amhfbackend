namespace MentalHealth.Models.DTOs
{
    public class CategoryWithPostCountDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int PostCount { get; set; }
    }
}
