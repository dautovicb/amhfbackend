using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MentalHealth.Models
{
	public class Post
	{
		public Post()
		{
			Comments = new List<Comment>();
		}

		public int id { get; set; }
		public int categoryId { get; set; }
		public required string title { get; set; }
		public required string content { get; set; }
		public DateTime createdAt { get; set; } = DateTime.UtcNow;
        
		// Navigation properties
		public virtual Category? Category { get; set; }
		public virtual ICollection<Comment> Comments { get; set; }
	}
}
