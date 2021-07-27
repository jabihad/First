using FileUploadApi.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Services.ServicePost.Interfaces
{
    public interface IPostService
    {
        Task<int> CreatePost(PostModel postModel);
        Task<PostModel> GetPostById(int id);
        Task<PostModel> GetPostByTitle(string title);
        Task<IEnumerable<PostModel>> GetAllPost(int pageIndex, int pageSize);
        Task<List<ResultModel>> GetAllPostByCategory();
        Task<bool> UpdatePost(PostModel postModel);
        Task<int> DeletePost(int id);
        Task<int> CreateComment(CommentModel commentModel);
        Task<int> DeleteComment(int id);
        Task<IEnumerable<CommentModel>> GetAllComment(string postTitleUrl);
        Task<bool> UpdateComment(CommentModel commentModel);
        Task<CommentModel> GetCommentById(int id);
    }
}
