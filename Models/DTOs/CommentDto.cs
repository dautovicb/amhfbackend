namespace MentalHealth.Models.DTOs
{
    public class CreateCommentDto
    {
        public int postId { get; set; }
        public required string content { get; set; }
    }

    public class UpdateCommentDto
    {
        public required string content { get; set; }
    }
} 