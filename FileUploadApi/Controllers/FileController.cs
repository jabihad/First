using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using FileUploadApi.Services.Upload.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace First.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [Authorize]
        [HttpPost("UploadFile")]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files)
        {
            var res = await _fileService.Upload(files);

            if (res == 0)
                return BadRequest();
            else if (res == 1)
                return Ok("All the files are successfully uploaded.");

            return StatusCode(500, "Internal server error");
        }
        [Authorize]
        [HttpPost("DeleteFile")]
        public async Task<bool> DeleteFile(ICollection<IFormFile> files)
        {
            var res = await _fileService.DeleteFile(files);
            return res;
        }
    }
}