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
            var result = await _adminService.GetExtension();
            return result;
        }
        [HttpPost("CreateExtension")]
        public async Task<IActionResult> CreateExtension([FromBody] ExtensionModel extensionModel)
        {
            try
            {
                var res = await _adminService.CreateExtension(extensionModel);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return Ok(new { message = "Can't Create Extension" });
            }
        }
        [HttpGet("GetExtensionById/{id}")]
        public async Task<ExtensionModel> GetExtensionById(int id)
        {
            var res = await _adminService.GetExtensionById(id);
            return res;

        }

        [HttpPost("EditExtension")]
        public async Task<IActionResult> EditExtension([FromBody] ExtensionModel extensionModel)
        {
            var res = await _adminService.EditExtension(extensionModel);
            return StatusCode(200);
        }
        [HttpPost("DeleteExtension")]
        public async Task<int> DeleteExtension([FromBody] ExtensionModel extensionModel)
        {
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
            var fileModel = await _adminService.FileList();
            return fileModel;
        }
        [HttpPost("DeleteFile")]
        public async Task<bool> DeleteFile([FromBody] FileModel fileModel)
        {
            var res = await _adminService.DeleteFile(fileModel);
            return true;
        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserModel userModel)
        {
            var res = await _adminService.CreateUser(userModel);
            if (res)
            {
                return Ok(new { message = "User is Created" });
            }
            return Ok(new { message = "User is not created" });
        }

        [HttpGet("GetAllUsers")]
        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            var res = await _adminService.GetAllUsers();
            return res;
        }
        [HttpGet("GetUserById/{id}")]
        public async Task<UserModel> GetUserById(string id)
        {
            var res = await _adminService.GetUserById(id);
            return res;
        }


        [HttpGet("GetAllRoles")]
        public IEnumerable<IdentityRole> GetAllRoles()
        {
            var res = _adminService.GetAllRoles();
            return res;
        }
        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] UserModel userModel)
        {
            var res = await _adminService.DeleteUser(userModel);
            if (res == true)
                return Ok(new { message = "User is Deleted" });

            return Ok(new { message = "User is not deleted" });
        }
        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserModel userModel)
        {
            var res = await _adminService.UpdateUser(userModel);
            return StatusCode(200);
        }
    }
}
