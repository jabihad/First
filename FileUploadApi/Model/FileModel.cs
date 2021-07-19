using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Model
{
    public class FileModel
    {
        public int Id { get; set; }
        public string FileUrl { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
