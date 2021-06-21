using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Model
{
    public class ExtensionModel
    {
        public int Id { get; set; }
        public string ExtensionName { get; set; }
        public double MaxSize { get; set; }
    }
}
