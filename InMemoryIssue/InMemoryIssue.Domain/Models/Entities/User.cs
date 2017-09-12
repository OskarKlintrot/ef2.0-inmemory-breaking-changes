using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InMemoryIssue.Domain.Models.Entities
{
    public class User
    {
        [Key]
        public string Username { get; set; }

        [Required]
        public string DisplayName { get; set; }
    }
}
