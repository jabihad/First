using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Services.Upload.Interfaces
{
    public interface IFileService
    {
        Task<int> Upload(ICollection<IFormFile> files);
        Task<bool> DeleteFile(ICollection<IFormFile> files);
    }
}
