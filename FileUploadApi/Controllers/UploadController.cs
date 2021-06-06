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
        //[Authorize]
        //[HttpPost, DisableRequestSizeLimit]
        //public IActionResult Upload(ICollection<IFormFile> files)
        //{
        //    try
        //    {
        //        var folderName = Path.Combine("StaticFiles", "Images");
        //        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        //        if (files.Count == 0)
        //        {
        //            return BadRequest();
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

        //        return Ok("All the files are successfully uploaded.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal server error");
        //    }
        //}
        [Authorize]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files)
        {
            var res = await _uploadService.Upload(files);

            if (res == 0)
                return BadRequest();
            else if (res == 1)
                return Ok("All the files are successfully uploaded.");

            return StatusCode(500, "Internal server error");
        }
    }
}