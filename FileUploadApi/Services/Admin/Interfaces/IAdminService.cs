using Entities.Models;
using FileUploadApi.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Services.Admin.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<Extension>> GetExtension();
        Task<bool> CreateExtension(ExtensionModel extensionModel);
        Task<ExtensionModel> GetExtensionById(int id);
        Task<bool> EditExtension(ExtensionModel extensionModel);
        Task<int> DeleteExtension(ExtensionModel extensionModel);
        Task<IEnumerable<FileModel>> FileList();
        Task<bool> DeleteFile(FileModel fileModel);
    }
}
