using AutoMapper;
using Entities.Models;
using FileUploadApi.Model;
using FileUploadApi.Repositories;
using FileUploadApi.Services.Admin.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using File = Entities.Models.File;

namespace FileUploadApi.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminController : ControllerBase
    {
        private readonly IRepository<Extension> _extension;
        private readonly IRepository<File> _file;
        private readonly IRepository<User> _user;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IAdminService _adminService;
        public AdminController(IRepository<Extension> extension,
            IRepository<File> file, IRepository<User> user,
            UserManager<User> userManager,
            IMapper mapper,
            IAdminService adminService
            )
        {
            _extension = extension;
            _file = file;
            _user = user;
            _userManager = userManager;
            _mapper = mapper;
            _adminService = adminService;
        }
        [HttpGet]
        [Route("GetExtension")]
        public async Task<IEnumerable<Extension>> GetExtension()
        {
            //var result = await _extension.GetAllIncludeAsync(
            //                             x => x,
            //                             null,
            //                             x => x.OrderBy(y => y.ExtensionName),
            //                             null,
            //                             true
            //                             );
            var result = await _adminService.GetExtension();
            return result;
        }
        [HttpPost("CreateExtension")]
        public async Task<IActionResult> CreateExtension([FromBody]ExtensionModel extensionModel)
        {
            //var res = _mapper.Map<Extension>(extensionModel);
            //var chk = await IsExist(extensionModel.ExtensionName);
            //if (!chk)
            //{
            //    await _extension.CreateAsync(res);
            //}
            //return StatusCode(200);
            var res = await  _adminService.CreateExtension(extensionModel);
            
            return StatusCode(200);
        }
        //private async Task<bool> IsExist(string extension)
        //{
        //    var ext = await _extension.FindAsync(e => e.ExtensionName.ToLower() == extension.ToLower());
        //    return ext == null ? false : true;
        //}
        [HttpGet("GetExtensionById/{id}")]
        public async Task<ExtensionModel> GetExtensionById(int id)
        {
            //var model = await _extension.FindAsync(f => f.Id == id);
            //var res = _mapper.Map<ExtensionModel>(model);
            //return res;
            var res = await _adminService.GetExtensionById(id);
            return res;

        }

        [HttpPost("EditExtension")]
        public async Task<IActionResult> EditExtension([FromBody] ExtensionModel extensionModel)
        {
            //var ext = await _extension.FindAsync(e => e.Id == extensionModel.Id);
            //ext.ExtensionName = extensionModel.ExtensionName;
            //ext.MaxSize = (double)extensionModel.MaxSize;

            //await _extension.UpdateAsync(ext);
            var res = await _adminService.EditExtension(extensionModel);

            return StatusCode(200);
        }
        [HttpPost("DeleteExtension")]
        public async Task<int> DeleteExtension([FromBody] ExtensionModel extensionModel)
        {
            //var id = extensionModel.Id;
            //return await _extension.DeleteAsync(e => e.Id == id);
            return await _adminService.DeleteExtension(extensionModel);
        }
        //[HttpPost("DeleteFiles")]
        //public async Task<IActionResult> DeleteFiles()
        //{
        //    var users = await _user.GetAllAsync();
        //    foreach (var user in users)
        //    {
        //        var folderName = Path.Combine("StaticFiles", user.Email);
        //        var fullFolderName = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //        if ((Directory.Exists(fullFolderName)))
        //        {
        //            Directory.Delete(fullFolderName, true);
        //        }
        //    }
        //    return StatusCode(200);
        //}
        [HttpGet("FileList")]
        public async Task<IEnumerable<FileModel>> FileList()
        {
            //var file = await _file.GetAllAsync();
            //var fileModel = _mapper.Map<IEnumerable<FileModel>>(file);
            var fileModel = await _adminService.FileList();
            return fileModel;
        }
        [HttpPost("DeleteFile")]
        public async Task<bool> DeleteFile([FromBody] FileModel fileModel)
        {
            //var id = fileModel.Id;
            //var res = await _file.DeleteAsync(f => f.Id == id);
            var res = await _adminService.DeleteFile(fileModel);
            return true;
        }
    }
}
