using FileUploadApi.Model;
using FileUploadApi.Services.ServiceCategoy.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Controllers
{
    [Route("api/category")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryModel categoryModel)
        {
            var res = await _categoryService.CreateCategory(categoryModel);
            if (res)
            {
                return StatusCode(200);
            }

            return Ok("Not Created");
        }
        [HttpGet("GetCategoryById/{id}")]
        public async Task<CategoryModel> GetCategoryById(int id)
        {
            var categoryModel = await _categoryService.GetCategoryById(id);
            return categoryModel;
        }
        //[Authorize(Roles = "customer")]
        [AllowAnonymous]
        [HttpGet("GetAllCategory")]
        public async Task<IEnumerable<CategoryModel>> GetAllCategory()
        {
            var res = await _categoryService.GetAllCategory();
            return res;
        }
        [HttpPost("UpdateCategory")]
        public async Task<bool> UpdateCategory([FromBody]CategoryModel categoryModel)
        {
            var res = await _categoryService.UpdateCategory(categoryModel);
            return res;
        }
        [HttpGet("DeleteCategory/{id}")]
        public async Task<int> DeleteCategory(int id)
        {
            var res = await _categoryService.DeleteCategory(id);
            return res;
        }

    }
}
