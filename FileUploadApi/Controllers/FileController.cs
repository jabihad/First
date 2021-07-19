using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using FileUploadApi.Model;
using FileUploadApi.Services.Upload.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FileController(IFileService fileService, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment)
        {
            _fileService = fileService;
            _httpContextAccessor = httpContextAccessor;
           _hostingEnvironment = hostingEnvironment;
        }
        [Authorize]
        [HttpPost("UploadFile")]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files)
        {
            var res = await _fileService.Upload(files);

            if (res == 0)
                return BadRequest();
            else if (res == 1)
                //return StatusCode(200, "All the files are successfully uploaded.");
                return Ok(new { message = "File is successfully Uploaded" });
                //return Ok(new { name = "Fabio", age = 42, gender = "M" });
                //return Json(new { name = "Fabio", age = 42, gender = "M" });
            else if (res == 3)
                return StatusCode(415, "File type is not supported");
            else if (res == 4)
                return StatusCode(413, "File Size is too big!!!");
            return StatusCode(500, "Internal server error");
        }
        /*[Authorize]
        [HttpPost("DeleteFile")]
        public async Task<bool> DeleteFile(ICollection<IFormFile> files)
        {
            var res = await _fileService.DeleteFile(files);
            return res;
        }*/
        [Authorize]
        [HttpPost("DeleteFile")]
        public async Task<bool> DeleteFile([FromBody]FileModel fileModel)
        {
            var id = fileModel.Id;
            var res = await _fileService.DeleteFile(id);
            return res;
        }
        [Authorize]
        [HttpGet("FileList")]
        public async Task<IEnumerable<FileModel>> FileList()
        {
            var res = await _fileService.FileList();
            return res;
        }
        [Authorize]
        [HttpGet("GetFileById/{id}")]
        public async Task<FileModel> GetFileById(int id)
        {
            var r = _hostingEnvironment.WebRootPath;
            var hostAddress = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value;
            var res = await _fileService.GetFileById(id);
            res.FileUrl = Path.Combine(hostAddress, "StaticFiles", res.FileUrl);
            return res;
        }


    }
}