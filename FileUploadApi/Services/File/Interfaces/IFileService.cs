using Entities.Models;
using FileUploadApi.Model;
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
        //Task<bool> DeleteFile(ICollection<IFormFile> files);
        Task<bool> DeleteFile(int id);
        Task<IEnumerable<FileModel>> FileList();
        Task<FileModel> GetFileById(int id);
    }
}
