using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string CreatedTime { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
