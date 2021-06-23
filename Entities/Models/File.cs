using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class File
    {
        public int Id { get; set; }
        public string FileUrl { get; set; }
        public string UserId { get; set; }
        //[ForeignKey("UserId")]
        public User User { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
