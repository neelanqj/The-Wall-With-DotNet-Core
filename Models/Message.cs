using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models;

namespace The_Wall_With_DotNet_Core.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Content { get; set; }
        public List<Comment> Comments {get;set;}
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}