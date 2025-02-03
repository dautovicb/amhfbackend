using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MentalHealth.Models
{
    [Index(nameof(name), IsUnique = true)]
    public class Category
    {
        public Category()
        {
        }

        public int id { get; set; }
        public required string name { get; set; }
    }
} 