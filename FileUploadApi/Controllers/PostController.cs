using AutoMapper;
using Entities.Models;
using FileUploadApi.Model;
using FileUploadApi.Repositories;
using FileUploadApi.Services.ServicePost.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postSvc;
        public PostController(IPostService postSvc)
        {
            _postSvc = postSvc;
        }
        [Authorize]
        [HttpPost("CreatePost")]
        public async Task<IActionResult> CreatePost([FromForm] PostModel postModel)
        {
            var res = await _postSvc.CreatePost(postModel);
            if (res == 0)
                return BadRequest();
            else if (res == 1)
                return Ok(new { message = "Post is successfully created" });
            else if (res == 3)
                return StatusCode(415, "File type is not supported");
            else if (res == 4)
                return StatusCode(413, "File Size is too big!!!");
            return StatusCode(500, "Internal server error");
        }
        [Authorize]
        [HttpPost("DeletePost/{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var res = await _postSvc.DeletePost(id);
            if (res == 1)
                return Ok(new { message = "Post is deleted" });
            else
                return Ok(new { message = "Post is not deleted" });
        }
        [Authorize]
        [HttpGet("GetAllPost/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetAllPost(int pageIndex, int pageSize)
        {
            try
            {
                var res = await _postSvc.GetAllPost(pageIndex, pageSize);
                return Ok(res);
            }
            catch
            {
                return Ok(new { message = "Can't get Post" });
            }
        }

        [Authorize]
        [HttpGet("GetAllPostByCategory")]
        public async Task<IActionResult> GetAllPostByCategory()
        {
            try
            {
                var res = await _postSvc.GetAllPostByCategory();
                return Ok(res);
            }
            catch
            {
                return Ok(new { message = "Can't get Post" });
            }
        }

        [Authorize]
        [HttpGet("GetPostById/{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            try
            {
                var res = await _postSvc.GetPostById(id);
                return Ok(res);
            }
            catch(Exception ex)
            {
                //return ExceptionResult(ex);
                return Ok(new { message = "Can't get Post"});
            }
        }
        [Authorize]
        [HttpPost("UpdatePost")]
        public async Task<IActionResult> UpdatePost([FromBody] PostModel postModel)
        {
            try
            {
                var res = await _postSvc.UpdatePost(postModel);
                return Ok(res);
            }
            catch
            {
                return Ok(new { message = "Can't Update Post" });
            }
        }

    }
}
