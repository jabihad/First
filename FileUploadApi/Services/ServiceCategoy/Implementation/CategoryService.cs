using AutoMapper;
using Entities.Models;
using FileUploadApi.Model;
using FileUploadApi.Repositories;
using FileUploadApi.Services.ServiceCategoy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Services.ServiceCategoy.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _category;
        private readonly IMapper _mapper;
        public CategoryService(IRepository<Category> category, IMapper mapper)
        {
            _category = category;
            _mapper = mapper;
        }
        public async Task<bool> CreateCategory(CategoryModel categoryModel)
        {
            var category = _mapper.Map<Category>(categoryModel);
            var chk = await IsExist(categoryModel.Name);
            if (!chk)
            {
                await _category.CreateAsync(category);
                return true;
            }
            return false;
        }
        private async Task<bool> IsExist(string categoryName)
        {
            var cat = await _category.FindAsync(c => c.Name.ToLower() == categoryName.ToLower());
            return cat == null ? false : true;
        }
        public async Task<CategoryModel> GetCategoryById(int id)
        {
            var category = await _category.FindAsync(c => c.Id == id);
            var categoryModel = _mapper.Map<CategoryModel>(category);
            return categoryModel;
        }

        public async Task<IEnumerable<CategoryModel>> GetAllCategory()
        {
            var res = await _category.GetAllAsync();
            var model = _mapper.Map<IEnumerable<CategoryModel>>(res);
            return model;
        }

        public async Task<bool> UpdateCategory(CategoryModel categoryModel)
        {
            var category = await _category.FindAsync(c => c.Id == categoryModel.Id);
            category.Name = categoryModel.Name;
            category.Description = categoryModel.Description;

            await _category.UpdateAsync(category);
            return true;
        }

        public async Task<int> DeleteCategory(int id)
        {
            var res = await _category.DeleteAsync(c => c.Id == id);
            return res;
        }
    }
}
