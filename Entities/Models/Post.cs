using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string TitleUrl { get; set; }
        public string Text { get; set; }
        public DateTime CreatedTime { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
