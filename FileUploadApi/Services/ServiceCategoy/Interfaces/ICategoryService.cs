using FileUploadApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Services.ServiceCategoy.Interfaces
{
    public interface ICategoryService
    {
        Task<bool> CreateCategory(CategoryModel categoryModel);
        Task<CategoryModel> GetCategoryById(int id);
        Task<IEnumerable<CategoryModel>> GetAllCategory();
        Task<bool> UpdateCategory(CategoryModel categoryModel);
        Task<int> DeleteCategory(int id);
    }
}
