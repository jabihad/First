using AutoMapper;
using Entities.Models;
using FileUploadApi.Model;
using FileUploadApi.Repositories;
using FileUploadApi.Services.AppUser.Interfaces;
using FileUploadApi.Services.Upload.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using File = Entities.Models.File;

namespace FileUploadApi.Services.Upload.Implementation
{
    public class FileService : IFileService
    {
        private readonly IAppUserService _appUserService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IRepository<Extension> _extension;
        private readonly IRepository<File> _fileRepo;
        private readonly IMapper _mapper;
        public FileService(IAppUserService appUserService,
                           IHttpContextAccessor httpContext,
                           IRepository<Extension> extension,
                           IRepository<File> fileRepo,
                           IMapper mapper)
        {
            //_appUserService = appUserService;
            _httpContext = httpContext;
            _extension = extension;
            _fileRepo = fileRepo;
            _mapper = mapper;
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
                var userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                var folderName = Path.Combine("StaticFiles", userId);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //var temp = "C://Users//BS512//source//repos//angular-authentication-identity//angular-identity-aspnetcore-security//AngularClient//src//assets";
                //var pathToSave = Path.Combine(temp, userId);

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
                    //
                    var extension = fileName.Substring(fileName.LastIndexOf('.') + 1);

                    var permittedExtensions = await _extension.GetAllAsync();

                    var isExtensionPermitted = permittedExtensions.FirstOrDefault(i => i.ExtensionName.ToLower() == extension.ToLower());
                    if(isExtensionPermitted == null)
                    {
                        return 3;
                    }
                    double maxFileLimit = isExtensionPermitted.MaxSize * 1048576; // mb to byte
                    if (file.Length > maxFileLimit)
                    {
                        return 4;
                    }
                    var fileNameWithOutExtension = fileName.Replace(fileName.Substring(fileName.LastIndexOf('.')), "");

                    var changedFileName = fileNameWithOutExtension + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "." + extension;
                    var fullPath = Path.Combine(pathToSave, changedFileName);
                    //var fullPath2 = fullPath.;
                    //var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        var fileModel = new FileModel()
                        {
                            FileUrl = userId + "\\" + changedFileName,
                            UserId = userId,
                            CreatedTime = DateTime.UtcNow
                        };
                        var res = _mapper.Map<File>(fileModel);
                        var s = await _fileRepo.CreateAsync(res);
                    }
                }

                return 1;// Ok("All the files are successfully uploaded.");
            }
            catch (Exception ex)
            {
                return 2;// StatusCode(500, "Internal server error");
            }
        }  
        public async Task<bool> DeleteFile(int id)
        {
            var file = await _fileRepo.FindAsync(f=>f.Id==id);
            if (file.FileUrl != null)
            {
                var userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                var folderName = Path.Combine("StaticFiles", userId);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", file.FileUrl);
                //var temp = "C://Users//BS512//source//repos//angular-authentication-identity//angular-identity-aspnetcore-security//AngularClient//src//assets";
                //var path = Path.Combine(temp, userId, file.FileUrl);
                if ((System.IO.File.Exists(path)))
                {
                    var res = await _fileRepo.DeleteAsync(f => f.Id==file.Id);
                    System.IO.File.Delete(path);
                    
                }
            }

            return true;
        }
        public async Task<IEnumerable<FileModel>> FileList()
        {
            var userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _fileRepo.FindAllAsync(u => u.UserId == userId);
            var fileModel = _mapper.Map<IEnumerable<FileModel>>(result);

            return fileModel;
        }
        public async Task<FileModel> GetFileById(int id)
        {
            var userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _fileRepo.FindAsync(f=>f.Id==id);
            var file = _mapper.Map<FileModel>(res);
            return file;
        }
    }
}
