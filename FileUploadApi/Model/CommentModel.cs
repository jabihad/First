using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Model
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string CreatedTime { get; set; }
        public string UserName { get; set; }
        public string PostTitleUrl { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
