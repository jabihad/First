using FileUploadApi.Services.AppUser.Interfaces;
using FileUploadApi.Services.Upload.Interfaces;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FileUploadApi.Services.Upload.Implementation
{
    public class FileService : IFileService
    {
        private readonly IAppUserService _appUserService;
        private readonly IHttpContextAccessor _httpContext;
        public FileService(IAppUserService appUserService, IHttpContextAccessor httpContext)
        {
            _appUserService = appUserService;
            _httpContext = httpContext;
        }
        //public async Task<int> Upload(ICollection<IFormFile> files)
        //{
        //    try
        //    {
        //        var currentEmail = _appUserService.GetuserEmail();
        //        //var currentId = _appUserService.GetuserId();

        //        var folderName = Path.Combine("StaticFiles", "Images");
        //        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        //        if (files.Count == 0)
        //        {
        //            return 0; // No File
        //        }

        //        foreach (var file in files)
        //        {
        //            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //            var fullPath = Path.Combine(pathToSave, fileName);
        //            var dbPath = Path.Combine(folderName, fileName);

        //            using (var stream = new FileStream(fullPath, FileMode.Create))
        //            {
        //                file.CopyTo(stream);
        //            }
        //        }

        //        return 1;// Ok("All the files are successfully uploaded.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return 2;// StatusCode(500, "Internal server error");
        //    }
        //}
        
        public async Task<int> Upload(ICollection<IFormFile> files)
        {
            try
            {
                var currentEmail = _appUserService.GetuserEmail();
                //var currentEmail = "jihad";
                var folderName = Path.Combine("StaticFiles", currentEmail);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (files.Count == 0)
                {
                    return 0; // No File
                }
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                foreach (var file in files)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                return 1;// Ok("All the files are successfully uploaded.");
            }
            catch (Exception ex)
            {
                return 2;// StatusCode(500, "Internal server error");
            }
        }
        public async Task<bool> DeleteFile(ICollection<IFormFile> files)
        {
            try
            {
                var currentEmail = _appUserService.GetuserEmail();
                //var currentEmail = "jihad";
                var folderName = Path.Combine("StaticFiles", currentEmail);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                foreach (var file in files)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    if ((System.IO.File.Exists(fullPath)))
                    {
                        System.IO.File.Delete(fullPath);     
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }
}
