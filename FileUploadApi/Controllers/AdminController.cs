using AutoMapper;
using Entities.Models;
using FileUploadApi.Model;
using FileUploadApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminController : ControllerBase
    {
        private readonly IRepository<Extension> _extension;
        private readonly IRepository<User> _user;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public AdminController(IRepository<Extension> extension, IRepository<User> user, UserManager<User> userManager, IMapper mapper)
        {
            _extension = extension;
            _user = user;
            _userManager = userManager;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("GetExtension")]
        public async Task<IEnumerable<Extension>> GetExtension()
        {
            var result = await _extension.GetAllIncludeAsync(
                                         x => x,
                                         null,
                                         x => x.OrderBy(y => y.ExtensionName),
                                         null,
                                         true
                                         );
            return result;
        }
        [HttpPost("CreateExtension")]
        public async Task<IActionResult> CreateExtension(ExtensionModel extensionModel)
        {
            var res = _mapper.Map<Extension>(extensionModel);
            var chk = await IsExist(extensionModel.ExtensionName);
            if (!chk)
            {
                await _extension.CreateAsync(res);
            }
            return StatusCode(200);
        }
        private async Task<bool> IsExist(string extension)
        {
            var ext = await _extension.FindAsync(e => e.ExtensionName.ToLower() == extension.ToLower());
            return ext == null ? false : true;
        }
        [HttpPost("EditExtension")]
        public async Task<IActionResult> EditExtension(ExtensionModel extensionModel)
        {
            var ext = await _extension.FindAsync(e => e.Id == extensionModel.Id);
            ext.ExtensionName = extensionModel.ExtensionName;
            ext.MaxSize = (double)extensionModel.MaxSize;

            await _extension.UpdateAsync(ext);

            return StatusCode(200);
        }
        [HttpPost("DeleteExtension/{id}")]
        public async Task<int> DeleteExtension(int id)
        {
            return await _extension.DeleteAsync(e => e.Id == id);
        }
        [HttpPost("DeleteFiles")]
        public async Task<IActionResult> DeleteFiles()
        {
            var users = await _user.GetAllAsync();
            foreach (var user in users)
            {
                var folderName = Path.Combine("StaticFiles", user.Email);
                var fullFolderName = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if ((Directory.Exists(fullFolderName)))
                {
                    Directory.Delete(fullFolderName, true);
                }
            }
            return StatusCode(200);
        }
    }
}
