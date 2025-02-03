namespace MentalHealth.Models.DTOs
{
    public class CreatePostDto
    {
        public int categoryId { get; set; }
        public required string title { get; set; }
        public required string content { get; set; }
    }

    public class UpdatePostDto
    {
        public int categoryId { get; set; }
        public string title { get; set; }
        public string content { get; set; }
    }
} 