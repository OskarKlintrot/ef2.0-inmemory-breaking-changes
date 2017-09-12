using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InMemoryIssue.Domain.Models.Entities
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
