using AutoMapper;
using Entities.Models;
using FileUploadApi.Model;
using FileUploadApi.Repositories;
using FileUploadApi.Services.ServicePost.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FileUploadApi.Services.ServicePost.Implementation
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post> _postRepo;
        private readonly IRepository<Extension> _extensionRepo;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PostService(IRepository<Post> postRepo, IRepository<Extension> extensionRepo, IRepository<Category> categoryRepo, IHttpContextAccessor httpContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _postRepo = postRepo;
            _extensionRepo = extensionRepo;
            _categoryRepo = categoryRepo;
            _httpContext = httpContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> CreatePost(PostModel postModel)
        {
            try
            {
                var title = postModel.Title;
                if (await IsExist(title))
                {
                    return 5;// Title Exists
                }
                var files = postModel.File;
                var userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                var folderName = Path.Combine("StaticFiles", userId);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                var fileName = ContentDispositionHeaderValue.Parse(files.ContentDisposition).FileName.Trim('"');
                var extension = fileName.Substring(fileName.LastIndexOf('.') + 1);

                var permittedExtensions = await _extensionRepo.GetAllAsync();

                var isExtensionPermitted = permittedExtensions.FirstOrDefault(i => i.ExtensionName.ToLower() == extension.ToLower());
                if (isExtensionPermitted == null)
                {
                    return 3;
                }
                double maxFileLimit = isExtensionPermitted.MaxSize * 1048576; // mb to byte
                if (files.Length > maxFileLimit)
                {
                    return 4;
                }
                var fileNameWithOutExtension = fileName.Replace(fileName.Substring(fileName.LastIndexOf('.')), "");

                var changedFileName = fileNameWithOutExtension + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "." + extension;
                var fullPath = Path.Combine(pathToSave, changedFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    files.CopyTo(stream);
                    var model = new PostModel()
                    {
                        ImageUrl = userId + "\\" + changedFileName,
                        Title = postModel.Title,
                        TitleUrl = postModel.Title.Replace(' ', '-'),
                        Text = postModel.Text,
                        CreatedTime = DateTime.UtcNow,
                        CategoryId = postModel.CategoryId,
                        UserId = userId
                    };
                    var res = _mapper.Map<Post>(model);
                    var s = await _postRepo.CreateAsync(res);
                }
                return 1;// Ok("All the files are successfully uploaded.");
            }
            catch (Exception ex)
            {
                return 2;// StatusCode(500, "Internal server error");
            }
        }
        private async Task<bool> IsExist(string title)
        {
            var chk = await _postRepo.FindAsync(e => e.Title == title);
            return chk == null ? false : true;
        }

        public async Task<int> DeletePost(int id)
        {
            try
            {
                var post = await _postRepo.FindAsync(p => p.Id == id);
                var userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                var folderName = Path.Combine("StaticFiles", userId);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", post.ImageUrl);
                if ((System.IO.File.Exists(path)))
                {
                    System.IO.File.Delete(path);
                }
                var res = await _postRepo.DeleteAsync(p => p.Id == id);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<PostModel>> GetAllPost(int pageIndex, int pageSize)
        {
            var userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var posts = await _postRepo.FindAllAsync(p => p.UserId == userId);
            var postModel = _mapper.Map<IEnumerable<PostModel>>(posts);
            //var res = postModel.Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize);
            var res = postModel.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return res;
        }
        public async Task<List<ResultModel>> GetAllPostByCategory()
        {
            List<ResultModel> resultModelList = new List<ResultModel>();
            ResultModel resultModel = new ResultModel();
            var userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var posts = await _postRepo.FindAllAsync(p => p.UserId == userId);
            var postModel = _mapper.Map<IEnumerable<PostModel>>(posts);
            var res = postModel.GroupBy(c => c.CategoryId).ToList();
            var hostAddress = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value;
            foreach (var item in res)
            {
                var category = await _categoryRepo.GetById(item.Key);

                resultModel = new ResultModel()
                {
                    CategoryName = category.Name,
                    DashboardPostDatas = item.Select(c => new DashboardPostData()
                    {
                        Id = c.Id,
                        Title = c.Title,
                        TitleUrl = c.TitleUrl,
                        Image = Path.Combine(hostAddress, "StaticFiles", c.ImageUrl),
                        Text = c.Text,
                        CreatedTime = c.CreatedTime
                    })
                    .Take(6)
                    .ToList()
                };
                resultModelList.Add(resultModel);
            }



            return resultModelList;
        }

        public async Task<PostModel> GetPostById(int id)
        {
            var hostAddress = _httpContext.HttpContext.Request.Scheme + "://" + _httpContext.HttpContext.Request.Host.Value;
            var userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var post = await _postRepo.FindAsync(p => p.UserId == userId && p.Id == id);
            if (post != null)
            {
                var postModel = _mapper.Map<PostModel>(post);
                postModel.ImageUrl = Path.Combine(hostAddress, "StaticFiles", postModel.ImageUrl);
                return postModel;
            }
            throw new Exception("Not Found");
        }
        public async Task<PostModel> GetPostByTitle(string title)
        {
            var hostAddress = _httpContext.HttpContext.Request.Scheme + "://" + _httpContext.HttpContext.Request.Host.Value;
            var userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var post = await _postRepo.FindAsync(p => p.UserId == userId && p.TitleUrl == title);
            if (post != null)
            {
                var postModel = _mapper.Map<PostModel>(post);
                postModel.ImageUrl = Path.Combine(hostAddress, "StaticFiles", postModel.ImageUrl);
                return postModel;
            }
            throw new Exception("Not Found");
        }

        public async Task<bool> UpdatePost(PostModel postModel)
        {
            var userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var post = await _postRepo.FindAsync(p => p.UserId == userId && p.Id == postModel.Id);

            post.Title = postModel.Title;
            post.Text = postModel.Text;

            await _postRepo.UpdateAsync(post);
            return true;
        }
    }

}
