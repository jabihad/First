using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Model
{
    public class PostModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile File { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreatedTime { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }
    }
}
