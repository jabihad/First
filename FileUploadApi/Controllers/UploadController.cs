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
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _uploadService;

        public UploadController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }
        [Authorize]
        [HttpPost("UploadFile")]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files)
        {
            var res = await _uploadService.Upload(files);

            if (res == 0)
                return BadRequest();
            else if (res == 1)
                return Ok("All the files are successfully uploaded.");

            return StatusCode(500, "Internal server error");
        }
        [Authorize]
        [HttpPost("DeleteFile")]
        public async Task<bool> DeleteFile(string fileName)
        {
            var res = await _uploadService.DeleteFile(fileName);
            return res;
        }
    }
}