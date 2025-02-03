using System;
using System.ComponentModel.DataAnnotations;

namespace MentalHealth.Models
{
    public class Comment
    {
        public int id { get; set; }
        public int postId { get; set; }
        public required string content { get; set; }
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property for Post has been removed
    }
} 