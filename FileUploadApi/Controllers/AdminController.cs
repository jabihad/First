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
    //[ApiController]
    [Authorize(Roles = "admin")]
    public class AdminController : ControllerBase
    {
        private readonly IRepository<Extension> _extension;
        private readonly IRepository<User> _user;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        public AdminController(IRepository<Extension> extension, IRepository<User> user, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _extension = extension;
            _user = user;
            _userManager = userManager;
            _mapper = mapper;
        }
        [HttpGet("GetExtension")]
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
            /*foreach (var item in model)
            {
                var extension = await _context.Extensions.FindAsync(item.Id);
                extension.IsSelected = item.IsSelected;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("index", "home");*/
            return StatusCode(200);
        }
        private async Task<bool> IsExist(string extension)
        {
            //var ext = _context.Extensions.Where(e => e.ExtensionName.ToLower() == extesion.ToLower()).FirstOrDefault();
            var ext = await _extension.FindAsync(e => e.ExtensionName.ToLower() == extension.ToLower());
            return ext == null ? false : true;
        }
        [HttpPost("DeleteEextension")]
        public async Task<int> DeleteExtension(int id)
        {
            return await _extension.DeleteAsync(e => e.Id == id);
            //var ext = _context.Extensions.FirstOrDefault(e => e.Id == id);
            //if (ext == null) return RedirectToAction("index", "home");

            //_context.Extensions.Remove(ext);
            //_context.SaveChanges();
            //return RedirectToAction("index", "home");
        }
        [HttpPost("DeleteFiles")]
        public async Task<IActionResult> DeleteFiles()
        {
            var users = await _user.GetAllAsync();
            foreach (var user in users)
            {
                var folderName = Path.Combine("StaticFiles", user.Email);
                if ((System.IO.File.Exists(folderName)))
                {
                    System.IO.File.Delete(folderName);
                }
            }
            return StatusCode(200);
            /*var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            foreach (var file in files)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                if ((System.IO.File.Exists(fullPath)))
                {
                    System.IO.File.Delete(fullPath);
                }
            }*/
            /*var totalFile = _context.FileStatus.Count(i => i.Status == Status.NotDeleted);
            var myTimer = new Stopwatch();
            myTimer.Start();
            _logger.LogInformation($"Timer start for Deleting {totalFile} files");

            //var users = _context.FileStatus.Where(f => f.Status == Status.NotDeleted).Select(f => f.UserId).Distinct().ToList();
            var users = _userManager.Users.Select(u => u.Id).ToList();
            var files = _context.FileStatus.ToList();
            bool deleteRecursively = true;
            foreach (var user in users)
            {
                string existingPath = Path.Combine(_hostingEnvironment.WebRootPath, "FileUpload", user);
                var filesPerUser = files.Where(f => f.UserId == user && f.Status == Status.NotDeleted).ToList();
                if (Directory.Exists(existingPath))
                {
                    Directory.Delete(existingPath, deleteRecursively);
                }
                foreach (var item in filesPerUser)
                {
                    item.Status = Status.Deleted;
                }
                _context.UpdateRange(filesPerUser);
                await _context.SaveChangesAsync();

            }
            myTimer.Stop();
            _logger.LogInformation($"Time taken to delete {totalFile}: {myTimer.ElapsedMilliseconds} ms");
            return RedirectToAction("index");*/
        }

    }
}
